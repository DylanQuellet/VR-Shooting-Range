using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FXAutoSpawner : MonoBehaviour
{
    [Header("Clé Addressables de l'effet")]
    [SerializeField] private string addressableKey = "Test_Muzzle";

    [Header("Point de spawn (position + rotation)")]
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(LoadAndSpawnFX());
    }

    private System.Collections.IEnumerator LoadAndSpawnFX()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("[TestMuzzleAddressableSpawner] Aucun spawnPoint assigné.");
            yield break;
        }

        Debug.Log($"[TestMuzzleAddressableSpawner] Chargement de l'effet Addressable : {addressableKey}");

        // Lancer le chargement Addressables
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);

        // Attendre la fin du chargement
        yield return handle;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[TestMuzzleAddressableSpawner] Échec du chargement Addressable : {addressableKey}");
            yield break;
        }

        GameObject prefab = handle.Result;

        if (prefab == null)
        {
            Debug.LogError("[TestMuzzleAddressableSpawner] Le prefab Addressable chargé est null.");
            yield break;
        }

        Debug.Log("[TestMuzzleAddressableSpawner] Effet chargé avec succès. Instanciation...");

        // Instanciation
        GameObject instance = Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Jouer les particules si présentes
        var ps = instance.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
            Debug.Log("[TestMuzzleAddressableSpawner] ParticleSystem trouvé et joué.");
        }
        else
        {
            Debug.LogWarning("[TestMuzzleAddressableSpawner] Aucun ParticleSystem trouvé dans l'effet instancié.");
        }
    }
}
