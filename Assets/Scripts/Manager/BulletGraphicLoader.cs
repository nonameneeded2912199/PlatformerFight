using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGraphicLoader : MonoBehaviour
{
    public static BulletGraphicLoader Instance { get; private set; }

    private Dictionary<BulletType, Dictionary<BulletColor, Sprite>> bulletGraphics;

    public Dictionary<BulletType, Dictionary<BulletColor, Sprite>> BulletGraphics { get => bulletGraphics; }
    public bool isBulletGraphicsLoaded { private set; get; } = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        bulletGraphics = new Dictionary<BulletType, Dictionary<BulletColor, Sprite>>();

        StartCoroutine(LoadBulletGraphics());
    }

    private IEnumerator LoadBulletGraphics()
    {
        //DirectoryInfo d;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Bullets");
        if (!isBulletGraphicsLoaded)
        {
            /*Debug.Log("BGMManager is starting to load BGM.");
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
                        d = new DirectoryInfo(Application.dataPath + "/Resources/Bullets");
            #elif UNITY_ANDROID
                            d = new DirectoryInfo(Application.persistentDataPath + "/Resources/Bullets");
            #endif
            FileInfo[] Files = d.GetFiles("*.png");

            foreach (FileInfo file in Files)
            {
                string fileNameWithoutExtension = file.Name.Split('.')[0];
                ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>("Bullets/" + fileNameWithoutExtension);
                while (!resourceRequest.isDone)
                {
                    yield return 0;
                }

                string bulletTypeName = fileNameWithoutExtension.Split('_')[0];
                string bulletColorName = fileNameWithoutExtension.Split('_')[1];

                BulletType bulletType = (BulletType)Enum.Parse(typeof(BulletType), bulletTypeName);
                BulletColor bulletColor = (BulletColor)Enum.Parse(typeof(BulletColor), bulletColorName);

                Sprite sprite = resourceRequest.asset as Sprite;
                if (!bulletGraphics.ContainsKey(bulletType))
                    bulletGraphics[bulletType] = new Dictionary<BulletColor, Sprite>();

                bulletGraphics[bulletType].Add(bulletColor, sprite);
            }

            isBulletGraphicsLoaded = true;
            Debug.Log("Bullet Graphics loaded successfully.");*/

            foreach (Sprite bulletImage in sprites)
            {
                string bulletTypeName = bulletImage.name.Split('_')[0];
                string bulletColorName = bulletImage.name.Split('_')[1];

                BulletType bulletType = (BulletType)Enum.Parse(typeof(BulletType), bulletTypeName);
                BulletColor bulletColor = (BulletColor)Enum.Parse(typeof(BulletColor), bulletColorName);

                if (!bulletGraphics.ContainsKey(bulletType))
                    bulletGraphics[bulletType] = new Dictionary<BulletColor, Sprite>();

                bulletGraphics[bulletType].Add(bulletColor, bulletImage);
                yield return 0;
            }

            isBulletGraphicsLoaded = true;
        }
        else
        {
            yield return null;
        }
    }
    
    public Sprite GetBulletGraphics(BulletType bulletType, BulletColor bulletColor)
    {
        if (bulletGraphics.ContainsKey(bulletType))
        {
            if (bulletGraphics[bulletType].ContainsKey(bulletColor))
            {
                return bulletGraphics[bulletType][bulletColor];
            }    
        }

        return null;
    }    
}

public enum BulletType
{
    Arrow,  
    Ball,
    Ball2,
    Bullet,
    Ice,
    Inverted,
    Kunai,
    Rice,
    Square,
    Star
}

public enum BulletColor
{
    DEEPRED,
    RED,
    DEEPPURPLE,
    PURPLE,
    DEEPBLUE,
    BLUE,
    ROYALBLUE,
    CYAN,
    DEEPGREEN,
    GREEN,
    CHARTREUSE,
    YELLOW,
    GOLDENYELLOW,
    ORANGE,
    DEEPGRAY,
    GRAY,
}

public enum BulletOwner
{
    Player,
    Enemy
}
