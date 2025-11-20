using UnityEngine;
using SchoolPanicRoguelike.Player;
using SchoolPanicRoguelike.UI;

namespace SchoolPanicRoguelike.Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ExtractionPoint2D : MonoBehaviour
    {
        [SerializeField]
        private bool requireUndetected = true;

        [SerializeField]
        private string resultControllerTag = "ResultUI";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentScenario != ScenarioType.TrainingGoneWrong)
            {
                return;
            }

            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player == null)
            {
                return;
            }

            if (requireUndetected)
            {
                DetectionMeter2D meter = FindObjectOfType<DetectionMeter2D>();
                if (meter != null && meter.CurrentValue > 0.2f)
                {
                    return;
                }
            }

            GameManager.Instance.HandleGameClear();
            ResultController2D result = FindResultController();
            if (result != null)
            {
                result.gameObject.SetActive(true);
            }
        }

        private ResultController2D FindResultController()
        {
            if (!string.IsNullOrEmpty(resultControllerTag))
            {
                GameObject tagged = GameObject.FindGameObjectWithTag(resultControllerTag);
                if (tagged != null)
                {
                    return tagged.GetComponent<ResultController2D>();
                }
            }

            return FindObjectOfType<ResultController2D>();
        }
    }
}
