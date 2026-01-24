/*
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GunGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // The gun is being held by the player
            // You can add logic here to handle shooting or other interactions
            animator.SetBool("IsEquipped", true);
        }
    }
}
*/
using UnityEngine;
public class GunGrab : MonoBehaviour
{
    public Animator animator;


    public void GunGrabbed()
    {
        //animator.SetBool("IsEquipped", true);
        animator.SetTrigger("Equip");
    }

    public void GunUnGrabbed()
    {
        //animator.SetBool("IsEquipped", false);
    }
}