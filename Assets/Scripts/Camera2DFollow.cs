using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public List<Transform> targets;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private float m_FixedY;
        private Vector3 offset;

        // Use this for initialization
        private void Start()
        {
            float screenHeightInUnits = Camera.main.orthographicSize * 2;
            float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height; // basically height * screen aspect ratio
            offset = new Vector3(screenWidthInUnits / 2 - 5, 0, 0);
            m_FixedY = transform.position.y;
            Transform target = GetMinXTarget();
            m_LastTargetPosition = target.position + offset;
            m_OffsetZ = (transform.position - target.position + offset).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            Transform target = GetMinXTarget();
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position + offset - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + offset + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = new Vector3(newPos.x, m_FixedY, newPos.z);

            m_LastTargetPosition = target.position + offset;
        }

        Transform GetMinXTarget()
        {
            Transform target = targets[0];
            foreach (Transform item in targets)
            {
                if (item.position.x < target.position.x)
                {
                    target = item;
                }
            }
            
            return target;
        }
    }
}
