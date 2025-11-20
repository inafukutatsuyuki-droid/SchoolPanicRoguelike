using UnityEngine;
using SchoolPanicRoguelike.Audio;

namespace SchoolPanicRoguelike.Player
{
    [RequireComponent(typeof(PlayerController2D))]
    public class PlayerStealthController2D : MonoBehaviour
    {
        [SerializeField]
        private KeyCode crouchKey = KeyCode.LeftControl;

        [SerializeField]
        private float crouchSpeedMultiplier = 0.6f;

        [SerializeField]
        private float crouchNoiseRadius = 1.2f;

        [SerializeField]
        private float standNoiseRadius = 2.5f;

        public bool IsCrouching { get; private set; }

        private PlayerController2D _controller;
        private float _lastNoiseTime;
        private float _noiseInterval = 1f;

        private void Awake()
        {
            _controller = GetComponent<PlayerController2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(crouchKey))
            {
                ToggleCrouch();
            }

            EmitStealthNoise();
        }

        private void ToggleCrouch()
        {
            IsCrouching = !IsCrouching;
            _controller.ExternalSpeedMultiplier = IsCrouching ? crouchSpeedMultiplier : 1f;
            _noiseInterval = IsCrouching ? 1.25f : 0.75f;
        }

        private void EmitStealthNoise()
        {
            if (Time.time - _lastNoiseTime < _noiseInterval)
            {
                return;
            }

            float radius = IsCrouching ? crouchNoiseRadius : standNoiseRadius;
            NoiseEvent2D.EmitNoise(transform.position, radius, 0.5f, 0.5f);
            _lastNoiseTime = Time.time;
        }
    }
}
