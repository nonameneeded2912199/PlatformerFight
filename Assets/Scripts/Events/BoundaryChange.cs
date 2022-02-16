using Cinemachine;
using UnityEngine;
using System.Collections;

public class BoundaryChange : MonoBehaviour
{
    [SerializeField]
    private GameObject newVCam;

    [SerializeField]
    private float defaultBlendingTime = 2f;

    private CinemachineBrain cinemachineBrain;
    private ICinemachineCamera currentVCam;

    [SerializeField]
    private TransformAnchor _playerTransformAnchor = default;

    private void Awake()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.tag != "MainCamera")
        {
            StartCoroutine(ChangeVCam(collision.transform, defaultBlendingTime));
        }
    }

    public IEnumerator ChangeVCam(Transform player, float blendingTime)
    {
        currentVCam = cinemachineBrain.ActiveVirtualCamera;
        ICinemachineCamera newVCam = this.newVCam.GetComponent<CinemachineVirtualCamera>();

        if (currentVCam == newVCam)
            yield break;

        bool isDone = false;

        while (!isDone)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = blendingTime;

            currentVCam.Priority = 0;
            currentVCam.VirtualCameraGameObject.SetActive(false);

            newVCam.Priority = 10;
            newVCam.VirtualCameraGameObject.SetActive(true);
            isDone = true;

            yield return new WaitForEndOfFrame();
        }

        _playerTransformAnchor.Provide(player);
    }
}
