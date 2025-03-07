using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public Canvas settings;

    public Slider volumeSlider;

    public GameObject audioContainer;
    private AudioSource musicSource;

    private PlayerData pd;

    public GameObject player;
    private PlayerMovement PlayerMovement;

    public GameObject Camera;
    private CameraMovement CameraMovement;


    // Start is called before the first frame update
    void Start()
    {
        musicSource = audioContainer.GetComponent<AudioSource>();
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        ApplyVolume(volumeSlider.value);

        pd = GetComponent<GameManager>().GetPlayerData();

        volumeSlider.onValueChanged.AddListener(ApplyVolume);

        PlayerMovement = player.GetComponent<PlayerMovement>();
        CameraMovement = Camera.GetComponent<CameraMovement>();

        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible

        settings.enabled = false;
    }

    private void Update()
    {
        if(pd == null)
        {
            pd = GetComponent<GameManager>().GetPlayerData();
        }
        if (musicSource == null)
        {
            musicSource = audioContainer.GetComponent<AudioSource>();

        }
        if (!pd.isFishing && Input.GetKeyDown(KeyCode.Escape))
        {
            settings.enabled = !settings.enabled;
            PlayerMovement.canMove = !PlayerMovement.canMove;
            CameraMovement.locked = !CameraMovement.locked;

            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }
    }

    void ApplyVolume(float value)
    {
        Debug.Log("Volume applied");
        if (musicSource != null)
        {
            musicSource.volume = value;
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}
