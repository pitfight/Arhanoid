using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public enum OtherManagers
    {
        Nothing,
        Override,
        Obey
    }

    private static SoundManager _instance;
    public static SoundManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Space(5)]
    [Header("Settings")]
    [SerializeField] private bool continueOnNextScene = false;
    [SerializeField] private OtherManagers otherManagers;

    [Space(5)]
    [Header("Main Sound")]
    [SerializeField] private AudioClip[] music;

    private float musicValue = 1f;
    private float sfxValue = 1f;

    private int lastRandomIndex = -1;

    private void Awake()
    {
        // Czy ustapic miejsca innym managerom
        if (otherManagers == OtherManagers.Obey && GameObject.FindGameObjectsWithTag("SoundManager").Length > 1)
        {
            //Debug.Log("Self destroy music (obeyed)");
            Destroy(this.gameObject);
        }
        // Czy niszczyć inne managery
        else if (otherManagers == OtherManagers.Override)
        {
            foreach (GameObject soundManager in GameObject.FindGameObjectsWithTag("SoundManager"))
            {
                //Debug.Log("Seek and destroy otherID=" + soundManager.GetInstanceID() + " thisID=" + this.gameObject.GetInstanceID());
                if (soundManager.GetInstanceID() != this.gameObject.GetInstanceID())
                {
                    Destroy(soundManager);
                }
            }
        }

        // Przechodzi do następnych scen
        if (continueOnNextScene) DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayMusic();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void PlayMusic()
    {
        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, music.Length);

        } while (randomIndex == lastRandomIndex);

        AudioClip audioClip = music[randomIndex];
        lastRandomIndex = randomIndex;

        musicSource.clip = audioClip;

        // Wiele utworów
        if (music.Length > 1)
        {
            musicSource.loop = false;
            Invoke("PlayMusic", audioClip.length);
        }
        // Jeden utwór
        else
        {
            musicSource.loop = true;
        }

        // Odtwarzaj
        musicSource.Play();
    }

    /**
     * Metody dla wgrywania sound.
     */

    public void PlayOnSFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayOnMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    /**
     * Przełonczyć muzykę.
     */

    public void NextSoundTrack()
    {
        try
        {
            CancelInvoke();
            AudioClip audioClip = music[lastRandomIndex + 1];
            lastRandomIndex++;
            musicSource.clip = audioClip;
            Invoke("PlayMusic", audioClip.length);
            musicSource.Play();
        }
        catch
        {
            CancelInvoke();
            AudioClip audioClip = music[0];
            lastRandomIndex = 0;
            musicSource.clip = audioClip;
            Invoke("PlayMusic", audioClip.length);
            musicSource.Play();
        }
    }

    /**
     * Metody dla zmiany dynamicznie nagłośnienie z pomocą Slidera.
     */

    public void ChangeVolumeMusic(float volume)
    {
        musicValue = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void ChangeVolumeSFX(float volume)
    {
        sfxValue = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void ToggleMute(bool toggle)
    {
        if (!toggle) audioMixer.SetFloat("Master", -80f);
        else audioMixer.SetFloat("Master", 0);
    }

    public bool ToggleState() => audioMixer.SetFloat("Master", 0f) ? false : true;

    public float GetVolumeMusic() => musicValue;
    public float GetVolumeSFX() => sfxValue;
}
