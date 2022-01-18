using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private string checkpointName;

    [SerializeField]
    private bool triggered = false;

    public string CheckpointName => checkpointName;

    public BoundaryChange boundaryChange;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(checkpointName))
        {
            if (PlayerPrefs.GetInt(checkpointName) == 1)
                triggered = true;
            else
                triggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !triggered)
        {
            //CharacterController.Instance.Regenerate();
            //if (_gameStateSO.LastCheckpoint != checkpointName)
            if (PlayerPrefs.GetString("CurrentCheckpoint") != checkpointName)
            {
                //_gameStateSO.SetCheckpoint(checkpointName);
                PlayerPrefs.SetString("CurrentCheckpoint", checkpointName);
                PlayerPrefs.SetInt(CheckpointName, 1);
            }
            triggered = true;
        }
    }
}
