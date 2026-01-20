using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyboardMovement : MonoBehaviour
{
    [Header("References")]
    public Transform xrCamera;      // Main Camera
    public Transform character;     // FBX du personnage (root)

    [Header("Movement")]
    public float moveSpeed = 3.5f;          // Vitesse max (run)
    public float walkMultiplier = 0.5f;     // Marche = 50%
    public float rotationDamping = 10f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        float forward = 0f;
        float right = 0f;

        // ZQSD (AZERTY)
        if (Keyboard.current.zKey.isPressed) forward += 1f;
        if (Keyboard.current.sKey.isPressed) forward -= 1f;
        if (Keyboard.current.dKey.isPressed) right += 1f;
        if (Keyboard.current.qKey.isPressed) right -= 1f;

        Vector3 input = new Vector3(right, 0f, forward);
        input = Vector3.ClampMagnitude(input, 1f);

        // Shift = courir
        bool isRunning =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        float speedMultiplier = isRunning ? 1f : walkMultiplier;

        // Direction relative caméra (yaw)
        Vector3 camForward = xrCamera.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = xrCamera.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * input.z + camRight * input.x;

        // Déplacement
        transform.position += moveDir * moveSpeed * speedMultiplier * Time.deltaTime;

        // Rotation du personnage vers la vue
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            character.rotation = Quaternion.Slerp(
                character.rotation,
                targetRot,
                rotationDamping * Time.deltaTime
            );
        }
    }
}
