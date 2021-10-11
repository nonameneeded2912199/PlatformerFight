using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private string checkPointName;

    public string CheckPointName { get => checkPointName; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (DataOnCheckPoint.CheckPointName != checkPointName)
            {
                DataOnCheckPoint.CheckPointName = checkPointName;

                DataOnCheckPoint.sceneName = gameObject.scene.name;

                SaveManager.CheckPointName = checkPointName;

                SaveManager.SceneName = gameObject.scene.name;

                SaveManager.SaveGame();

                Debug.Log("Checked Point: " + transform.position);
            }
            //CharacterController.Instance.Regenerate();
        }
    }
}
