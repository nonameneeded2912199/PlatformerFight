using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //collision.gameObject.GetComponent<CharacterController>().Die();
            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            collision.gameObject.transform.position = spawnPoint.position;
            collision.gameObject.GetComponent<CharacterThings.CharacterStats>().CurrentHP = collision.gameObject.GetComponent<CharacterThings.CharacterStats>().MaxHP;
        }    
    }
}
