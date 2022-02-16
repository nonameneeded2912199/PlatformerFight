using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] 
	private InputReader _inputReader = default;

	[SerializeField] 
	private GameStateSO _gameStateSO = default;

	[SerializeField]
	private PlayableDatabaseSO _playableDatabaseSO = default;

	[SerializeField] 
	private TransformAnchor _playerTransformAnchor = default;

	[SerializeField] 
	private TransformEventChannelSO _playerSpawnedChannel = default;
	//[SerializeField] private PathStorageSO _pathTaken = default;

	[Header("Scene Ready Event")]
	[SerializeField] 
	private VoidEventChannelSO _onSceneReady = default; //Raised by SceneLoader when the scene is set to active

	[SerializeField]
	private Checkpoint[] _possibleSpawnLocation;

	[SerializeField]
	private Transform defaultSpawnPoint;

	private void Awake()
	{
		//_spawnLocations = GameObject.FindObjectsOfType<LocationEntrance>();
		//_defaultSpawnPoint = transform.GetChild(0);
	}

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += SpawnPlayer;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= SpawnPlayer;

		_playerTransformAnchor.Unset();
	}

	private Checkpoint GetSpawnLocation()
	{
		if (_possibleSpawnLocation.Length <= 0 || !PlayerPrefs.HasKey("CurrentCheckpoint"))
			return null;


		int possibleSpawnPointIndex = Array.FindIndex(_possibleSpawnLocation, 
			element => element.CheckpointName == PlayerPrefs.GetString("CurrentCheckpoint"));
		if (possibleSpawnPointIndex == -1)
        {
			return null;
        }
		else
        {
			Checkpoint checkpointSpawn = _possibleSpawnLocation[possibleSpawnPointIndex];
			return checkpointSpawn;
        }
	}

	private void SpawnPlayer()
	{
		Checkpoint checkpoint = GetSpawnLocation();
		Transform spawnLocation;

		if (checkpoint != null)
			spawnLocation = checkpoint.transform;
		else
			spawnLocation = defaultSpawnPoint;

		GameObject playerPrefab = _playableDatabaseSO.GetPlayableInfo(_gameStateSO.ChosenPlayerID).playerGameObject;
		GameObject playerClone = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);

		//_playerSpawnedChannel.RaiseEvent(playerClone.transform);
		if (checkpoint == null)
			_playerTransformAnchor.Provide(playerClone.transform); //the CameraSystem will pick this up to frame the player
		else
			checkpoint.boundaryChange.StartCoroutine(checkpoint.boundaryChange.ChangeVCam(playerClone.transform, 0.5f));

		//TODO: Probably move this to the GameManager once it's up and running
		_inputReader.EnableGameplayInput();
	}
}
