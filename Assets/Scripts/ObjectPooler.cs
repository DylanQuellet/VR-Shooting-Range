using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton pour un accès facile

    [System.Serializable]
    public class Pool
    {
        public string tag;           // Identifiant (ex: "Bullet", "Target")
        public GameObject prefab;    // L'objet à spawner
        public int size;             // Taille initiale du pool
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // On les crée mais on les cache
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Le pool avec le tag " + tag + " n'existe pas.");
            return null;
        }

        GameObject objectToSpawn;

        // Vérifie s'il y a des objets inactifs disponibles
        if (poolDictionary[tag].Count > 0 && !poolDictionary[tag].Peek().activeInHierarchy)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else
        {
            Debug.Log("No more bullets available");
            //return null;

            // Optionnel : Si le pool est vide, on peut soit attendre, soit étendre le pool.
            Pool targetPool = pools.Find(p => p.tag == tag);
            objectToSpawn = Instantiate(targetPool.prefab);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Appeler une interface ou une méthode de réinitialisation si nécessaire
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        // Remettre l'objet dans la file pour une future réutilisation
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
