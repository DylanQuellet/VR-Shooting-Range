using UnityEngine;

public class PlayerLocomotionAnimator : MonoBehaviour
{
    [Header("References")]
    public Transform playerRoot;     // Le GameObject qui se déplace (PlayerRoot)
    public Animator animator;

    [Header("Speed settings")]
    public float maxSpeed = 3.5f;    // vitesse max en run
    public float damping = 10f;      // lissage animator

    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        lastPosition = playerRoot.position;
    }

    void Update()
    {
        // Déplacement horizontal uniquement
        Vector3 delta = playerRoot.position - lastPosition;
        delta.y = 0f;

        float speed = delta.magnitude / Time.deltaTime; // m/s

        // Normalisation 0 → 1 pour le Blend Tree
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);

        // Lissage pour éviter le jitter
        currentSpeed = Mathf.Lerp(currentSpeed, normalizedSpeed, damping * Time.deltaTime);

        animator.SetFloat("Speed", currentSpeed);

        lastPosition = playerRoot.position;
    }
}
