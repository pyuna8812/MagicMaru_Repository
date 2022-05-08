using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility : MonoBehaviour
{
    public float moveSpeed;
    public const float LEFT_MAX = -30.2f;
    public const float RiGHT_MAX = 30.2f;
    public bool isLeft { get; set; }
    public bool isRight { get; set; }

    private void LateUpdate()
    {
        if (isLeft)
        {
            if(transform.position.x <= LEFT_MAX)
            {
                return;
            }
            MoveCamera(Vector3.left);
        }
        else if (isRight)
        {
            if (transform.position.x >= RiGHT_MAX)
            {
                return;
            }
            MoveCamera(Vector3.right);
        }
    }
    private void MoveCamera(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
