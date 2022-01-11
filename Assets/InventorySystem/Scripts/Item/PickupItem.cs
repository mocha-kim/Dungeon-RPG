using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

namespace RPG.InventorySystem.Items
{

    public class PickupItem : MonoBehaviour, IInteractable
    {
        public float distance = 1.0f;
        public float Distance => distance;

        public ItemObject itemObject;
        public GameObject particlePrefab;

        public void Interact(GameObject other)
        {
            float calcDistance = Vector3.Distance(transform.position, other.transform.position);

            if (calcDistance > distance)
                return;

            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player?.PickupItem(itemObject) ?? false)
                Destroy(gameObject);
        }

        public void StopInteract(GameObject other)
        {
            throw new System.NotImplementedException();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            GetComponent<SpriteRenderer>().sprite = itemObject?.icon;
#endif
        }

        private void Start()
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            particle.transform.parent = transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }

}