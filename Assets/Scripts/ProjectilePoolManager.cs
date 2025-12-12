using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance;

    public float maxDistance = 40f;
    public float checkInterval = 0.3f;

    private readonly List<Bullet> activeProjectiles = new List<Bullet>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CleanupRoutine());
    }

    public void RegisterProjectile(Bullet b)
    {
        if (!activeProjectiles.Contains(b))
            activeProjectiles.Add(b);
    }

    public void UnregisterProjectile(Bullet b)
    {
        activeProjectiles.Remove(b);
    }

    private IEnumerator CleanupRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(checkInterval);

        while (true)
        {
            yield return delay;

            for (int i = activeProjectiles.Count - 1; i >= 0; i--)
            {
                var proj = activeProjectiles[i];

                if (!proj.gameObject.activeInHierarchy)
                {
                    activeProjectiles.RemoveAt(i);
                    continue;
                }

                float distance = Vector3.Distance(proj.spawnPosition, proj.transform.position);

                if (distance > maxDistance)
                    proj.DisableBullet();
            }
        }
    }
}
