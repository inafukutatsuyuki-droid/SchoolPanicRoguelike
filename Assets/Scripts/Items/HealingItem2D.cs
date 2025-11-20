using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Items
{
    public class HealingItem2D : ItemBase2D
    {
        [SerializeField]
        private float healAmount = 25f;

        private bool _isInInventory;
        private PlayerStats _ownerStats;

        protected override void HandleCollected(GameObject playerObject)
        {
            _ownerStats = playerObject.GetComponent<PlayerStats>();
            _isInInventory = true;
        }

        public void Use()
        {
            if (!_isInInventory || _ownerStats == null)
            {
                return;
            }

            _ownerStats.Heal(healAmount);
            _isInInventory = false;
            Destroy(gameObject);
        }
    }
}
