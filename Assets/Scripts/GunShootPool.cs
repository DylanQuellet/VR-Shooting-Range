
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GunShootPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab to spawn
    [SerializeField] private Transform spawnPoint; // Point where the bullet will spawn
    //[SerializeField] private ParticleSystem MuzzleFlash;

    private GameObject muzzleInstance;

    public void Shoot()
    {
        if (bulletPrefab != null && spawnPoint != null)
        {
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", spawnPoint.position, spawnPoint.rotation);
            bullet.tag = "Bullet"; // Add the 'Bullet' tag to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * 20f; // Adjust speed as needed
            }
            //Destroy(bullet, 10f); // Destroy the bullet after 10 seconds
            

            //if (MuzzleFlash != null)
            //{
            //    MuzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //    MuzzleFlash.Play();
            //}
            
            
            
            if (FXLoader.MuzzlePrefab != null) // Maintenant, utilise la variable statique mise à jour
            {
                // 1. Instanciation si c'est la première fois ou réutilisation
                if (muzzleInstance == null)
                    muzzleInstance = Instantiate(FXLoader.MuzzlePrefab, spawnPoint.position, spawnPoint.rotation);

                // 2. Mise à jour de la position et rotation
                muzzleInstance.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

                // 3. Obtention et lecture du système de particules (utilisez Get/ComponentInChildren si nécessaire)
                var ps = muzzleInstance.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }
                else
                {
                    Debug.LogError("ParticleSystem non trouvé sur le prefab chargé !");
                }
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned.");
        }
    }
}

/*
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GunShootPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint; // POINT DE DÉPART DE L'OBJET

    private GameObject muzzleInstance;
    private GameObject testInstance; // Nouvelle instance pour le test

    public void Shoot()
    {
        // ------------------------------------------------
        // 1. LOGIQUE DE TEST (PRIORITAIRE)
        // ------------------------------------------------
        if (FXLoader.TestPrefab != null)
        {
            // Instancier ou réutiliser l'objet de test
            if (testInstance == null)
            {
                testInstance = Instantiate(FXLoader.TestPrefab, spawnPoint.position, spawnPoint.rotation);
            }

            // Placer l'objet de test au bout du canon
            testInstance.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            // Optionnel : ajouter une petite force pour qu'il s'envole légèrement
            Rigidbody rb = testInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * 5f; // Petite force, juste pour voir
            }

            Debug.Log("TEST : Cube/Capsule affiché au spawnPoint.");
            return; // Arrêter la fonction pour ne pas tirer de balle
        }

        // ------------------------------------------------
        // 2. LOGIQUE DE PRODUCTION (Originale)
        // ------------------------------------------------
        if (bulletPrefab != null && spawnPoint != null)
        {
            // ... (Votre logique de spawn de la balle ObjectPooler)
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", spawnPoint.position, spawnPoint.rotation);
            bullet.tag = "Bullet";
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * 20f;
            }

            // Logique du Muzzle Flash
            if (FXLoader.MuzzlePrefab != null)
            {
                if (muzzleInstance == null)
                    muzzleInstance = Instantiate(FXLoader.MuzzlePrefab, spawnPoint.position, spawnPoint.rotation);

                muzzleInstance.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

                var ps = muzzleInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned.");
        }
    }
}
*/