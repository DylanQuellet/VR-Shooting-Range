using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    //public float speed = 20f;
    public Vector3 spawnPosition { get; private set; }
    private Rigidbody rb;

    public void OnObjectSpawn()
    {
        spawnPosition = transform.position;
        /*
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = transform.forward * speed;
        */
        // Enregistrement dans le manager
        ProjectilePoolManager.Instance.RegisterProjectile(this);
    }

    public void DisableBullet()
    {
        ProjectilePoolManager.Instance.UnregisterProjectile(this);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (ProjectilePoolManager.Instance != null)
            ProjectilePoolManager.Instance.UnregisterProjectile(this);
    }
}
