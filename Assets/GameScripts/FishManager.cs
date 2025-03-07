using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    public TextAsset jsonFile; // Drag your JSON file here in the Unity Inspector

    public SC_InventorySystem inventorySystem;
    public List<SC_Item> allFishItems; // Assign SC_Items in Inspector

    public GameObject encyclopedia;
    private EncyclopediaManager encyclopediaManager;

    public GameObject player;

    private FishCollection fishData;

    public GameObject gameManager;
    GameManager gameManagerScript;

    private Coroutine fishingCoroutine; // Referencia a la corrutina activa
    private bool isFishingCoroutineRunning = false; // Para evitar múltiples ejecuciones

    public GameObject bannerPrefab;
    GameObject banner;

    public GameObject audioSlotPrefab;
    public GameObject audioFishPrefab;


    //For the animation at fishing
    public GameObject mainCamera;
    public Transform topDownCameraPosition;

    private Vector3 originalCameraPosition; //To come back to the camera pos
    private Quaternion originalCameraRotation;

    public GameObject fishShadowPrefab;
    private GameObject fishShadow;

    public GameObject anzueloPrefab;
    private GameObject anzuelo;

    public float catchDistance = 5.0f;
    private bool isFishInRange = false; // Track whether the fish is in range

    private Vector3 centerBite = new Vector3(-11, -2.3f, 80);


    public GameObject cameraHolder;
    CameraMovement cameraMovement;

    PlayerData playerData;

    FISHING_ROD FISHING_ROD_MATERIAL;



    // Start is called before the first frame update
    void Start()
    {
        fishData = JsonUtility.FromJson<FishCollection>(jsonFile.text);
        gameManagerScript = gameManager.GetComponent<GameManager>();
        encyclopediaManager = encyclopedia.GetComponent<EncyclopediaManager>();

        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        cameraMovement = cameraHolder.GetComponent<CameraMovement>();

        topDownCameraPosition.transform.rotation = Quaternion.Euler(90, 0, 0);

        playerData = gameManagerScript.GetPlayerData();
    }

    void Update()
    {
        if (gameManagerScript.isFishing() && !isFishingCoroutineRunning)
        {
            StartCoroutine(FishingSequence());
        }
        else if (!gameManagerScript.isFishing() && isFishingCoroutineRunning) // Detener si deja de pescar
        {
            StopAllCoroutines();
            isFishingCoroutineRunning = false;
            ResetFishing();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            forceFish();
        }

        if(fishShadow != null)
        {
            Debug.Log(IsFishInRange());
        }

        if(playerData.getLevel() >= 10)
        {
            FISHING_ROD_MATERIAL = FISHING_ROD.GOLD;
        }else if(playerData.getLevel() >= 5)
        {
            FISHING_ROD_MATERIAL = FISHING_ROD.SILVER;
        }
        else
        {
            FISHING_ROD_MATERIAL = FISHING_ROD.NORMAL;
        }
    }

    IEnumerator FishingSequence()
    {

        originalCameraPosition = mainCamera.transform.position;

        if (isFishingCoroutineRunning)
        {
            Debug.LogWarning("FishingSequence is already running");
            yield break;
        }

        isFishingCoroutineRunning = true;
        isFishInRange = false;

        while (gameManagerScript.isFishing())
        {
            yield return new WaitForSeconds(5f);

            float p = Random.value;
            float fishingProbability = 0.3f + (0.1f * gameManagerScript.getLevel());

            Debug.Log($"Fishing attempt: p = {p}, fishingProbability = {fishingProbability}");

            if (p <= fishingProbability)
            {
                Debug.Log("Fish bite detected! Starting bite sequence...");

                DestroyInstances();

                yield return StartCoroutine(MoveCameraToTopDown());
                yield return StartCoroutine(SpawnBite());
                yield return StartCoroutine(SpawnFishShadow());
                StartCoroutine(MoveFishTowardsBite());

                isFishInRange = false;

                while (gameManagerScript.isFishing() && fishShadow != null)
                {

                    if (IsFishInRange())
                    {
                        if (!isFishInRange)
                        {
                            PlaySound(audioFishPrefab);
                            isFishInRange = true;
                        }
                    }
                    else
                    {
                        isFishInRange = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("Spacebar pressed");

                        if (IsFishInRange())
                        {
                            Debug.Log("Fish in range & spacebar");
                            CatchFish();
                        }
                        else
                        {
                            yield return StartCoroutine(MoveCameraToOriginalPosition());
                        }
                        UnlockCamera();

                        isFishInRange = false;
                        DestroyInstances();
                        break; // Exit the loop after catching the fish
                    }
                    yield return null;
                }
                ResetFishing();
                yield return StartCoroutine(MoveCameraToOriginalPosition());
                StopAllCoroutines();
            }
        }
        isFishingCoroutineRunning = false;
    }

    public void forceFish()
    {
        int rarity = Random.Range(0, 4); //Rehacer probabilidad pero mas tarde k me da toda la pereza
        Fish selectedFish = GetRandomFishByRarity(rarity);

        Debug.Log("Caught: " + selectedFish.name);

        SC_Item item = FindItemByID(selectedFish.id);
        if (item != null)
        {
            inventorySystem.AddItem(item);
        }
        else
        {
            Debug.LogError("No SC_Item found for fish ID: " + selectedFish.id);
        }

        gameManagerScript.changeFishingState();

        banner = Instantiate(bannerPrefab, new Vector3(0, 0, 0), new Quaternion());
        AnimationTween animationTween = banner.GetComponent<AnimationTween>();

        if (animationTween != null)
        {
            // Asignar el sprite del pez capturado
            animationTween.sprite = item.itemIcon;
        }

        if (!encyclopediaManager.IsFishCaught(selectedFish.id))
        {
            encyclopediaManager.MarkFishAsCaught(selectedFish.id);
        }
    }

    void CatchFish()
    {

        Debug.Log("Catch fish called");

        if (!inventorySystem.isFull())
        {
            int rarity = RandomRarity(Random.value);
            Fish selectedFish = GetRandomFishByRarity(rarity);

            Debug.Log("Caught: " + selectedFish.name);

            SC_Item item = FindItemByID(selectedFish.id);
            if (item != null)
            {
                inventorySystem.AddItem(item);
            }
            else
            {
                Debug.LogError("No SC_Item found for fish ID: " + selectedFish.id);
            }

            gameManagerScript.changeFishingState();

            banner = Instantiate(bannerPrefab, new Vector3(0, 0, 0), new Quaternion());
            AnimationTween animationTween = banner.GetComponent<AnimationTween>();

            if (animationTween != null)
            {
                // Asignar el sprite del pez capturado
                animationTween.sprite = item.itemIcon;
            }

            PlaySound(audioSlotPrefab);

            if (!encyclopediaManager.IsFishCaught(selectedFish.id))
            {
                encyclopediaManager.MarkFishAsCaught(selectedFish.id);
            }
            DestroyInstances();
            ResetFishing();
            gameManagerScript.setFishing(false);
            playerData.decrementFishingRodMultiplierCounter();
            gameManagerScript.SavePlayerData();
        }
    }

    int RandomRarity(float rng)
    {
        float legendary = 0.01f;
        float very_rare = 0.05f;
        float rare = 0.2f;

        SetProbabilities(ref legendary, ref very_rare, ref rare);

        if (rng < legendary)
        {
            return 4; 
        }
        else if (rng < very_rare)
        {
            return 3; 
        }
        else if (rng < rare)
        {
            return 2; 
        }
        else return 1; 
    }

    void SetProbabilities(ref float legendary, ref float very_rare, ref float rare)
    {
        if(FISHING_ROD_MATERIAL == FISHING_ROD.NORMAL)
        {
            legendary = 0.01f;
            very_rare = 0.05f;
            rare = 0.2f;
        }else if(FISHING_ROD_MATERIAL == FISHING_ROD.SILVER)
        {
            legendary = 0.03f;
            very_rare = 0.08f;
            rare = 0.23f;
        }else if(FISHING_ROD_MATERIAL == FISHING_ROD.GOLD)
        {
            legendary = 0.05f;
            very_rare = 0.12f;
            rare = 0.27f;
        }
    }

    public void PlaySound(GameObject prefab)
    {

        GameObject prefabt = Instantiate(prefab);
        if (prefabt == null)
        {
            Debug.LogError("Failed to instantiate audioPrefab!");
            return;
        }

        AudioSource ass = prefabt.GetComponent<AudioSource>();
        if (ass == null)
        {
            Debug.LogError("AudioSource component is missing on the prefab!");
            return;
        }

        ass.Play();

        Destroy(prefabt);
        Destroy(ass);
    }


    public Fish GetRandomFishByRarity(int rarity)
    {
        List<Fish> fishList;

        switch (rarity)
        {
            case 0:
                fishList = fishData.common;
                break;
            case 1:
                fishList = fishData.rare;
                break;
            case 2:
                fishList = fishData.very_rare;
                break;
            case 3:
                fishList = fishData.legendary;
                break;
            default:
                return null;
        }

        // Pick a random fish from the selected rarity list
        return fishList[Random.Range(0, fishList.Count)];
    }

    SC_Item FindItemByID(int id)
    {
        return allFishItems.Find(item => item.itemID == id);
    }


    //Camera things
    IEnumerator MoveCameraToTopDown()
    {
        cameraHolder.transform.rotation = Quaternion.Euler(60f, 0, 0);
        LockCamera();

        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        Debug.Log("Saved camera position = {" + originalCameraPosition.ToString() + "}");

        float duration = 1.0f;
        float elapsed = 0.0f;

        Vector3 targetPosition = topDownCameraPosition.position;
        Quaternion targetRotation = cameraHolder.transform.rotation; // Set rotation to (90, 0, 0)


        while (elapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, 
                targetPosition, elapsed / duration);
            mainCamera.transform.rotation = Quaternion.Slerp(originalCameraRotation, 
                targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;

        Debug.Log($"Top-down view transition complete: rotation = {mainCamera.transform.rotation.eulerAngles}");
    }


    Vector3 GetRandomPositionInCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle in radians
        float randomRadius = Random.Range(0f, radius); // Random radius within the circle

        // Convert polar coordinates to Cartesian coordinates
        float x = center.x + randomRadius * Mathf.Cos(angle);
        float z = center.z + randomRadius * Mathf.Sin(angle);

        return new Vector3(x, center.y, z);
    }

    //Instantiate the shadow
    IEnumerator SpawnFishShadow()
    {

        float radius = 20f;

        Vector3 fishPos = GetRandomPositionInCircle(centerBite, radius);

        fishShadow = Instantiate(fishShadowPrefab, fishPos, Quaternion.identity);
        yield return null;
    }

    IEnumerator MoveFishTowardsBite()
    {

        if (fishShadow == null)
        {
            Debug.LogError("Fish shadow is null in MoveFishTowardsBite");
            yield break; // Exit the coroutine if fishShadow is null
        }

        float fishSpeed = 2.0f;

        Debug.Log("Starting MoveFishTowardsBite coroutine");


        while (fishShadow != null && Vector3.Distance(fishShadow.transform.position, centerBite) > 0.1f)
        {
            fishShadow.transform.position = Vector3.MoveTowards(
                fishShadow.transform.position,
                centerBite,
                fishSpeed * Time.deltaTime
            );
            yield return null;
        }

        if(fishShadow != null)
        {
            Debug.Log("Fish reached the center without being caught");
            DestroyInstances();
            fishShadow = null;
            Debug.Log("Calling MoveCameraToOriginalPosition from MoveFishTowardsBite()");
            yield return StartCoroutine(MoveCameraToOriginalPosition());
            ResetFishing();
        }
    }

    //Instanciar el anzuelo
    IEnumerator SpawnBite()
    {
        Vector3 bitePos = centerBite;
        anzuelo = Instantiate(anzueloPrefab, bitePos, Quaternion.identity);
        yield return null;
    }


    bool IsFishInRange()
    {
        if (fishShadow == null || anzuelo == null)
        {
            Debug.Log("Fish shadow or bait is null");
            return false;
        }

        float d = Vector3.Distance(fishShadow.transform.position,
            anzuelo.transform.position);

        //Debug.Log($"Distance between fish and bait: {d}");

        return d <= (catchDistance + FishingRodMaterialDistance()) * playerData.fishing_rod_range_multiplier;
    }

    float FishingRodMaterialDistance()
    {
        if(FISHING_ROD_MATERIAL == FISHING_ROD.GOLD)
        {
            return 2;
        }else if(FISHING_ROD_MATERIAL == FISHING_ROD.SILVER)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Finish fishing animation
    void ResetFishing()
    {
        Debug.Log("Reset fishing was called");
        if (fishShadow != null)
        {
            DestroyInstances();
            fishShadow = null;
        }

        isFishInRange = false;

        // Move the camera back to the original position
        StartCoroutine(MoveCameraToOriginalPosition());
        isFishingCoroutineRunning = false;
        gameManagerScript.setFishing(false);

    }

    IEnumerator MoveCameraToOriginalPosition()
    {
        float duration = 1.0f; // Duration of the transition
        float elapsed = 0.0f;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        while (elapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, originalCameraPosition, elapsed / duration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, originalCameraRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;

        UnlockCamera();

        Debug.Log($"Original view transition complete: rotation = {mainCamera.transform.rotation.eulerAngles}");
    }


    void LockCamera()
    {
        if (cameraMovement != null)
        {
            cameraMovement.enabled = false; // Disable the CameraMovement script
        }

        cameraMovement.locked = true;
    }

    void UnlockCamera()
    {
        if (cameraMovement != null)
        {
            cameraMovement.enabled = true; // Disable the CameraMovement script
        }
        cameraMovement.locked = false;
    }

    void DestroyInstances()
    {
        if(fishShadow != null)
        {
            Destroy(fishShadow);
        }

        if(anzuelo != null)
        {
            Destroy(anzuelo);
        }
    }
}

public enum FISHING_ROD{
    NORMAL,
    SILVER,
    GOLD
}