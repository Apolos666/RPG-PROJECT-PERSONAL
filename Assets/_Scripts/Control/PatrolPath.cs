using System;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float _gizmosRadius = 0.2f;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextPoint(i);
                Gizmos.DrawSphere(GetWayPoint(i), _gizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int GetNextPoint(int i)
        {
            if (i + 1 == transform.childCount)
                return 0;
            return i + 1;
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}


