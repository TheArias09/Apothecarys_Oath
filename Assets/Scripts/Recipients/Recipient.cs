using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recipients
{
    public class Recipient : MonoBehaviour
    {
        [SerializeField, Min(0.0f)] float quantity = 0;
        [SerializeField, Min(0.0f)] float maxQuantity = 1;
        [SerializeField, Min(0.0f)] float pourSpeed = 0.1f;

        [SerializeField] bool affectsShader = false;
        [SerializeField] GameObject shaderObject;

        Renderer rend;

        private void Awake()
        {
            if (shaderObject == null) return;
            rend = shaderObject.GetComponent<Renderer>();
        }

        private void Update()
        {
            if (affectsShader)
            {
                Debug.Log("Update with: " + (quantity * 0.2f - 0.1f).ToString());
                rend.material.SetFloat("_Fill", quantity * 0.2f - 0.1f);
            }
        }

        public void PourInVoid()
        {
            quantity -= Time.deltaTime * pourSpeed;

            quantity = Mathf.Clamp(quantity, 0, maxQuantity);
        }

        public void PourIn(Recipient recipient)
        {
            var quantityPulled = Time.deltaTime * pourSpeed;
            if (quantityPulled > quantity)
            {
                quantityPulled = quantity;
            }

            quantity -= quantityPulled;
            recipient.quantity += quantityPulled;

            if (recipient.quantity < 0) recipient.quantity = 0;
            if (recipient.quantity > recipient.maxQuantity) recipient.quantity = recipient.maxQuantity;
            if (quantity > maxQuantity) quantity = maxQuantity;
        }
    }
}