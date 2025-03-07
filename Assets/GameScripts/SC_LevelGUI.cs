using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_LevelGUI : MonoBehaviour
{

    public TMP_Text content;
    public GameObject player;

    public GameObject gameManager;
    GameManager gameManagerScript;

    private const string LEVEL = "Level ";
    private const string XP = "XP ";

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        if(gameManagerScript.getList() == null)
        {
            print("Fixing");
            gameManagerScript.fixList();
        }

        print(gameManagerScript.getList().Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManagerScript.getLevel() == 10)
        {
            content.text = LEVEL + " 10 (MAX)";
        }
        else
        {
            content.text = LEVEL + gameManagerScript.getLevel() + " " + XP + gameManagerScript.getXP() + "/" + gameManagerScript.getList()[gameManagerScript.getLevel()].Value;
        }
        content.text += "\nMONEY: " + gameManagerScript.getMoney();
    }
}
