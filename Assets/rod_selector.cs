using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rod_selector : MonoBehaviour
{
    public GameObject gamemanagerobj;
    private GameManager gamemanager;

    public Sprite normalSprite;
    public Sprite silverSprite;
    public Sprite goldenSprite;

    private Image rodImage;

    void Start()
    {
        gamemanager = gamemanagerobj.GetComponent<GameManager>();
        rodImage = GetComponent<Image>(); // Get the Image component of this UI object

        rodImage.enabled = true;

    }

    void Update()
    {
        if (gamemanager.getLevel() < 5)
        {
            rodImage.sprite = normalSprite; // Assign the sprite, not the Image component
        }
        else if (gamemanager.getLevel() < 10)
        {
            rodImage.sprite = silverSprite;
        }
        else
        {
            rodImage.sprite = goldenSprite;
        }
    }
}
