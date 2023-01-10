using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandInfo : MonoBehaviour
{
    public enum GrabHandType { None, Left, Right };

    public GrabHandType GrabHand { get; private set; }

    private HandGrabInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<HandGrabInteractable>();
    }

    private void Update()
    {
        if(interactable.Interactors.Count == 0)
        {
            GrabHand = GrabHandType.None;
            return;
        }

        foreach (HandGrabInteractor interactor in interactable.Interactors)
        {
            GrabHand = interactor.Hand.Handedness == Oculus.Interaction.Input.Handedness.Left ? GrabHandType.Left : GrabHandType.Right;
        }
    }
}
