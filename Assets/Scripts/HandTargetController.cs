using UnityEngine;

public class HandTargetController : MonoBehaviour
{
    public Transform reference; // caméra ou weapon socket
    public Vector3 offset;      // offset local
    public float damping = 5f;  // anti-jitter

    void Update()
    {
        // Position avec offset + décalage Y
        Vector3 targetPos = reference.position + reference.TransformDirection(offset);
        targetPos.y -= 0.2f; // décalage vertical
        targetPos.z += 0.1f; // décalage avant
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * damping);

        // Rotation inversée sur Z
        Quaternion targetRot = reference.rotation * Quaternion.Euler(90f, 0, 180f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * damping);
    }
}
