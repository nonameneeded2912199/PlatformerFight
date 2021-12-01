using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool isShieldCircular = true;

    [ConditionalField("isShieldCircular", true), Tooltip("Circular hitbox radius")]
    public float shieldRadius;

    [ConditionalField("isShieldCircular", false), Tooltip("Rectangular hitbox horitzontal size")]
    public float horizontalSize;
    [ConditionalField("isShieldCircular", false), Tooltip("Rectangular hitbox vertical size")]
    public float verticalSize;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isShieldCircular)
            Gizmos.DrawWireSphere(transform.position, shieldRadius);
        else
        {
            Vector2 topLeft = new Vector2(transform.position.x - horizontalSize / 2, transform.position.y + verticalSize / 2);

            Vector2 topRight = new Vector2(transform.position.x + horizontalSize / 2, transform.position.y + verticalSize / 2);

            Vector2 bottomLeft = new Vector2(transform.position.x - horizontalSize / 2, transform.position.y - verticalSize / 2);

            Vector2 bottomRight = new Vector2(transform.position.x + horizontalSize / 2, transform.position.y - verticalSize / 2);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topLeft, bottomLeft);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }
}
