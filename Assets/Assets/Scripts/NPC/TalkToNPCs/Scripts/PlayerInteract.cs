using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerInteract : MonoBehaviour
{

    public XRNode inputSource;
    private InputDevice device;

    private void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    private void Update()
    {
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
        {
            InteractWithObject();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            interactable.Interact(transform);
        }
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}