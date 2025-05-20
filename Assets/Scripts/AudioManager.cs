using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Tooltip("A scriptable object that contains the audio and looping information of a single music track.")]
    public BGMAsset BGM;

    [Tooltip("Audio source for BGM")]
    public AudioSource BGM_audioSource;

    [Tooltip("2d sfx audio source. Used for UI SFX or major telegraph sounds.")]
    public AudioSource SFX2d_audioSource;

    [Tooltip("Prefab that appears to play a sound in 3d space then dies.")]
    [SerializeField] private AudioSource temp3dSFXPrefab;

    public bool musicEnabled = true;
    private Scene sceneLastUpdate;
    private BGMAsset bgmLastUpdate;
    [SerializeField] float sfxRepeatCooldownMax;
    [SerializeField] float sfxRepeatCooldownSpeed;
    public float sfxRepeatCooldownNow;

    //specific sounds
    //public AudioClip sfxRoomClear;

    //Get mixer
    [SerializeField] AudioMixer Mixer;

    //volume parameters that player settings modify
    const string MIXER_MASTER = "PlayerMasterVol";
    const string MIXER_BGM = "PlayerBGMVol";
    const string MIXER_SFX = "PlayerSFXVol";
    const string MIXER_AMB = "PlayerAMBVol";

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            //pass this instance's info to the main music manager, then destroy self
            AudioManager.Instance.BGM = BGM;
            AudioManager.Instance.musicEnabled = musicEnabled;
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log(Instance);
        }
    }

    void Start()
    {
        NewSceneActOnInfo();
    }

    // Update is called once per frame
    // Do not use FixedUpdate for this. The music will not loop when timescale is 0 in FixedUpdate
    void Update()
    {
        if (sceneLastUpdate != SceneManager.GetActiveScene()) NewSceneActOnInfo();
        if (musicEnabled && BGM != null)
        {
            if ((BGM_audioSource.time >= BGM.loopEndPoint) && (BGM.customLoop))
            {
                Debug.Log("Time to loop!");
                //if custom loop is enabled, reset the life source's position (and death source, if there is one)
                BGM_audioSource.time -= BGM.loopLength;
            }
        }

        sceneLastUpdate = SceneManager.GetActiveScene();
        bgmLastUpdate = BGM;
        if (sfxRepeatCooldownNow > 0.0f) sfxRepeatCooldownNow -= sfxRepeatCooldownSpeed;
    }

    void PlayBGMStatic()
    {
        BGM_audioSource.clip = BGM.audioClip;
        BGM_audioSource.loop = BGM.canLoop;
        BGM_audioSource.volume = 1.0f;
        BGM_audioSource.time = 0.0f;
        BGM_audioSource.Play();
    }

    void NewSceneActOnInfo()
    {
        LoadVolumeSettings();
        if (BGM == null)
        {
            //music asset
            BGM_audioSource.Stop();
            Debug.Log("BGM = NULL");
        }
        else
        {
            Debug.Log("BGM != NULL");
            if (BGM != bgmLastUpdate)
            {
                //BGM_audioSource.clip is empty by default.
                //if the music from the last room doesn't match the music for this room... play the new music!
                PlayBGMStatic();
                Debug.Log("BGM: " + BGM);
                Debug.Log("bgmLastUpdate: " + bgmLastUpdate);
            }
        }
    }

    void LoadVolumeSettings()
    {
        //load volume settings
       /* SaveDataScript.Load();

        Mixer.SetFloat(MIXER_MASTER, Mathf.Log10(SaveDataScript.MySaveData.masterVol) * 20);
        Mixer.SetFloat(MIXER_BGM, Mathf.Log10(SaveDataScript.MySaveData.bgmVol) * 20);
        Mixer.SetFloat(MIXER_SFX, Mathf.Log10(SaveDataScript.MySaveData.sfxVol) * 20);
        Mixer.SetFloat(MIXER_AMB, Mathf.Log10(SaveDataScript.MySaveData.ambVol) * 20);*/
    }

    //sound effects
    public void PlaySFX2D(AudioClip audioClip = null, bool randomPitch = false, AudioClip[] audioClips = null, float customPitch = 1.0f)
    {
        if (randomPitch) SFX2d_audioSource.pitch = Random.Range(0.9f, 1.1f);
        else SFX2d_audioSource.pitch = customPitch;
        if (audioClip != null) SFX2d_audioSource.PlayOneShot(audioClip);
        if (audioClips != null) SFX2d_audioSource.PlayOneShot(audioClips[(int)Random.Range(0, audioClips.Length - 1)]);
    }

    public void PlaySFX3D(AudioClip audioClip = null, Transform soundTransform = null, float volume = 1.0f, AudioClip[] audioClips = null, bool randomPitch = false)
    {
        if ((soundTransform != null) && (audioClip != null || audioClips != null))
        {
            if (audioClip == null && audioClips != null) { audioClip = audioClips[Random.Range(0, audioClips.Length)]; }
            //instantiate 3d sound obj prefab
            AudioSource audioSource = Instantiate(temp3dSFXPrefab, soundTransform.position, Quaternion.identity);
            //assign clip
            audioSource.clip = audioClip;
            //assign volume
            audioSource.volume = volume;
            //assign length
            float length = audioClip.length;
            //randomise pitch
            if (randomPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
            //play audio
            audioSource.Play();
            //destroy when done playing
            Destroy(audioSource.gameObject, length);
        }
    }

    /*public void PlaySliderSFX(float sliderValue)
    {
        if (sfxRepeatCooldownNow <= 0.0f)
        {
            //sliders are between 0 and 1, so proportionally...
            float pitchMin = 0.5f;
            float pitchMax = 1.0f;
            float pitchFinal = Mathf.Lerp(pitchMin, pitchMax, sliderValue);
            PlaySFX2D(sliderChangeSFX, false, null, pitchFinal);
            sfxRepeatCooldownNow = sfxRepeatCooldownMax;
        }
    }*/
}
