
/*
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FXLoader : MonoBehaviour
{
    private static bool isQuest;   // devient static
    public static bool IsQuest => isQuest;  // getter public

    public static GameObject MuzzlePrefab;
    public static GameObject ExplosionPrefab;

    public static GameObject GunModelPrefab;

    private void Awake()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                    isQuest = true;
        #else
                isQuest = false;
        #endif
    }

    private async void Start()
    {
        string muzzleKey = isQuest ? "Muzzle_VR" : "Muzzle_PC";
        string explosionKey = isQuest ? "Explosion_VR" : "Explosion_PC";

        MuzzlePrefab = await Addressables.LoadAssetAsync<GameObject>(muzzleKey).Task;
        ExplosionPrefab = await Addressables.LoadAssetAsync<GameObject>(explosionKey).Task;

        string gunModelKey = isQuest ? "GunModel_VR" : "GunModel_PC";
        GunModelPrefab = await Addressables.LoadAssetAsync<GameObject>(gunModelKey).Task;

    }
}
*/
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Threading.Tasks;

public class FXLoader : MonoBehaviour
{
    private static bool isQuest;
    public static bool IsQuest => isQuest;

    public static GameObject MuzzlePrefab;
    public static GameObject ExplosionPrefab;
    public static GameObject GunModelPrefab;

    public static event Action OnGunLoaded;

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        isQuest = true;
#else
        isQuest = false;
#endif
    }

    private async void Start()
    {
        string muzzleKey = isQuest ? "Muzzle_VR" : "Muzzle_PC";
        string explosionKey = isQuest ? "Explosion_VR" : "Explosion_PC";
        string gunModelKey = isQuest ? "GunModel_VR" : "GunModel_PC";

        // Chargement asynchrone des FX
        MuzzlePrefab = await Addressables.LoadAssetAsync<GameObject>(muzzleKey).Task;
        ExplosionPrefab = await Addressables.LoadAssetAsync<GameObject>(explosionKey).Task;

        // Chargement du gun
        GunModelPrefab = await Addressables.LoadAssetAsync<GameObject>(gunModelKey).Task;

        // Notifie que le gun est prêt
        OnGunLoaded?.Invoke();
    }
}
