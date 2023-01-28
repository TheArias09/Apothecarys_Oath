using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationPlayer : MonoBehaviour
{
    //This is the default length of OVRInput's vibration, somehow it cannot be changed
    private static readonly float VIBRATION_DURATION = 2f;

    [SerializeField][Range(0, 1)] private float strength;
    [SerializeField][Range(0, 1)] private float frequency;

    private GrabHandInfo grabHandInfo;
    private GrabHandInfo.GrabHandType vibratingHand;

    private bool vibrationActive = false;
    private float vibrationTimer;

    private void Start()
    {
        grabHandInfo = GetComponent<GrabHandInfo>();
    }

    private void Update()
    {
        if (!vibrationActive) return;
        
        vibrationTimer -= Time.deltaTime;
        if (vibrationTimer < 0) Vibrate(grabHandInfo.GrabHand);

    }

    private void Vibrate(GrabHandInfo.GrabHandType hand)
    {
        vibrationTimer = VIBRATION_DURATION;
        if (!vibrationActive) return;

        if (hand == GrabHandInfo.GrabHandType.Left)
        {
            vibratingHand = GrabHandInfo.GrabHandType.Left;
            OVRInput.SetControllerVibration(frequency, strength, OVRInput.Controller.LTouch);
        }
        else if (hand == GrabHandInfo.GrabHandType.Right)
        {
            vibratingHand = GrabHandInfo.GrabHandType.Right;
            OVRInput.SetControllerVibration(frequency, strength, OVRInput.Controller.RTouch);
        }
    }

    public void StartVibration()
    {
        vibrationActive = true;
        Vibrate(grabHandInfo.GrabHand);
    }

    public void StopVibration()
    {
        vibrationActive = false;

        if (vibratingHand == GrabHandInfo.GrabHandType.Left)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
        else if (vibratingHand == GrabHandInfo.GrabHandType.Right)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
}
