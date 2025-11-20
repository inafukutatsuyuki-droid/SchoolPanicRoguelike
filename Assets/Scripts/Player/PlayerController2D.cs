using UnityEngine;

namespace SchoolPanicRoguelike.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private float dashMultiplier = 1.5f;

        [SerializeField]
        private float staminaCostPerDash = 5f;

        private Rigidbody2D _rigidbody;
        private Vector2 _movementInput;
        private Vector2 _lastMoveDirection = Vector2.down;
        private PlayerStats _playerStats;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerStats = GetComponent<PlayerStats>();
        }

        private void Update()
        {
            ReadInput();
            UpdateOrientation();
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void ReadInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            _movementInput = new Vector2(x, y).normalized;

            if (_movementInput.sqrMagnitude > 0.01f)
            {
                _lastMoveDirection = _movementInput;
            }
        }

        private void UpdateOrientation()
        {
            if (_lastMoveDirection.sqrMagnitude > 0.01f)
            {
                transform.up = _lastMoveDirection;
            }
        }

        private void MoveCharacter()
        {
            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (_playerStats == null || _playerStats.TryConsumeStamina(staminaCostPerDash * Time.fixedDeltaTime))
                {
                    speed *= dashMultiplier;
                }
            }

            _rigidbody.velocity = _movementInput * speed;
        }
    }
}
