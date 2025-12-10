using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public float speed = 20f;
    public float lifeTime = 2f;
    private Rigidbody rb;

    // Appel� automatiquement par l'ObjectPooler lors de l'activation
    public void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // 1. R�initialiser la vitesse (important si le RB a gard� l'inertie pr�c�dente)
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 2. Appliquer la force
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        // 3. D�sactiver automatiquement apr�s X secondes (remplace Destroy)
        Invoke("DisableBullet", lifeTime);
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Logique de d�g�ts ici...

        // D�sactiver imm�diatement � l'impact
        // CancelInvoke("DisableBullet"); // Annuler le timer de vie
        //DisableBullet();
    }
}