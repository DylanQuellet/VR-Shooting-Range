using UnityEngine;

public class ImpactHit : MonoBehaviour
{
    [SerializeField] private ParticleSystem Explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (Explosion != null)
            {
                Explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                Explosion.Play();
            }
            Destroy(gameObject);
        }
    }
}
