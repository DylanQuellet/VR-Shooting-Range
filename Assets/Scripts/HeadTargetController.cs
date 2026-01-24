using UnityEngine;

public class HeadTargetController : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        transform.position = cameraTransform.position;
        // Suivi rotation caméra
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, rotationSpeed * Time.deltaTime);
    }
}
