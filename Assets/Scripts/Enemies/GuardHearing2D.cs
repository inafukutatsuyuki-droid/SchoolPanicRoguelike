using System.Collections;
using UnityEngine;
using SchoolPanicRoguelike.Audio;

namespace SchoolPanicRoguelike.Enemies
{
    public class GuardHearing2D : MonoBehaviour
    {
        [SerializeField]
        private float checkInterval = 0.5f;

        [SerializeField]
        private float hearingRange = 6f;

        private GuardController2D _controller;
        private Coroutine _hearingRoutine;

        public void Initialize(GuardController2D controller)
        {
            _controller = controller;
            if (_hearingRoutine != null)
            {
                StopCoroutine(_hearingRoutine);
            }

            _hearingRoutine = StartCoroutine(HearingLoop());
        }

        public void SetHearingRange(float range)
        {
            hearingRange = Mathf.Max(0f, range);
        }

        private IEnumerator HearingLoop()
        {
            while (true)
            {
                foreach (NoiseEvent2D noise in NoiseEvent2D.GetActiveEvents())
                {
                    float distance = Vector2.Distance(transform.position, noise.Position);
                    float effectiveRange = Mathf.Min(noise.Radius, hearingRange);
                    if (distance <= effectiveRange)
                    {
                        _controller?.OnHeardNoise(noise.Position);
                        break;
                    }
                }

                yield return new WaitForSeconds(checkInterval);
            }
        }
    }
}
