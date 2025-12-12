using UnityEngine;
using System.Collections;

public class AutoDisableFX : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    private void OnEnable()
    {
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);

        // Désactive l'objet
        gameObject.SetActive(false);
    }
}
