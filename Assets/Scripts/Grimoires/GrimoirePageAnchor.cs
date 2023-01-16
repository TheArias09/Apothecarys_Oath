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

    private float epsilon = 0.0005f;

    private void Update()
    {
        if((movementTrackerGrabbable.transform.localPosition - triggerPosition).sqrMagnitude < epsilon && canTrigger)
        {
            canTrigger = false;
            OnPositionTriggered?.Invoke();
            ResetGrabbable();
            //movementTrackerGrabbableCollider.localPosition = Vector3.zero;
        }
    }

    private void ResetGrabbable()
    {
        movementTrackerGrabbable.Disable();
        movementTrackerGrabbable.transform.localPosition = Vector3.zero;
        movementTrackerGrabbable.Enable();
    }

    public void DeactivateAnchor()
    {
        rend.enabled = false;
        this.enabled = false;
    }

    public void ActivateAnchor()
    {
        rend.enabled = true;
        this.enabled = true;
    }

    public void HandleHover()
    {
        isHovered = true;
        UpdateColor();
    }

    public void HandleUnhover()
    {
        isHovered = false;
        ResetGrabbable();
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
        ResetGrabbable();
        UpdateColor();
    }

    private void UpdateColor()
    {
        rend.material.color = isSelected ? selectColor : (isHovered ? hoverColor : baseColor);
    }
}
