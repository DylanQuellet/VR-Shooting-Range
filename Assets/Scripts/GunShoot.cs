using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab to spawn
    [SerializeField] private Transform spawnPoint; // Point where the bullet will spawn
    [SerializeField] private ParticleSystem MuzzleFlash;
    public void Shoot()
    {
        if (bulletPrefab != null && spawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation); // Rotate 90 degrees counterclockwise
            bullet.tag = "Bullet"; // Add the 'Bullet' tag to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.forward * 20f; // Adjust speed as needed
            }
            Destroy(bullet, 10f); // Destroy the bullet after 10 seconds
            if (MuzzleFlash != null)
            {
                MuzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                MuzzleFlash.Play();
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned.");
        }
    }
}
