
/*
using UnityEngine;
using UnityEngine.XR.Management;

public class GunModelLoader : MonoBehaviour
{
    [SerializeField] private Transform modelParent; // Emplacement du modèle
    private GameObject modelInstance;

    private Vector3 modelScale;
    private Vector3 modelRotation;

    private void OnEnable()
    {
        FXLoader.OnGunLoaded += LoadModel;
    }

    private void OnDisable()
    {
        FXLoader.OnGunLoaded -= LoadModel;
    }

    private void LoadModel()
    {
        if (FXLoader.GunModelPrefab == null)
        {
            Debug.LogError("GunModelPrefab non chargé !");
            return;
        }

        // Définir scale et rotation selon la plateforme
        if (!FXLoader.IsQuest)
        {
            modelScale = new Vector3(0.1f, 0.1f, 0.1f);
            modelRotation = new Vector3(90f, 0f, 90f);
        }
        else
        {
            modelScale = new Vector3(1f, 1f, 1f);
            modelRotation = new Vector3(0f, -90f, 90f);
        }

        // Instanciation du modèle
        modelInstance = Instantiate(
            FXLoader.GunModelPrefab,
            modelParent.position,
            modelParent.rotation,
            modelParent
        );

        // Appliquer scale et rotation
        modelInstance.transform.localScale = modelScale;
        modelInstance.transform.localRotation = Quaternion.Euler(modelRotation);

        // Reset position locale
        modelInstance.transform.localPosition = Vector3.zero;
    }
}
*/

using UnityEngine;

public class GunModelLoader : MonoBehaviour
{
    [SerializeField] private Transform modelParent; // Emplacement du modèle
    private GameObject modelInstance;

    private Vector3 modelScale;
    private Vector3 modelRotation;

    private void OnEnable()
    {
        FXLoader.OnGunLoaded += LoadModel;
    }

    private void OnDisable()
    {
        FXLoader.OnGunLoaded -= LoadModel;
    }

    private void LoadModel()
    {
        if (FXLoader.GunModelPrefab == null)
        {
            Debug.LogError("GunModelPrefab non chargé !");
            return;
        }

        // Définir scale et rotation selon la plateforme
        if (!FXLoader.IsQuest)
        {
            modelScale = new Vector3(0.1f, 0.1f, 0.1f);
            modelRotation = new Vector3(25f, 180f, 0f);
        }
        else
        {
            modelScale = new Vector3(1f, 1f, 1f);
            modelRotation = new Vector3(180f, 90f, 115f);
        }

        // --- Instanciation du modèle sans parent ---
        modelInstance = Instantiate(FXLoader.GunModelPrefab);

        // Appliquer scale et rotation avant de rattacher au parent
        modelInstance.transform.localScale = modelScale;
        modelInstance.transform.localRotation = Quaternion.Euler(modelRotation);

        // Rattacher au parent et reset position locale
        modelInstance.transform.SetParent(modelParent);
        modelInstance.transform.localPosition = Vector3.zero;
    }
}
