using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    private Dictionary<string, AudioClip> audioClips;

    public Dictionary<string, AudioClip> AudioClips { get => audioClips; }

    public bool isSELoaded { get; set; } = false;

    public AudioSource seSource { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        seSource = GetComponent<AudioSource>();
        audioClips = new Dictionary<string, AudioClip>();

        StartCoroutine(LoadSE());
    }

    public void PlaySE(string seName)
    {
        if (audioClips.ContainsKey(seName))
        {
            if (seSource.clip != audioClips[seName])
            {
                seSource.clip = audioClips[seName];
                seSource.Play();
            }
        }
    }

    private IEnumerator LoadSE()
    {
        DirectoryInfo d;
        if (!isSELoaded)
        {
            Debug.Log("SEManager is starting to load SE.");
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            d = new DirectoryInfo(Application.dataPath + "/Resources/SE");
#elif UNITY_ANDROID
            d = new DirectoryInfo(Application.persistentDataPath + "/Resources/SE");
#endif
            FileInfo[] Files = d.GetFiles("*.mp3");

            foreach (FileInfo file in Files)
            {
                string fileNameWithoutExtension = file.Name.Split('.')[0];
                ResourceRequest resourceRequest = Resources.LoadAsync<AudioClip>("SE/" + fileNameWithoutExtension);
                //Debug.Log("BGM/" + file.);
                while (!resourceRequest.isDone)
                {
                    yield return 0;
                }

                AudioClip clip = resourceRequest.asset as AudioClip;
                audioClips.Add(clip.name, clip);
            }

            isSELoaded = true;
            Debug.Log("SE loaded successfully.");
        }
        else
        {
            yield return null;
        }
    }
}
