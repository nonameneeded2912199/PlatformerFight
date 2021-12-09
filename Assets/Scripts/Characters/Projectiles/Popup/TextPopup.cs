using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    public float movingSpeed;
    public float alphaSpeed;
    private TextMeshPro text;
    Color alpha;

    [SerializeField]
    private TextPopupEventChannelSO popupEventChannel;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (enabled)
        {
            transform.Translate(new Vector3(0, movingSpeed * Time.deltaTime, 0));
            alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
            text.color = alpha;
        }
    }

    public void SetPopup(string content, Vector2 position, float destroyTime = 3f, DamageType damageType = DamageType.NormalDamage)
    {
        transform.position = position;
        text.text = content;
        SetDamageType(damageType);
        
        Invoke("BackToPool", destroyTime);
    }
    
    public void SetDamageType(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.NormalDamage:
                alpha = Color.white;
                break;
            case DamageType.CriticalDamage:
                alpha = Color.red;
                break;
            case DamageType.Heal:
                alpha = Color.green;
                break;
        }
    }
    
    private void BackToPool()
    {
        popupEventChannel.RaiseReturnTextPopupEvent(this);
    }
}

public enum DamageType
{
    NormalDamage,
    CriticalDamage,
    Heal
}
