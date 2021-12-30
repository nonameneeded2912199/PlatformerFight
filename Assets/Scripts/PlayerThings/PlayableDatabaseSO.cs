using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayableDatabase", menuName = "Playable Character/Database")]
public class PlayableDatabaseSO : ScriptableObject
{
    [SerializeField]
    private PlayableCharacterInfo[] database;

    public int Amount => database.Length;

    public PlayableCharacterInfo GetPlayableInfo(int id)
    {
        foreach (PlayableCharacterInfo info in database)
        {
            if (info.CharacterID == id)
                return info;
        }

        return null;
    }
}
