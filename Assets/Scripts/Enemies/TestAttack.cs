using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : AttackPhase
{
    [SerializeField]
    ulong frame = 0;
    float angle1 = 0f, angle2 = 0f;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (frame % 500 == 120)
        //{
        //    for (float i = 0; i < 10; i ++)
        //    {
        //        Vector3 vector = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(2f, 5f));
        //        GameObject bullet = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        //        BulletCommand command = bullet.AddComponent<BulletCommand>();
        //        bullet.transform.position = vector;
        //        bullet.transform.localScale = new Vector3(2f, 2f, 2f);
        //        void update()
        //        {
        //            angle1 = Bullet.GetAngle(player.transform.position - command.transform.position);
        //            if (command.frame >= 150 && command.frame % 10 == 0)
        //            {
        //                for (float a = (angle1 - (Mathf.PI / 18)); a <= angle1 + (Mathf.PI / 18) + 0.01f; a += (Mathf.PI / 18))
        //                {
        //                    GameObject bullet1 = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        //                    Bullet bulletCom = bullet1.GetComponent<Bullet>();
        //                    bullet1.transform.position = vector;
        //                    bullet1.transform.localScale = new Vector3(2f, 2f, 2f);
        //                    bulletCom.player = player;
        //                    bulletCom.Direction = a;
        //                    bulletCom.Speed = 0.075f;
        //                }
        //            }
        //            if (command.frame == 500)
        //            {
        //                command.gameObject.SetActive(false);
        //            }
        //        }
        //        command.update = update;
        //    }             
        //}
        //if (frame % 500 >= 380 && frame % 12 == 0)
        //{
        //    if (frame % 1000 < 500)
        //    {
        //        angle2 += 0.025f;
        //    }
        //    else
        //    {
        //        angle2 -= 0.025f;
        //    }
        //    for (float i = 0; i < 2 * Mathf.PI; i += (2 * Mathf.PI) / 18)
        //    {
        //        GameObject bullet = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        //        Bullet bulletCom = bullet.GetComponent<Bullet>();
        //        bulletCom.player = player;
        //        bullet.transform.localPosition = new Vector3(0, 0, 0);
        //        bullet.transform.localScale = new Vector3(2f, 2f, 2f);
        //        bulletCom.Direction = (i + angle2);
        //        bulletCom.Speed = 0.025f;
        //    }
        //}
        //frame++;

    }
}
