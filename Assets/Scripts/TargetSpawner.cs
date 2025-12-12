/**
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Liste des positions possibles dans la scène

    public void SpawnTarget()
    {
        // Choisir une position aléatoire
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Demander une cible au pool
        ObjectPooler.Instance.SpawnFromPool("Target", spawnPoint.position, spawnPoint.rotation);
    }

    private void Update()
    {
        
    }
}
**/

using UnityEngine;
using System.Collections; // Nécessaire pour les Coroutines

public class TargetSpawner : MonoBehaviour
{
    // Points d'apparition (référencés dans l'éditeur)
    public Transform[] spawnPoints;

    // Paramètres de temps
    public float minSpawnInterval = 1f; // Temps minimum entre les spawns
    public float maxSpawnInterval = 3f; // Temps maximum entre les spawns
    

    // Le tag de la cible dans l'ObjectPooler
    private const string TargetTag = "Target";

    void Start()
    {
        // Démarrer la boucle d'apparition dès le début du jeu
        StartCoroutine(AutoSpawnTargets());
    }

    /// <summary>
    /// Coroutine qui gère l'apparition périodique des cibles.
    /// </summary>
    IEnumerator AutoSpawnTargets()
    {
        while (true) // Boucle infinie pour l'apparition
        {
            // 1. Déterminer le temps d'attente aléatoire (1s à 3s)
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // 2. Attendre le temps défini
            yield return new WaitForSeconds(spawnInterval);
            if (GameplayManager.Instance.currentTarget < GameplayManager.Instance.maxTargets)
            {
                // 3. Appeler la fonction de spawn
                GameplayManager.Instance.RegisterTargetSpawn();
                SpawnTarget();
                
            }
        }
    }

    /// <summary>
    /// Prend une cible du pool et la place à une position aléatoire.
    /// </summary>
    /*
    public void SpawnTarget()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Aucun point d'apparition (SpawnPoints) n'est assigné au TargetSpawner.");
            return;
        }

        // Choisir une position aléatoire parmi la liste
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Demander une cible au pooler (réutilise ou instancie si nécessaire)
        ObjectPooler.Instance.SpawnFromPool(TargetTag, spawnPoint.position, spawnPoint.rotation);

        // Optionnel : Vous pouvez ajouter ici la logique pour ne pas spawmer si 
        // le nombre de cibles actives est déjà trop élevé.
    }
    */
    public void SpawnTarget()
    {
        // sécurité
        if (spawnPoints.Length == 0)
            return;

        // essayer max N fois pour trouver une place libre
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawn = spawnPoints[randomIndex];

            // vérifier si un objet actif occupe ce point
            if (IsSpawnPointFree(spawn.position))
            {
                ObjectPooler.Instance.SpawnFromPool(TargetTag, spawn.position, spawn.rotation);
                return;
            }
        }

        // si on arrive ici : tous occupés
        // tu peux ajouter un "do nothing"
    }
    private bool IsSpawnPointFree(Vector3 position)
    {
        foreach (GameObject t in ObjectPooler.Instance.poolDictionary[TargetTag])
        {
            if (t.activeInHierarchy)
            {
                // Si une target active est à moins de X mètres
                if (Vector3.Distance(t.transform.position, position) < 0.1f)
                    return false;
            }
        }
        return true;
    }


}