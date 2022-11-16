using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recipients
{
    public class Flowing : MonoBehaviour
    {
        [SerializeField] Transform bottleneckCenterPoint;
        [SerializeField] float bottleneckRadius;

        [SerializeField] float sphereCastRadius = 0.01f;
        [SerializeField] float maxSphereCastDistance = 10;
        [SerializeField] LayerMask sphereCastLayerMask;

        Recipient recipient;

        private void Awake()
        {
            recipient = GetComponent<Recipient>();
        }

        private float GetFlowAngle()
        {
            var axis = Vector3.Cross(Vector3.up, transform.up);
            var flowAngle = Vector3.SignedAngle(Vector3.up, transform.up, axis);

            return flowAngle - 90;
        }

        private Vector3 GetFlowPoint()
        {
            var xProjection = Vector3.Dot(transform.right, Vector3.down);
            var zProjection = Vector3.Dot(transform.forward, Vector3.down);

            var projection = transform.right * xProjection + transform.forward * zProjection;

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
            Debug.Log("Flow!");

            var hits = Physics.SphereCastAll(GetFlowPoint(), sphereCastRadius, Vector3.down, maxSphereCastDistance, sphereCastLayerMask);
            
            foreach(var hit in hits)
            {
                if (hit.transform.gameObject == gameObject) continue;

                Debug.Log("Fill!");

                var target = hit.collider.GetComponentInParent<Recipient>();
                
                recipient.PourIn(target);

                return;
            }

            recipient.PourInVoid();
        }

        private void OnDrawGizmos()
        {
            var point = GetFlowPoint();

            Gizmos.DrawSphere(point, sphereCastRadius);
        }
    }
}
