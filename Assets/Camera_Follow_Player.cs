using UnityEngine;

public class Camera_Follow_Player : MonoBehaviour
{
    [SerializeField]
    public Transform player;

    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -10);
    }
}
