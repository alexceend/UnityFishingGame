using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTween : MonoBehaviour
{
    public GameObject canvas;
    public Image spriteImg;
    public Sprite sprite;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (spriteImg != null && sprite != null)
        {
            spriteImg.sprite = sprite;
        }

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        FadeIn();
    }

    void FadeIn()
    {
        canvasGroup.alpha = 0; // Start invisible
        LeanTween.alphaCanvas(canvasGroup, 1f, 1f).setOnComplete(MoveBanner);
    }

    void MoveBanner()
    {
        GameObject[] banners = GameObject.FindGameObjectsWithTag("BannerImage");
        GameObject[] fishes = GameObject.FindGameObjectsWithTag("FishImage");

        if (banners.Length == 0 || fishes.Length == 0)
        {
            Debug.LogWarning("No banners or fish images found!");
            return;
        }

        // Get the width of the first banner as a reference
        float bannerWidth = banners[0].GetComponent<RectTransform>().rect.width;

        float bannerTargetX = -800f; // Destination X position for banners
        float fishTargetX = -800f;    // Destination X position for fish

        float moveDuration = 5f; // Base duration for movement

        foreach (GameObject banner in banners)
        {
            // Move the banner
            LeanTween.moveX(banner, bannerTargetX, moveDuration).setOnComplete(FadeOut);
            LeanTween.moveY(banner, banner.transform.position.y + 10f, 0.5f).setLoopPingPong(4);

        }

        foreach (GameObject fish in fishes)
        {
            float fishWidth = fish.GetComponent<RectTransform>().rect.width; // Get fish width
            float widthRatio = fishWidth / bannerWidth; // Calculate movement proportion

            float adjustedDuration = moveDuration * widthRatio; // Adjust duration based on width

            // Move the fish
            LeanTween.moveX(fish, fishTargetX, adjustedDuration).setOnComplete(FadeOut);
            LeanTween.moveY(fish, fish.transform.position.y + 10f, 0.5f).setLoopPingPong(4);

        }
    }



    void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setOnComplete(DestroyMe);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
