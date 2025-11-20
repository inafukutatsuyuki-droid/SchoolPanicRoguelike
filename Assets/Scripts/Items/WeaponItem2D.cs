using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Items
{
    public class WeaponItem2D : ItemBase2D
    {
        [SerializeField]
        private float attackBonus = 5f;

        protected override void HandleCollected(GameObject playerObject)
        {
            PlayerAttack2D attack = playerObject.GetComponent<PlayerAttack2D>();
            if (attack != null)
            {
                attack.AddAttackModifier(attackBonus);
            }
        }
    }
}
