using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrimoirePageAnchor : MonoBehaviour
{
    [SerializeField] Color baseColor;
    [SerializeField] Color hoverColor;
    [SerializeField] Color selectColor;

    [SerializeField] HandGrabInteractable movementTrackerGrabbable;
    [SerializeField] Transform movementTrackerGrabbableCollider;
    [SerializeField] Vector3 triggerPosition;

    [SerializeField] UnityEvent OnPositionTriggered;

    private bool isHovered;
    private bool isSelected;
    private bool canTrigger = true;

    [SerializeField] Renderer rend;

    private void Update()
    {
        if(movementTrackerGrabbable.transform.localPosition == triggerPosition && canTrigger)
        {
            canTrigger = false;
            OnPositionTriggered?.Invoke();
            movementTrackerGrabbable.Disable();
            movementTrackerGrabbable.transform.localPosition = Vector3.zero;
            movementTrackerGrabbable.Enable();
            //movementTrackerGrabbableCollider.localPosition = Vector3.zero;
        }
    }

    public void HandleHover()
    {
        isHovered = true;
        UpdateColor();
    }

    public void HandleUnhover()
    {
        isHovered = false;
        UpdateColor();
    }

    public void HandleSelect()
    {
        isSelected = true;
        canTrigger = true;
        UpdateColor();
    }

    public void HandleUnselect()
    {
        isSelected = false;
        canTrigger = false;
        UpdateColor();
    }

    private void UpdateColor()
    {
        rend.material.color = isSelected ? selectColor : (isHovered ? hoverColor : baseColor);
    }
}
