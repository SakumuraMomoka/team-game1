using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;

    private void Update()
    {
        this.transform.position
            = new Vector3(playerTrans.position.x, playerTrans.position.y, this.transform.position.z);
    }
}
