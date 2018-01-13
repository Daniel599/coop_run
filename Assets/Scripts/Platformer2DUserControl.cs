using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public string m_PostFix;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump" + m_PostFix);
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + m_PostFix);
            if (m_Character.IsPaused() && m_Jump)
            {
                m_Character.Continue();
                m_Jump = false;
            }
            float speed = 1;
            if (h < 0)
            {
                speed = 0.5f;
            }
            speed = h;
            // Pass all parameters to the character control script.
            m_Character.Move(speed, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
