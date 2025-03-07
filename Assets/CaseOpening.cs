using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseOpening : MonoBehaviour
{
    // Start is called before the first frame update

    public Canvas Canvas;
    public RectTransform itemRow; // The UI row that scrolls
    public Image[] itemImages;
    public Image finalSkinImage; // Displays the won skin
    public Image finalSkinImageBG; // Displays the won skin
    public Sprite[] skinSprites; // Array of skin images

    public RectTransform viewport; // Reference to the viewport RectTransform


    private RectTransform[] items; // Converted RectTransforms
    private bool isSpinning = false;
    private float spinSpeed = 50000f; // Adjust speed
    private float deceleration = 0.992f; // Slows down effect
    private float itemWidth = 120f;
    private float totalSpinTime = 3.0f;

    private int chosenIndex;


    void Start()
    {

        Canvas.enabled = false;
        finalSkinImage.enabled = false;
        finalSkinImageBG.enabled = false;

        items = new RectTransform[itemImages.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = itemImages[i].GetComponent<RectTransform>();
        }
    }


    public int StartCaseOpening()
    {
        Canvas.enabled = true;
        if (!isSpinning)
        {
            StartCoroutine(SpinCase());
        }
        return chosenIndex;
    }

    IEnumerator SpinCase()
    {

        yield return new WaitForSeconds(1.0f);

        isSpinning = true;
        chosenIndex = Random.Range(0, skinSprites.Length);
        Debug.Log(chosenIndex);

        float elapsedTime = 0f;

        while (elapsedTime < totalSpinTime)
        {
            itemRow.anchoredPosition += Vector2.left * spinSpeed * Time.deltaTime;
            spinSpeed *= deceleration;

            // Ensure it doesn't stop too early
            if (spinSpeed < 200f)
            {
                spinSpeed = 200f; // Keep a minimum speed before stopping
            }

            foreach (RectTransform item in items)
            {
                float globalX = item.anchoredPosition.x + itemRow.anchoredPosition.x;
                if (globalX < -itemWidth)
                {
                    float rightMostX = FindRightMostItemX();
                    item.anchoredPosition = new Vector2(rightMostX + itemWidth, 
                        item.anchoredPosition.y);
                }
            }

            elapsedTime += Time.deltaTime;
            Debug.Log(IsItemCentered(chosenIndex));
            yield return null;
        }

        if (!IsItemCentered(chosenIndex))
        { 
            yield return StartCoroutine(CenterItem(chosenIndex));
        }

        finalSkinImage.sprite = skinSprites[chosenIndex];

        isSpinning = false;
        finalSkinImage.enabled = true;
        finalSkinImageBG.enabled = true;

        yield return new WaitForSeconds(3.0f);

        finalSkinImage.enabled = false;
        finalSkinImageBG.enabled = false;
        Canvas.enabled = false;

    }

    bool IsItemCentered(int index)
    {
        // Calculate the global position of the chosen item
        float itemGlobalX = items[index].anchoredPosition.x + itemRow.anchoredPosition.x;

        // Calculate the center position of the visible area of the itemRow
        float viewportCenterX = GetRollingViewportCenterX();
        // Calculate the boundaries for being considered "centered"
        float halfItemWidth = itemWidth / 2; // Half the width of an item
        float leftBoundary = viewportCenterX - halfItemWidth; // Left boundary of the centered area
        float rightBoundary = viewportCenterX + halfItemWidth; // Right boundary of the centered area

        // Check if the item is within the boundaries
        return itemGlobalX >= leftBoundary && itemGlobalX <= rightBoundary;
    }

    float GetRollingViewportCenterX()
    {
        // Get the width of the viewport
        float viewportWidth = viewport.rect.width;

        // Calculate the center position of the viewport
        return viewport.anchoredPosition.x + (viewportWidth / 2);
    }

    IEnumerator CenterItem(int index)
    {
        while (!IsItemCentered(index))
        {
            itemRow.anchoredPosition += Vector2.left * spinSpeed * Time.deltaTime;
            spinSpeed *= deceleration;

            // Ensure it doesn't stop too early
            if (spinSpeed < 200f)
            {
                spinSpeed = 200f; // Keep a minimum speed before stopping
            }

            foreach (RectTransform item in items)
            {
                float globalX = item.anchoredPosition.x + itemRow.anchoredPosition.x;
                if (globalX < -itemWidth)
                {
                    float rightMostX = FindRightMostItemX();
                    item.anchoredPosition = new Vector2(rightMostX + itemWidth,
                        item.anchoredPosition.y);
                }
            }

        }
        yield return null;
    }

    float FindRightMostItemX()
    {
        float maxX = float.MinValue;
        foreach (RectTransform item in items)
        {
            if (item.anchoredPosition.x > maxX)
            {
                maxX = item.anchoredPosition.x;
            }
        }
        return maxX;
    }

}
