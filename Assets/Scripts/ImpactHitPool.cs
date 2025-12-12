/*
using System.Collections;
using UnityEngine;

public class ImpactHitPool : MonoBehaviour
{
    [SerializeField] private ParticleSystem Explosion;
    public GameObject Cylindre;
    public GameObject CibleEntier;

    public void OnChildTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (Explosion != null)
            {
                Explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                Explosion.Play();
            }
            Cylindre.SetActive(false);
            StartCoroutine(DisableAfterFX());
        }
    }
    IEnumerator DisableAfterFX()
    {
        yield return new WaitForSeconds(3f);
        Cylindre.SetActive(true);
        //Explosion.Stop();
        CibleEntier.SetActive(false);
    }

}
*/

using System.Collections;
using UnityEngine;

public class ImpactHitPool : MonoBehaviour
{
    public GameObject Cylindre;
    public GameObject CibleEntier;

    private GameObject explosionInstance;

    private bool hasBeenHit = false;

    public void OnChildTriggerEnter(Collider other)
    {
        if (hasBeenHit) return; // Empêche les doubles attaques

        if (other.CompareTag("Bullet"))
        {
            hasBeenHit = true;
            // Lorsque la cible est touchée
            GameplayManager.Instance.AddScore(10);

            // Exemple : diminuer le nombre de cibles actives
            GameplayManager.Instance.RegisterTargetDespawn();

            other.BroadcastMessage("DisableBullet", SendMessageOptions.DontRequireReceiver);

            PlayExplosion();

            Cylindre.SetActive(false);
            StartCoroutine(DisableAfterFX());
        }
    }

    private void PlayExplosion()
    {
        if (FXLoader.ExplosionPrefab == null)
        {
            Debug.LogError("ExplosionPrefab non chargé via FXLoader !");
            return;
        }

        // 1. Instanciation une seule fois
        if (explosionInstance == null)
        {
            explosionInstance = Instantiate(
                FXLoader.ExplosionPrefab,
                transform.position,
                transform.rotation,
                transform
            );
        }

        // Position décalée en Y
        Vector3 spawnPos = transform.position + new Vector3(0f, 1f, 0f);
        // Rotation vers le haut
        Quaternion spawnRot = Quaternion.LookRotation(Vector3.up);

        // 2. Mise à jour transform
        explosionInstance.transform.SetPositionAndRotation(spawnPos, spawnRot);

        // 3. Récupération/lecture du ParticleSystem
        ParticleSystem ps = explosionInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
        else
        {
            Debug.LogError("ParticleSystem non trouvé sur ExplosionPrefab !");
        }
    }

    IEnumerator DisableAfterFX()
    {
        yield return new WaitForSeconds(3f);

        Cylindre.SetActive(true);
        hasBeenHit = false; // Réinitialise l'état pour permettre de futurs hits
        CibleEntier.SetActive(false);
    }
}
