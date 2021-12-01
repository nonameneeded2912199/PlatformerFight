using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float movingSpeed;
    public float alphaSpeed;
    public float destroyTime;
    private TextMeshPro text;
    Color alpha;

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

    public void SetPopup(int damage, DamageType damageType, Vector2 position)
    {
        transform.position = position;
        text.text = System.Convert.ToString(damage);
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
        Invoke("BackToPool", destroyTime);
    }  
    
    private void BackToPool()
    {
        PoolManager.ReleaseObject(gameObject);
    }
}

public enum DamageType
{
    NormalDamage,
    CriticalDamage,
    Heal
}
