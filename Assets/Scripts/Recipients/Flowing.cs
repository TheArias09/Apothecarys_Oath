using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recipients
{
    public class Flowing : MonoBehaviour
    {
        //TODO: Gérer l'influence de l'angle et du remplissage en fonciton 

        [SerializeField] Transform bottleneckCenterPoint;
        [SerializeField] float bottleneckRadius;

        [SerializeField] float sphereCastRadius = 0.01f;
        [SerializeField] float maxSphereCastDistance = 10;
        [SerializeField] LayerMask sphereCastLayerMask;

        [SerializeField] float pourSpeed = 0.1f;

        IngredientWrapper ingredientWrapper;

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
            var flowAngle = GetFlowAngle();

            if(flowAngle > 0)
            {
                Flow();
            }
        }

        private void Flow()
        {
            Debug.Log("Flow!" + gameObject.name);

            var hits = Physics.SphereCastAll(GetFlowPoint(), sphereCastRadius, Vector3.down, maxSphereCastDistance, sphereCastLayerMask);
            
            foreach(var hit in hits)
            {
                if (hit.transform.gameObject == gameObject) continue;

                Debug.Log("Fill!");

                var targetIngredientWrapper = hit.collider.GetComponentInParent<IngredientWrapper>();

                //TODO: Take flowAngle into account.
                var deltaQuantity = pourSpeed * Time.deltaTime;

                List<Ingredient> pouredIngredients = ingredientWrapper.Pour(deltaQuantity);

                if (pouredIngredients != null)
                {
                    deltaQuantity = pouredIngredients.Sum(ing => ing.Quantity);
                    var filledCorrectly = targetIngredientWrapper.FillWith(pouredIngredients, deltaQuantity);
                    
                    if (!filledCorrectly) Overflow();
                }

                return;
            }

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
