using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(ZombieStats))]
    public class ZombieController2D : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 2.5f;

        [SerializeField]
        private float detectionRange = 8f;

        [SerializeField]
        private float attackRange = 1.2f;

        [SerializeField]
        private float attackCooldown = 1.5f;

        private Rigidbody2D _rigidbody;
        private ZombieStats _stats;
        private Transform _target;
        private PlayerStats _targetStats;
        private float _lastAttackTime = -999f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _stats = GetComponent<ZombieStats>();
        }

        private void Start()
        {
            FindTarget();
        }

        private void Update()
        {
            if (_target == null)
            {
                FindTarget();
                return;
            }

            float distance = Vector2.Distance(transform.position, _target.position);

            if (distance <= attackRange)
            {
                TryAttack();
            }
        }

        private void FixedUpdate()
        {
            if (_target == null)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            Vector2 direction = (_target.position - transform.position);
            float distance = direction.magnitude;

            if (distance <= detectionRange)
            {
                direction.Normalize();
                _rigidbody.velocity = direction * moveSpeed;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        private void FindTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _target = player.transform;
                _targetStats = player.GetComponent<PlayerStats>();
            }
        }

        private void TryAttack()
        {
            if (_targetStats == null)
            {
                return;
            }

            if (Time.time - _lastAttackTime < attackCooldown)
            {
                return;
            }

            _lastAttackTime = Time.time;
            _targetStats.TakeDamage(_stats.AttackPower);
        }
    }
}
