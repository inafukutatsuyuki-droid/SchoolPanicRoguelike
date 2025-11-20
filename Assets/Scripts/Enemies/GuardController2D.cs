using System.Collections;
using UnityEngine;
using SchoolPanicRoguelike.Game;
using SchoolPanicRoguelike.Player;
using SchoolPanicRoguelike.UI;

namespace SchoolPanicRoguelike.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(GuardVision2D))]
    [RequireComponent(typeof(GuardHearing2D))]
    public class GuardController2D : MonoBehaviour
    {
        private enum GuardState
        {
            Patrol,
            Chase,
            Alert
        }

        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private float chaseSpeed = 4.5f;

        [SerializeField]
        private float alertDuration = 5f;

        [SerializeField]
        private float patrolWaitTime = 1.5f;

        [SerializeField]
        private float attackRange = 1.3f;

        [SerializeField]
        private float attackDamage = 15f;

        [SerializeField]
        private float attackCooldown = 1.25f;

        [SerializeField]
        private Transform[] patrolPoints;

        private Rigidbody2D _rigidbody;
        private GuardVision2D _vision;
        private GuardHearing2D _hearing;
        private PlayerStats _player;
        private GuardState _state = GuardState.Patrol;
        private int _currentPatrolIndex;
        private float _lastSeenTime;
        private float _nextAttackTime;
        private Vector2 _alertTarget;
        private Coroutine _patrolWaitRoutine;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _vision = GetComponent<GuardVision2D>();
            _hearing = GetComponent<GuardHearing2D>();
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerStats>();
            _vision.Initialize(this, _player);
            _hearing.Initialize(this);
        }

        private void Update()
        {
            switch (_state)
            {
                case GuardState.Patrol:
                    HandlePatrol();
                    break;
                case GuardState.Chase:
                    HandleChase();
                    break;
                case GuardState.Alert:
                    HandleAlert();
                    break;
            }
        }

        public void OnPlayerSpotted(float attentionScore)
        {
            _lastSeenTime = Time.time;
            _state = GuardState.Chase;
            ReportAttention(attentionScore);
        }

        public void OnVisionMaintained(float attentionScore)
        {
            _lastSeenTime = Time.time;
            ReportAttention(attentionScore);
        }

        public void OnLostSight()
        {
            if (_state == GuardState.Chase)
            {
                _state = GuardState.Alert;
                _alertTarget = _player != null ? (Vector2)_player.transform.position : transform.position;
                StartCoroutine(AlertCountdown());
            }
        }

        public void OnHeardNoise(Vector2 noisePosition)
        {
            if (_state == GuardState.Chase)
            {
                return;
            }

            _state = GuardState.Alert;
            _alertTarget = noisePosition;
            StartCoroutine(AlertCountdown());
        }

        private void HandlePatrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            Transform target = patrolPoints[_currentPatrolIndex];
            Vector2 direction = ((Vector2)target.position - _rigidbody.position).normalized;
            _rigidbody.velocity = direction * moveSpeed;

            if (Vector2.Distance(_rigidbody.position, target.position) <= 0.2f && _patrolWaitRoutine == null)
            {
                _rigidbody.velocity = Vector2.zero;
                _patrolWaitRoutine = StartCoroutine(WaitAndMoveToNext());
            }
        }

        private IEnumerator WaitAndMoveToNext()
        {
            yield return new WaitForSeconds(patrolWaitTime);
            _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
            _patrolWaitRoutine = null;
        }

        private void HandleChase()
        {
            if (_player == null)
            {
                _state = GuardState.Patrol;
                return;
            }

            Vector2 direction = ((Vector2)_player.transform.position - _rigidbody.position).normalized;
            _rigidbody.velocity = direction * chaseSpeed;

            float distance = Vector2.Distance(_player.transform.position, transform.position);
            if (distance <= attackRange)
            {
                TryAttack();
            }

            if (Time.time - _lastSeenTime > 3f)
            {
                OnLostSight();
            }
        }

        private void HandleAlert()
        {
            Vector2 direction = (_alertTarget - _rigidbody.position).normalized;
            _rigidbody.velocity = direction * moveSpeed;
        }

        private IEnumerator AlertCountdown()
        {
            yield return new WaitForSeconds(alertDuration);
            _state = GuardState.Patrol;
        }

        private void TryAttack()
        {
            if (Time.time < _nextAttackTime || _player == null)
            {
                return;
            }

            _player.TakeDamage(attackDamage);
            _nextAttackTime = Time.time + attackCooldown;
        }

        private void ReportAttention(float attentionScore)
        {
            DetectionMeter2D meter = FindObjectOfType<DetectionMeter2D>();
            if (meter != null)
            {
                meter.ReportAttention(attentionScore);
            }
        }

        public void SetPatrolRoute(Transform[] points)
        {
            patrolPoints = points;
            _currentPatrolIndex = 0;
        }
    }
}
