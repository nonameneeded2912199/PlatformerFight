using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	[Header("Pause menu music")]
	[SerializeField] private AudioCueSO _pauseMusic = default;
	[SerializeField] private BoolEventChannelSO _onPauseOpened = default;

	private AudioCueSO currentMusic = default;

	private void OnEnable()
	{
		_onPauseOpened.OnEventRaised += PlayPauseMusic;
		_onSceneReady.OnEventRaised += PlayMusicFromStart;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= PlayMusicFromStart;
		_onPauseOpened.OnEventRaised -= PlayPauseMusic;
	}

	private void PlayMusicFromStart()
	{
		currentMusic = _thisSceneSO.musicTrack;
		_playMusicOn.RaisePlayEvent(currentMusic, _audioConfig);
	}

	private void PlayMusic()
	{
		_playMusicOn.RaisePlayEvent(currentMusic, _audioConfig, default, false);
	}

	private void PlayPauseMusic(bool open)
	{
		if (open)
		{
			Debug.Log("Paused");
			_playMusicOn.RaisePlayEvent(_pauseMusic, _audioConfig, default, false);
		}
		else
			PlayMusic();
	}
}
