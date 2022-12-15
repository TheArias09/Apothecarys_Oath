using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recipients
{
    public class Flowing : MonoBehaviour
    {
        //TODO: G�rer l'influence de l'angle et du remplissage en fonciton 

        [SerializeField] private Transform bottleneckCenterPoint;
        [SerializeField] private float bottleneckRadius;

        [SerializeField] private float sphereCastRadius = 0.01f;
        [SerializeField] private float maxSphereCastDistance = 10;
        [SerializeField] private LayerMask sphereCastLayerMask;

        [SerializeField] private AnimationCurve fillCoefficientToAngleThreshold;
        [SerializeField] private AnimationCurve deltaAngleToPourSpeed;
        [SerializeField] private float maxPourSpeed = 0.5f;

        private IngredientWrapper ingredientWrapper;

        private void Awake()
        {
            ingredientWrapper = GetComponent<IngredientWrapper>();
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
            Debug.Log("Angle Threshold: " + ComputeAngleThreshold());
            return GetFlowAngle() - ComputeAngleThreshold();
        }

        private Vector3 GetFlowPoint()
        {
            var xProjection = Vector3.Dot(bottleneckCenterPoint.right, Vector3.down);
            var zProjection = Vector3.Dot(bottleneckCenterPoint.forward, Vector3.down);

            var projection = bottleneckCenterPoint.right * xProjection + bottleneckCenterPoint.forward * zProjection;

            var point = bottleneckCenterPoint.position + bottleneckRadius * projection.normalized;

            return point;
        }

        private void Update()
        { 
            var deltaAngle = ComputeDeltaAngle();

            Debug.Log("DeltaAngle: " + deltaAngle);

            var snap = GetComponentInChildren<SnapInteractable>();
            if(snap)
            {
                Debug.Log(snap.Interactors.Count);
                if (snap.Interactors.Count > 0) return;
            }

            if (deltaAngle > 0) Flow(deltaAngle);
        }

        private void Flow(float deltaAngle)
        {
            var deltaAngleNormalized = Mathf.InverseLerp(0, 180 - (ComputeAngleThreshold() + 90f), deltaAngle);
            Debug.Log("DeltaAngleNormalized: " + deltaAngleNormalized);
            var pourSpeed = deltaAngleToPourSpeed.Evaluate(deltaAngleNormalized) * maxPourSpeed;

            var deltaQuantity = pourSpeed * Time.deltaTime;

            Debug.Log("PourSpeed: " + pourSpeed);

            Debug.Log("Flow!" + gameObject.name);
            var hits = Physics.SphereCastAll(GetFlowPoint(), sphereCastRadius, Vector3.down, maxSphereCastDistance, sphereCastLayerMask);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == gameObject) continue;

                Debug.Log("Fill!");
                var targetIngredientWrapper = hit.collider.GetComponentInParent<IngredientWrapper>();
                List<Ingredient> pouredIngredients = ingredientWrapper.Pour(deltaQuantity);

                if (pouredIngredients != null)
                {
                    deltaQuantity = pouredIngredients.Sum(ing => ing.Quantity);
                    var filledCorrectly = targetIngredientWrapper.FillWith(pouredIngredients, deltaQuantity);
                    if (!filledCorrectly) Overflow();
                }
                
                return;
            }

            ingredientWrapper.Pour(deltaQuantity);
            //recipient.PourInVoid();
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
