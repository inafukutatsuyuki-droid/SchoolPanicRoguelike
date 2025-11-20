using UnityEngine;
using SchoolPanicRoguelike.Audio;

namespace SchoolPanicRoguelike.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerNoiseEmitter2D : MonoBehaviour
    {
        [SerializeField]
        private float baseInterval = 0.6f;

        [SerializeField]
        private float walkNoiseRadius = 2f;

        [SerializeField]
        private float dashNoiseRadius = 3.5f;

        [SerializeField]
        private float crouchNoiseRadius = 1f;

        [SerializeField]
        private float attackNoiseRadius = 1.2f;

        private Rigidbody2D _rigidbody;
        private PlayerStealthController2D _stealth;
        private float _lastNoiseTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _stealth = GetComponent<PlayerStealthController2D>();
        }

        private void Update()
        {
            EmitMovementNoise();
        }

        public void EmitAttackNoise()
        {
            NoiseEvent2D.EmitNoise(transform.position, attackNoiseRadius, 0.8f, 0.5f);
        }

        private void EmitMovementNoise()
        {
            float speed = _rigidbody.velocity.magnitude;
            if (speed <= 0.1f)
            {
                return;
            }

            float interval = _stealth != null && _stealth.IsCrouching ? baseInterval * 1.5f : baseInterval;
            if (Time.time - _lastNoiseTime < interval)
            {
                return;
            }

            bool isDashing = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float radius;
            if (_stealth != null && _stealth.IsCrouching)
            {
                radius = crouchNoiseRadius;
            }
            else if (isDashing)
            {
                radius = dashNoiseRadius;
            }
            else
            {
                radius = walkNoiseRadius;
            }

            NoiseEvent2D.EmitNoise(transform.position, radius, 1f, 0.75f);
            _lastNoiseTime = Time.time;
        }
    }
}
