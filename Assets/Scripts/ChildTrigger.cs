using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    public ImpactHitPool parentScript;

    private void OnTriggerEnter(Collider other)
    {
        parentScript?.OnChildTriggerEnter(other);
    }
}
