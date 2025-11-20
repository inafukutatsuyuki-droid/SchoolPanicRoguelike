using System.Collections;
using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Enemies
{
    public class GuardController2D : MonoBehaviour
    {
        private enum GuardState
        {
            Patrol,
            Alert,
            Chase
        }

        [SerializeField]
        private float detectionRange = 10f;

        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private float alertDuration = 5f;

        private Rigidbody2D _rigidbody;
        private GuardState _state = GuardState.Patrol;
        private Transform _target;
        private PlayerStats _targetStats;
        private Coroutine _alertCountdownCoroutine;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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
            }

            if (_state == GuardState.Chase && _target != null)
            {
                float distance = Vector2.Distance(transform.position, _target.position);
                if (distance > detectionRange)
                {
                    OnLostSight();
                }
            }
        }

        private void FixedUpdate()
        {
            if (_target == null)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            switch (_state)
            {
                case GuardState.Chase:
                    MoveTowardsTarget();
                    break;
                default:
                    _rigidbody.velocity = Vector2.zero;
                    break;
            }
        }

        private void MoveTowardsTarget()
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rigidbody.velocity = direction * moveSpeed;
        }

        public void OnDetectedTarget(Transform target)
        {
            _target = target;
            _targetStats = target.GetComponent<PlayerStats>();
            EnterChaseState();
        }

        public void OnHeardNoise(Transform suspectedTarget)
        {
            _target = suspectedTarget;
            _targetStats = suspectedTarget.GetComponent<PlayerStats>();
            EnterAlertState();
        }

        public void OnLostSight()
        {
            if (_state == GuardState.Chase)
            {
                EnterAlertState();
            }
        }

        private void EnterChaseState()
        {
            _state = GuardState.Chase;
            CancelAlertCountdown();
        }

        private void EnterAlertState()
        {
            _state = GuardState.Alert;
            RestartAlertCountdown();
        }

        private void RestartAlertCountdown()
        {
            CancelAlertCountdown();
            _alertCountdownCoroutine = StartCoroutine(AlertCountdown());
        }

        private void CancelAlertCountdown()
        {
            if (_alertCountdownCoroutine != null)
            {
                StopCoroutine(_alertCountdownCoroutine);
                _alertCountdownCoroutine = null;
            }
        }

        private IEnumerator AlertCountdown()
        {
            yield return new WaitForSeconds(alertDuration);
            if (_state == GuardState.Alert)
            {
                _state = GuardState.Patrol;
            }
            _alertCountdownCoroutine = null;
        }

        private void FindTarget()
        {
            if (_target != null)
            {
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _target = player.transform;
                _targetStats = player.GetComponent<PlayerStats>();
            }
        }

        private void OnDisable()
        {
            CancelAlertCountdown();
        }
    }
}
