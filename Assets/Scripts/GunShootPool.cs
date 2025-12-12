
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GunShootPool : MonoBehaviour
{
    //[SerializeField] private GameObject bulletPrefab; // Prefab to spawn
    [SerializeField] private Transform spawnPoint; // Point where the bullet will spawn
    //[SerializeField] private ParticleSystem MuzzleFlash;

    private GameObject muzzleInstance;

    public void Shoot()
    {
        if (spawnPoint != null)
        {
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", spawnPoint.position, spawnPoint.rotation);
            bullet.tag = "Bullet"; // Add the 'Bullet' tag to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * 20f; // Adjust speed as needed
            }

            /*
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
            */
            if (FXLoader.MuzzlePrefab != null)
            {
                GameObject fx = Instantiate(FXLoader.MuzzlePrefab, spawnPoint.position, spawnPoint.rotation);
            }

        }

        else
        {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned.");
        }
    }
}
