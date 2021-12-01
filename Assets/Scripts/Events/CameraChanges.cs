using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanges : MonoBehaviour
{
    [SerializeField]
    private GameObject newVCamObject;

    [SerializeField]
    private float blendingTime = 2f;

    private CinemachineBrain cinemachineBrain;
    private ICinemachineCamera currentVCam;

    private void Awake()
    {

    }

    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void ChangeVCam()
    {
        currentVCam = cinemachineBrain.ActiveVirtualCamera;
        ICinemachineCamera newVCam = newVCamObject.GetComponent<CinemachineVirtualCamera>();

        if (currentVCam == newVCam)
            return;

        cinemachineBrain.m_DefaultBlend.m_Time = blendingTime;

        currentVCam.Priority = 0;
        currentVCam.VirtualCameraGameObject.SetActive(false);

        newVCam.Priority = 10;
        newVCam.VirtualCameraGameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.tag != "MainCamera")
        {
            ChangeVCam();
        }
    }

    public void RefollowTargetOnNewVCam(Transform target)
    {
        if (cinemachineBrain == null)
            cinemachineBrain = GetComponent<CinemachineBrain>();

        if (currentVCam != null)
            currentVCam.Follow = null;

        currentVCam = cinemachineBrain.ActiveVirtualCamera;
        currentVCam.Follow = target;
    }
}
