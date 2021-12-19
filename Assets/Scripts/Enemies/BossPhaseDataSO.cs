using PlatformerFight.CharacterThings;
using UnityEngine;

public abstract class BossPhaseDataSO : ScriptableObject
{
    public string enemyName = "Unnamed";

    public CharacterData_SO phaseStats;

    public int scoreYield;

    public BossPhaseType phaseType;

    public string phaseName = "Untitled";

    public float phaseBonusTime;
}

public enum BossPhaseType
{
    NormalAttack,
    SpellCardAttack
}
