using UnityEngine;

namespace SchoolPanicRoguelike.Level
{
    public enum SpawnCategory
    {
        Player,
        Zombie,
        Item
    }

    public class SpawnPoint2D : MonoBehaviour
    {
        [SerializeField]
        private SpawnCategory spawnType;

        public SpawnCategory SpawnType => spawnType;
    }
}
