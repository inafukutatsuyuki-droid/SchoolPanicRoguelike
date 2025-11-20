using UnityEngine;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.Enemies
{
    public class GuardVision2D : MonoBehaviour
    {
        [SerializeField]
        private float viewDistance = 8f;

        [SerializeField]
        private float viewAngle = 90f;

        [SerializeField]
        private LayerMask obstacleMask;

        private GuardController2D _controller;
        private PlayerStats _player;

        public void Initialize(GuardController2D controller, PlayerStats player)
        {
            _controller = controller;
            _player = player;
        }

        public void SetViewDistance(float distance)
        {
            viewDistance = Mathf.Max(0f, distance);
        }

        private void Update()
        {
            if (_controller == null || _player == null)
            {
                return;
            }

            Vector2 directionToPlayer = ((Vector2)_player.transform.position - (Vector2)transform.position);
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > viewDistance)
            {
                return;
            }

            Vector2 forward = transform.up;
            float dot = Vector2.Dot(forward.normalized, directionToPlayer.normalized);
            float angleToPlayer = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

            if (angleToPlayer > viewAngle * 0.5f)
            {
                return;
            }

            if (Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask))
            {
                _controller.OnLostSight();
                return;
            }

            float attention = 1f - (angleToPlayer / (viewAngle * 0.5f));
            _controller.OnPlayerSpotted(Mathf.Clamp01(attention));
            _controller.OnVisionMaintained(Mathf.Clamp01(attention));
        }
    }
}
