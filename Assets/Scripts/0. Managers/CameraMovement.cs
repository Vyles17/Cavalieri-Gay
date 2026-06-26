using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = targetPlayer.position + offset;
    }

}
