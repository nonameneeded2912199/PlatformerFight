using PlatformerFight.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayerButtonControl : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField]
    private SkillEventChannelSO _onSkill1Set = default;

    [SerializeField]
    private SkillEventChannelSO _onSkill2Set = default;

    [SerializeField]
    private SkillEventChannelSO _onSkill3Set = default;

    [SerializeField]
    private SkillEventChannelSO _onSkill4Set = default;

    private Skill skill1;
    private Skill skill2;
    private Skill skill3;
    private Skill skill4;

    [Space]
    [Header("Skill Icons")]
    [SerializeField]
    private Image imageSkill1;

    [SerializeField]
    private Image imageSkill2;

    [SerializeField]
    private Image imageSkill3;

    [SerializeField]
    private Image imageSkill4;

    [SerializeField]
    private Image cooldownSkill1;

    [SerializeField]
    private Image cooldownSkill2;

    [SerializeField]
    private Image cooldownSkill3;

    [SerializeField]
    private Image cooldownSkill4;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        _onSkill1Set.OnEventRaised += SetSkill1;
        _onSkill2Set.OnEventRaised += SetSkill2;
        _onSkill3Set.OnEventRaised += SetSkill3;
        _onSkill4Set.OnEventRaised += SetSkill4;

    }

    private void OnDisable()
    {
        _onSkill1Set.OnEventRaised -= SetSkill1;
        _onSkill2Set.OnEventRaised -= SetSkill2;
        _onSkill3Set.OnEventRaised -= SetSkill3;
        _onSkill4Set.OnEventRaised -= SetSkill4;
    }

    private void Update()
    {
        if (skill1 != null)
        {
            cooldownSkill1.fillAmount = skill1.CooldownRate;
        }

        if (skill2 != null)
        {
            cooldownSkill2.fillAmount = skill2.CooldownRate;
        }

        if (skill3 != null)
        {
            cooldownSkill3.fillAmount = skill3.CooldownRate;
        }

        if (skill4 != null)
        {
            cooldownSkill4.fillAmount = skill4.CooldownRate;
        }
    }

    private void SetSkill1(Skill skill)
    {
        skill1 = skill;
        imageSkill1.sprite = skill.SkillIcon;
    }

    private void SetSkill2(Skill skill)
    {
        skill2 = skill;
        imageSkill2.sprite = skill.SkillIcon;
    }

    private void SetSkill3(Skill skill)
    {
        skill3 = skill;
        imageSkill3.sprite = skill.SkillIcon;
    }

    private void SetSkill4(Skill skill)
    {
        skill4 = skill;
        imageSkill4.sprite = skill.SkillIcon;
    }
}
