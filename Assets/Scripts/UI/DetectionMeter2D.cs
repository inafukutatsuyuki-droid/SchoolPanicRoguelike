using UnityEngine;
using UnityEngine.UI;

namespace SchoolPanicRoguelike.UI
{
    public class DetectionMeter2D : MonoBehaviour
    {
        [SerializeField]
        private Slider meterSlider;

        [SerializeField]
        private float decayDelay = 2f;

        [SerializeField]
        private float decayRate = 0.25f;

        [SerializeField]
        private float buildRate = 0.5f;

        public float CurrentValue { get; private set; }

        private float _lastAttentionTime;

        private void Awake()
        {
            if (meterSlider != null)
            {
                meterSlider.minValue = 0f;
                meterSlider.maxValue = 1f;
                meterSlider.value = 0f;
            }
        }

        private void Update()
        {
            if (Time.time - _lastAttentionTime > decayDelay)
            {
                CurrentValue = Mathf.Max(0f, CurrentValue - decayRate * Time.deltaTime);
            }

            if (meterSlider != null)
            {
                meterSlider.value = CurrentValue;
            }
        }

        public void ReportAttention(float focus)
        {
            _lastAttentionTime = Time.time;
            CurrentValue = Mathf.Clamp01(CurrentValue + focus * buildRate * Time.deltaTime);
        }
    }
}
