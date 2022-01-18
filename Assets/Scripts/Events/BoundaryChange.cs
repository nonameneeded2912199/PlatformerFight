using Cinemachine;
using UnityEngine;

public class BoundaryChange : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D cameraBoundary;

    [SerializeField]
    private CinemachineConfiner cameraConfiner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && gameObject.tag != "MainCamera")
        {
            ChangeBoundary();
        }
    }

    public void ChangeBoundary()
    {
        cameraConfiner.m_BoundingShape2D = cameraBoundary;
    }
}
