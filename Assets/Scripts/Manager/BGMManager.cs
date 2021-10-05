using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    private Dictionary<string, AudioClip> audioClips;

    public Dictionary<string, AudioClip> AudioClips { get => audioClips; }

    public bool isBGMLoaded { get; set; } = false;

    public AudioSource bgmSource { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        audioClips = new Dictionary<string, AudioClip>();
        bgmSource.volume = PlayerPrefs.GetFloat("Volume");

        StartCoroutine(LoadBGM());
    }
    
    public void PlayBGM(string bgmName)
    {
        if (audioClips.ContainsKey(bgmName))
        {
            if (bgmSource.clip != audioClips[bgmName])
            {
                bgmSource.clip = audioClips[bgmName];
                bgmSource.Play();
            }    
        }    
    }    

    private IEnumerator LoadBGM()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("BGM");
        //DirectoryInfo d;
        if (!isBGMLoaded)
        {
            /*Debug.Log("BGMManager is starting to load BGM.");
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
                d = new DirectoryInfo(Application.dataPath + "/Resources/BGM");
            #elif UNITY_ANDROID
                d = new DirectoryInfo(Application.persistentDataPath + "/Resources/BGM");
            #endif
            FileInfo[] Files = d.GetFiles("*.mp3");  
            
            foreach (FileInfo file in Files)
            {
                string fileNameWithoutExtension = file.Name.Split('.')[0];
                ResourceRequest resourceRequest = Resources.LoadAsync<AudioClip>("BGM/" + fileNameWithoutExtension);
                //Debug.Log("BGM/" + file.);
                while (!resourceRequest.isDone)
                {
                    yield return 0;
                }

                AudioClip clip = resourceRequest.asset as AudioClip;
                audioClips.Add(clip.name, clip);
            }

            isBGMLoaded = true;
            Debug.Log("BGM loaded successfully.");*/

            foreach (AudioClip clip in audioClips)
            {
                this.audioClips.Add(clip.name, clip);
                yield return 0;
            }

            isBGMLoaded = true;
        }    
        else
        {
            yield return null;
        }            
    }   
}
