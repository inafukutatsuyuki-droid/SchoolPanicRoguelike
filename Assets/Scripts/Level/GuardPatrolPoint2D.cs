using UnityEngine;

namespace SchoolPanicRoguelike.Level
{
    public class GuardPatrolPoint2D : MonoBehaviour
    {
        [SerializeField]
        private Transform[] patrolPoints;

        public Transform[] GetPatrolPoints()
        {
            return patrolPoints;
        }
    }
}
