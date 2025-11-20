using System.Collections;
using UnityEngine;
using SchoolPanicRoguelike.Enemies;

namespace SchoolPanicRoguelike.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerAttack2D : MonoBehaviour
    {
        [SerializeField]
        private Collider2D attackCollider;

        [SerializeField]
        private float attackDamage = 10f;

        [SerializeField]
        private float attackCooldown = 0.5f;

        [SerializeField]
        private float attackActiveTime = 0.2f;

        private bool _isAttacking;
        private float _lastAttackTime = -999f;
        private float _attackModifier = 0f;

        private void Awake()
        {
            if (attackCollider != null)
            {
                attackCollider.enabled = false;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryAttack();
            }
        }

        public void AddAttackModifier(float bonus)
        {
            _attackModifier += bonus;
        }

        private void TryAttack()
        {
            if (_isAttacking)
            {
                return;
            }

            if (Time.time - _lastAttackTime < attackCooldown)
            {
                return;
            }

            StartCoroutine(PerformAttack());
        }

        private IEnumerator PerformAttack()
        {
            _isAttacking = true;
            _lastAttackTime = Time.time;

            if (attackCollider != null)
            {
                attackCollider.enabled = true;
            }

            yield return new WaitForSeconds(attackActiveTime);

            if (attackCollider != null)
            {
                attackCollider.enabled = false;
            }

            _isAttacking = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isAttacking || attackCollider == null)
            {
                return;
            }

            ZombieStats zombie = other.GetComponent<ZombieStats>();
            if (zombie != null)
            {
                float totalDamage = attackDamage + _attackModifier;
                zombie.TakeDamage(totalDamage);
            }
        }
    }
}
