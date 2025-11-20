using System.Collections;
using UnityEngine;
using SchoolPanicRoguelike.Audio;

namespace SchoolPanicRoguelike.Player
{
    public class PlayerThrowDecoy2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject decoyPrefab;

        [SerializeField]
        private float throwForce = 5f;

        [SerializeField]
        private float decoyNoiseRadius = 4f;

        [SerializeField]
        private float decoyDelay = 1.5f;

        [SerializeField]
        private int decoyCount = 1;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                TryThrowDecoy();
            }
        }

        private void TryThrowDecoy()
        {
            if (decoyCount <= 0 || decoyPrefab == null)
            {
                return;
            }

            Vector3 mouseWorld = _camera != null ? _camera.ScreenToWorldPoint(Input.mousePosition) : transform.position + transform.up;
            mouseWorld.z = 0f;
            Vector2 direction = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;
            GameObject decoy = Instantiate(decoyPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = decoy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
            }

            StartCoroutine(DecoyRoutine(decoy));
            decoyCount--;
        }

        private IEnumerator DecoyRoutine(GameObject decoy)
        {
            yield return new WaitForSeconds(decoyDelay);
            if (decoy != null)
            {
                NoiseEvent2D.EmitNoise(decoy.transform.position, decoyNoiseRadius, 1f, 0.5f);
                Destroy(decoy);
            }
        }
    }
}
