using System;
using System.Collections.Generic;

[Serializable]
public class Fish
{
    public int id;
    public string name;
    public string description;
    public string sprite;
    public int xp_given;
}

[Serializable]
public class FishCollection
{
    public List<Fish> common;
    public List<Fish> rare;
    public List<Fish> very_rare;
    public List<Fish> legendary;
}
