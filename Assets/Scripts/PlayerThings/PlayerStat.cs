using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public float hp;
    public float maxHP;

    public Image imageHP;

    void Start()
    {
        hp = maxHP;
    }

    void Update()
    {
        
    }

    public void GetHurt(float damage)
    {
    }

}
