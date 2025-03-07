using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public bool isFishing;
    public int level;

    private const int MAX_LVL = 10;

    public int xp;
    public double money;

    public List<KeyValuePair<int, int>> levelMap;

    public float fishing_rod_range_multiplier = 1.0f;
    public int fishing_rod_range_multiplier_counter = 0;

    public float money_multiplier = 1.0f;
    public int money_multiplier_counter = 0;

    public float xp_multiplier = 1.0f;
    public int xp_multiplier_counter = 0;


    public void changeFishingState()
    {
        isFishing = !isFishing;
    }

    public void decrementFishingRodMultiplierCounter()
    {
        if(fishing_rod_range_multiplier_counter != 0)
        {
            fishing_rod_range_multiplier_counter--;
        }

        if (fishing_rod_range_multiplier_counter == 0)
        {
            fishing_rod_range_multiplier = 1.0f;
        }
    }

    public void decrementMoneyMultiplierCounter()
    {
        if (money_multiplier_counter != 0)
        {
            money_multiplier_counter--;
        }

        if (money_multiplier_counter == 0)
        {
            money_multiplier = 1.0f;
        }
    }

    public void decrementXPMultiplierCounter()
    {
        if (xp_multiplier_counter != 0)
        {
            xp_multiplier_counter--;
        }

        if (xp_multiplier_counter == 0)
        {
            xp_multiplier = 1.0f;
        }
    }

    public void addXPAndMoney(int xpToAdd, float moneyToAdd)
    {
        if (level < MAX_LVL)
        {
            int xpToNextLevel = GetXPForNextLevel(level) - xp;
            if (xpToNextLevel >= xpToAdd)
            {
                xp += xpToAdd;
            }
            else
            {
                xp = xpToAdd - xpToNextLevel;
                level++;
            }
        }

        money += moneyToAdd;

        if(xpToAdd != 0 || moneyToAdd != 0)
        {
            decrementMoneyMultiplierCounter();
            decrementXPMultiplierCounter();
        }
    }

    private int GetXPForNextLevel(int currentLevel)
    {
        foreach (var entry in levelMap)
        {
            if (entry.Key == currentLevel + 1)
                return entry.Value;
        }
        return int.MaxValue; // Default to max if level not found
    }

    public void seedLevelHasHmap()
    {
        levelMap = new List<KeyValuePair<int, int>>();
        levelMap.Add(new KeyValuePair<int, int>(1, 100));
        levelMap.Add(new KeyValuePair<int, int>(2, 200));
        levelMap.Add(new KeyValuePair<int, int>(3, 300));
        levelMap.Add(new KeyValuePair<int, int>(4, 400));
        levelMap.Add(new KeyValuePair<int, int>(5, 500));
        levelMap.Add(new KeyValuePair<int, int>(6, 600));
        levelMap.Add(new KeyValuePair<int, int>(7, 700));
        levelMap.Add(new KeyValuePair<int, int>(8, 800));
        levelMap.Add(new KeyValuePair<int, int>(9, 900));
        levelMap.Add(new KeyValuePair<int, int>(10, 1000));
    }

    public int getLevel()
    {
        return level;
    }
}