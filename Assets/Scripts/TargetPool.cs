using UnityEngine;

public class TargetPool : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private MeshRenderer meshRenderer;
    private Color originalColor;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    public void OnObjectSpawn()
    {
        // 1. Réinitialiser la santé
        currentHealth = maxHealth;

        // 2. Réinitialiser le visuel (si la cible change de couleur quand touchée)
        meshRenderer.material.color = originalColor;

        // 3. Réactiver les colliders si vous les aviez désactivés
        GetComponent<Collider>().enabled = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Animation de mort ou particules ici...

        // Au lieu de Destroy, on désactive
        gameObject.SetActive(false);

        // Optionnel : Notifier le GameManager pour respawn une nouvelle cible
        //GameManager.Instance.RespawnTarget();
    }
}