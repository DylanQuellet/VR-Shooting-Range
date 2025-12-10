using UnityEngine;
// N�cessaire pour utiliser les classes et �v�nements VR
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections; // N�cessaire si vous utilisez une coroutine pour le d�lai de tir
public class PanPanController : MonoBehaviour
{
    // --- Configuration du tir ---
    public string bulletTag = "Bullet";
    public Transform muzzle; // Le point de sortie de la balle
    public float fireRate = 0.5f; // Cadence de tir (ex: 2 balles par seconde)
    private float nextFireTime = 0f;

    // --- R�f�rences VR ---
    // Composant obligatoire sur le m�me GameObject
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    // R�f�rence au Pooler (si vous n'utilisez pas le Singleton)
    // private ObjectPooler pooler; 

    void Awake()
    {
        // 1. Obtenir la r�f�rence au composant Grab Interactable
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // 2. S'abonner aux �v�nements d'activation (Trigger press�) et de d�sactivation (Trigger rel�ch�)
        // Le += est la syntaxe pour s'abonner � un �v�nement
        grabInteractable.activated.AddListener(StartFire);
        grabInteractable.deactivated.AddListener(StopFire);

        // pooler = ObjectPooler.Instance; // Optionnel si ObjectPooler est un Singleton
    }

    void OnDestroy()
    {
        // 3. IMPORTANT : Toujours se d�sabonner pour �viter les fuites m�moire
        grabInteractable.activated.RemoveListener(StartFire);
        grabInteractable.deactivated.RemoveListener(StopFire);
    }

    /// <summary>
    /// D�clench� par l'�v�nement `activated` du XRGrabInteractable
    /// </summary>
    private void StartFire(ActivateEventArgs arg0)
    {
        // On d�marre le tir continu (utile pour les tirs automatiques/semi-automatiques)
        StartCoroutine(FiringRoutine());
    }

    /// <summary>
    /// D�clench� par l'�v�nement `deactivated` du XRGrabInteractable
    /// </summary>
    private void StopFire(DeactivateEventArgs arg0)
    {
        // On arr�te le tir quand la g�chette est rel�ch�e
        StopAllCoroutines();
    }

    /// <summary>
    /// Coroutine g�rant la cadence de tir (utile pour les tirs semi-auto ou auto)
    /// </summary>
    IEnumerator FiringRoutine()
    {
        while (true) // Boucle de tir tant que la coroutine est active
        {
            if (Time.time > nextFireTime)
            {
                // Mettre � jour le temps du prochain tir possible
                nextFireTime = Time.time + fireRate;

                // --- La fonction de tir r�elle ---
                Fire();
            }
            // Attendre la prochaine frame pour v�rifier � nouveau
            yield return null;
        }
    }

    /// <summary>
    /// La logique de tir qui utilise l'Object Pooling.
    /// </summary>
    private void Fire()
    {
        // 1. R�cup�rer la balle du pool (votre fonction de Task 1)
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(
            bulletTag,
            muzzle.position,
            muzzle.rotation
        );

        // 2. Jouer le son du tir
        // GetComponent<AudioSource>().Play(); // Assurez-vous d'avoir un AudioSource
    }
}
