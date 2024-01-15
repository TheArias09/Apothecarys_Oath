using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Recipients
{
    public class Flowing : MonoBehaviour
    {
        [SerializeField] private Transform bottleneckCenterPoint;
        [SerializeField] private Collider bottleneckCollider;
        [SerializeField] private float bottleneckRadius;

        [SerializeField] private float sphereCastRadius = 0.01f;
        [SerializeField] private float maxSphereCastDistance = 10;
        [SerializeField] private LayerMask sphereCastLayerMask;

        [SerializeField] private AnimationCurve fillCoefficientToAngleThreshold;
        [SerializeField] private AnimationCurve deltaAngleToPourSpeed;
        [SerializeField] private float maxPourSpeed = 0.5f;

        [SerializeField] bool flowOnGrab = false;

        private enum FlowingState {NOT_FLOWING, VOID_FLOWING, RECIPIENT_FLOWING};
        private FlowingState currentFlowingState = FlowingState.NOT_FLOWING; 

        [Header("Events")]
        [SerializeField] UnityEvent OnStartFlowingInRecipient;
        [SerializeField] UnityEvent OnStartFlowingInVoid;
        [SerializeField] UnityEvent OnStopFlowing;
        [SerializeField] UnityEvent OnPressingIngredient;
        

        public bool IsFlowing { get; private set; } = false;
        public float CurrentDeltaQuantity { get; private set; } = 0f;

        private GrabHandInfo grabHandInfo;

        private IngredientWrapper ingredientWrapper;

        private void Awake()
        {
            ingredientWrapper = GetComponent<IngredientWrapper>();
            grabHandInfo = GetComponent<GrabHandInfo>();
            currentFlowingState = FlowingState.NOT_FLOWING;
            if(bottleneckCollider)
                bottleneckRadius = bottleneckCollider.transform.lossyScale.x / 2;
        }

        private float GetFlowAngle()
        {
            var axis = Vector3.Cross(Vector3.up, bottleneckCenterPoint.up);
            var flowAngle = Vector3.SignedAngle(Vector3.up, bottleneckCenterPoint.up, axis);

            return flowAngle - 90;
        }

        private float ComputeAngleThreshold()
        {
            // FillCoefficient between 0 and 1.
            var fillCoefficient = ingredientWrapper.GetTotalQty() / ingredientWrapper.RecipientQuantity;

            var angleThresholdNormalized = fillCoefficientToAngleThreshold.Evaluate(fillCoefficient);
            // AngleThreshold between -90f and 90f.
            return -fillCoefficientToAngleThreshold.Evaluate(fillCoefficient) * 180f + 90f;
        }

        private float ComputeDeltaAngle()
        {
            return GetFlowAngle() - ComputeAngleThreshold();
        }

        public Vector3 GetFlowPoint()
        {
            if (flowOnGrab) return transform.position;

            var xProjection = Vector3.Dot(bottleneckCenterPoint.right, Vector3.down);
            var zProjection = Vector3.Dot(bottleneckCenterPoint.forward, Vector3.down);

            var projection = bottleneckCenterPoint.right * xProjection + bottleneckCenterPoint.forward * zProjection;

            var point = bottleneckCenterPoint.position + bottleneckRadius * projection.normalized;

            return point;
        }

        private bool GetLeftPalmInput()
        {
            return (OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.Four) || OVRInput.Get(OVRInput.Button.PrimaryThumbstick)) &&
                   OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) != 0 &&
                   OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) != 0;
        }

        private bool GetRightPalmInput()
        {
            return (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.SecondaryThumbstick)) &&
                   OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 &&
                   OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) != 0;
        }

        private void Update()
        {
            // Buffer old state
            var oldFlowingState = currentFlowingState;

            if(flowOnGrab)
            {
                if (grabHandInfo.GrabHand == GrabHandInfo.GrabHandType.Left && GetLeftPalmInput()
                    || grabHandInfo.GrabHand == GrabHandInfo.GrabHandType.Right && GetRightPalmInput()
                    || Input.GetKey(KeyCode.Space)
                    )
                {
                    if (!IsFlowing)
                    {
                        OnPressingIngredient.Invoke();
                    }
                    Flow(90f);
                }
                else
                {
                    IsFlowing = false;
                    currentFlowingState = FlowingState.NOT_FLOWING;
                    CurrentDeltaQuantity = 0;
                }

                if (ingredientWrapper.GetTotalQty() <= 0f)
                {
                    Destroy(gameObject);
                }
                CheckFlowingTransition(oldFlowingState);
                return;
            }

            var deltaAngle = ComputeDeltaAngle();

            var snap = GetComponentInChildren<SnapInteractable>();

            if (snap && snap.Interactors.Count > 0)
            {
                IsFlowing = false;
                currentFlowingState = FlowingState.NOT_FLOWING;
                CurrentDeltaQuantity = 0;
                CheckFlowingTransition(oldFlowingState);
                return;
            }

            if (deltaAngle <= 0)
            {
                IsFlowing = false;
                currentFlowingState = FlowingState.NOT_FLOWING;
                CurrentDeltaQuantity = 0;
                CheckFlowingTransition(oldFlowingState);
                return;
            }

            Flow(deltaAngle);
            CheckFlowingTransition(oldFlowingState);
        }

        private void Flow(float deltaAngle)
        {
            var deltaAngleNormalized = Mathf.InverseLerp(0, 180 - (ComputeAngleThreshold() + 90f), deltaAngle);
            var pourSpeed = deltaAngleToPourSpeed.Evaluate(deltaAngleNormalized) * maxPourSpeed;
            var deltaQuantity = pourSpeed * Time.deltaTime;

            CurrentDeltaQuantity = deltaQuantity;

            var hits = Physics.SphereCastAll(GetFlowPoint(), sphereCastRadius, Vector3.down, maxSphereCastDistance, sphereCastLayerMask);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == gameObject) continue;

                var targetIngredientWrapper = hit.collider.GetComponentInParent<IngredientWrapper>();
                List<Ingredient> pouredIngredients = ingredientWrapper.Pour(deltaQuantity);

                if (pouredIngredients != null && pouredIngredients.Count != 0)
                {
                    IsFlowing = true;
                    currentFlowingState = FlowingState.RECIPIENT_FLOWING;
                    deltaQuantity = pouredIngredients.Sum(ing => ing.Quantity);

                    var filledCorrectly = targetIngredientWrapper.FillWith(pouredIngredients, deltaQuantity);
                    if (!filledCorrectly) Overflow();
                }
                else
                {
                    IsFlowing = false;
                    currentFlowingState = FlowingState.NOT_FLOWING;
                    CurrentDeltaQuantity = 0;
                }

                return;
            }

            // Pour in void.
            var pouredIngredientsInVoid = ingredientWrapper.Pour(deltaQuantity);

            IsFlowing = (pouredIngredientsInVoid != null) && (pouredIngredientsInVoid.Count != 0);
            // UDPATE STATE ACCORDINGLY (either not flowing or flowing in Void)
            currentFlowingState = IsFlowing ? FlowingState.VOID_FLOWING : FlowingState.NOT_FLOWING;
            if (!IsFlowing) CurrentDeltaQuantity = 0;
        }


        private void CheckFlowingTransition(FlowingState oldState)
        {
            if(oldState == FlowingState.NOT_FLOWING)
            {
                if(currentFlowingState == FlowingState.VOID_FLOWING)
                {
                    OnStartFlowingInVoid.Invoke();
                }

                if(currentFlowingState == FlowingState.RECIPIENT_FLOWING)
                {
                    OnStartFlowingInRecipient.Invoke();
                }

                return;
            }

            if(oldState == FlowingState.VOID_FLOWING)
            {
                if(currentFlowingState == FlowingState.NOT_FLOWING)
                {
                    OnStopFlowing.Invoke();
                }

                if(currentFlowingState == FlowingState.RECIPIENT_FLOWING)
                {
                    OnStartFlowingInRecipient.Invoke();
                }

                return;
            }

            if(oldState == FlowingState.RECIPIENT_FLOWING)
            {
                if(currentFlowingState == FlowingState.NOT_FLOWING)
                {
                    OnStopFlowing.Invoke();
                }
                if(currentFlowingState == FlowingState.VOID_FLOWING)
                {
                    OnStartFlowingInVoid.Invoke();
                }
                return;
            }

        }

        public GameObject GetTargetPotion()
        {
            var hits = Physics.SphereCastAll(GetFlowPoint(), sphereCastRadius, Vector3.down, maxSphereCastDistance, sphereCastLayerMask);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == gameObject) continue;
                return hit.transform.gameObject;
            }

            return null;
        }

        private static void Overflow()
        {
            Debug.Log("Overflow!");
        }

        private void OnDrawGizmos()
        {
            var point = GetFlowPoint();
            Gizmos.DrawSphere(point, sphereCastRadius);
        }
    }
}