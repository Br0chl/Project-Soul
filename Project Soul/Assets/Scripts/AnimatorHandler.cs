using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        public InputHandler inputHandler;
        public PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            // snapping the values
            #region Vertical
            float v = 0f;

            if (verticalMovement > 0f && verticalMovement < 0.55f)
            { v = 0.5f; }
            else if (verticalMovement > 0.55f)
            { v = 1f; }
            else if (verticalMovement < 0f && verticalMovement > -0.55f)
            { v = -0.5f; }
            else if (verticalMovement < -0.55f)
            { v = -1f; }
            else { v = 0f; }
            #endregion
            #region Horizontal
            float h = 0f;

            if (horizontalMovement > 0f && horizontalMovement < 0.55f)
            { h = 0.5f; }
            else if (horizontalMovement > 0.55f)
            { h = 1f; }
            else if (horizontalMovement < 0f && horizontalMovement > -0.55f)
            { h = -0.5f; }
            else if (horizontalMovement < -0.55f)
            { h = -1f; }
            else { h = 0f; }
            #endregion

            if (isSprinting)
            { 
                v = 2f;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        { canRotate = true; }

        public void StopRotation()
        { canRotate = false; }

        private void OnAnimatorMove()
        {
            if (inputHandler.isInteracting == false)
            { return; }

            float delta = Time.deltaTime;
            playerLocomotion.rb.drag = 0f;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0f;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rb.velocity = velocity;
        }
    }
}
