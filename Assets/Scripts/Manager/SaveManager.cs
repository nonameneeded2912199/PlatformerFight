using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

//public static class SaveManager
//{
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//    private static readonly string SAVE_PATH = Application.dataPath + "/save.dat";
//#elif UNITY_ANDROID
//    private static readonly string SAVE_PATH = Application.persistentDataPath. + "/save.dat";
//#endif

//    public static bool IsSaveDataAlreadyLoaded { get; private set; }

//    //public static Vector3 CheckPoint { get; set; }

//    public static string CheckPointName { get; set; }

//    public static string SceneName { get; set; }


//    public static void SaveGame()
//    {
//        BinaryFormatter binartFormatter = new BinaryFormatter();
//        FileStream fileStream = new FileStream(SAVE_PATH, FileMode.Create);

//        try
//        {
//            IDataManager savedData = new PlayerData();
//            savedData.SetDataToSave();

//            binartFormatter.Serialize(fileStream, savedData);
//        }
//        catch (SerializationException e)
//        {
//            Debug.Log(e.Message);
//        }
//        finally
//        {
//            fileStream.Close();
//            IsSaveDataAlreadyLoaded = false;
//        }
//    }

//    public static PlayerData LoadGame()
//    {
//        if (File.Exists(SAVE_PATH))
//        {
//            FileStream fileStream = new FileStream(SAVE_PATH, FileMode.Open);

//            try
//            {
//                BinaryFormatter binaryFormatter = new BinaryFormatter();

//                PlayerData savedData = binaryFormatter.Deserialize(fileStream) as PlayerData;

//                return savedData;
//            }
//            catch (SerializationException e)
//            {
//                Debug.Log(e.Message);
//            }
//            finally
//            {
//                fileStream.Close();
//                IsSaveDataAlreadyLoaded = true;
//            }
//        }

//        // save file not found
//        return null;
//    }
//}
