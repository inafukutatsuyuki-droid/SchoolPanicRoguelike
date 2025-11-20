using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Items
{
    public enum ItemType
    {
        Weapon,
        Healing,
        Utility
    }

    [RequireComponent(typeof(Collider2D))]
    public abstract class ItemBase2D : MonoBehaviour
    {
        [SerializeField]
        protected string itemName;

        [SerializeField]
        [TextArea]
        protected string description;

        [SerializeField]
        protected ItemType itemType;

        private bool _collected;

        private void Reset()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (_collected)
            {
                return;
            }

            if (!collision.CompareTag("Player"))
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Collect(collision.gameObject);
            }
        }

        private void Collect(GameObject playerObject)
        {
            _collected = true;
            HandleCollected(playerObject);
            gameObject.SetActive(false);
        }

        protected abstract void HandleCollected(GameObject playerObject);

        public string GetItemName() => itemName;

        public ItemType GetItemType() => itemType;

        public string GetDescription() => description;
    }
}
