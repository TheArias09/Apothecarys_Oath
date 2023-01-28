using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabHandInfo : MonoBehaviour
{
    public enum GrabHandType { None, Left, Right };

    public GrabHandType GrabHand { get; private set; }

    private List<HandGrabInteractable> interactables;

    private void Awake()
    {
        interactables = GetComponentsInChildren<HandGrabInteractable>().ToList();
    }

    private void Update()
    {
        GrabHand = GrabHandType.None;

        foreach(var interactable in interactables)
        {
            if (interactable.Interactors.Count == 0) continue;

            foreach (var interactor in interactable.Interactors)
            {
                GrabHand = interactor.Hand.Handedness == Oculus.Interaction.Input.Handedness.Left ? GrabHandType.Left : GrabHandType.Right;
                return;
            }
        }
    }
}
