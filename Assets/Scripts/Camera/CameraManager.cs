using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;

    [SerializeField] [Range(.5f, 3f)] private float _speedMultiplier = 1f; 
    //[SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    [SerializeField] private TransformAnchor _playerTransformAnchor = default;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void OnEnable()
    {
        _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;
    }

    private void OnDisable()
    {
        _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;
    }

    private void Start()
    {
        if (_playerTransformAnchor.isSet)
            SetupPlayerVirtualCamera();
    }


    public void SetupPlayerVirtualCamera()
    {
        Transform target = _playerTransformAnchor.Value;

        cinemachineVirtualCamera.Follow = target;
        cinemachineVirtualCamera.LookAt = target;
        //cinemachineVirtualCamera.OnTargetObjectWarped(target, target.position);
    }
}
