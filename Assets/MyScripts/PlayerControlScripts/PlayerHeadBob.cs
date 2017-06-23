using System;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace PlayerControl
{
    public class PlayerHeadBob : MonoBehaviour
    {
        
        public float bobbingSpeed = 0.18f;
        public float bobbingAmount = 0.2f;
        public float jumpBobbingSpeed = 0.15f;
        public float jumpBobbingAmount = 0.5f;
        public float midpoint = 2.0f;
        public bool jumpFlag;

        private float bSpeed;
        private float bAmount;
        
        private PlayerMovementScript movementScript;
        private PlayerCrouchMovement crouchScript;
        private float timer = 0.0f;
        private float jumpTimer = Mathf.PI * 2;
        private float horizontal;
        private float vertical;
        private float waveslice;
        private float walkBobbingSpeed;
        private float walkBobbingAmount;
        private float runBobbingSpeed;
        private float runBobbingAmount;
        private bool activeObjFlag;

        void OnEnable()
        {
            movementScript = transform.root.GetComponent<PlayerMovementScript>();
            crouchScript = transform.root.GetComponent<PlayerCrouchMovement>();
            walkBobbingSpeed = bobbingSpeed;
            walkBobbingAmount = bobbingAmount;
            runBobbingSpeed = bobbingSpeed * 2;
            runBobbingAmount = bobbingAmount * 2;
            saveInitials();
        }

        void OnDisable()
        {
            bobbingSpeed = bSpeed;
            bobbingAmount = bAmount;
        }

        void Update()
        {
            checkForWalkRunJump();
            waveslice = 0.0f;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else if (!jumpFlag)
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }

            if (!crouchScript.isCrouching)
            {
                if (!jumpFlag)
                {
                    headBob();
                }
                else
                {
                    jumpHeadBob();
                }
            }
        }

        void saveInitials()
        {
            bSpeed = bobbingSpeed;
            bAmount = bobbingAmount;
        }

        void checkForWalkRunJump()
        {
            if (movementScript.isWalking)
            {
                bobbingSpeed = walkBobbingSpeed;
                bobbingAmount = walkBobbingAmount;
            }
            else if (movementScript.isRunning)
            {
                bobbingAmount = runBobbingAmount;
                bobbingSpeed = runBobbingSpeed;
            }

            if (movementScript.isJumping)
            {
                jumpFlag = true;
            }
        }

        void headBob()
        {
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingAmount;
                if (!jumpFlag)
                {
                    float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                    totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                    translateChange = totalAxes * translateChange;
                }
                transform.localPosition = new Vector3(transform.localPosition.x, midpoint + translateChange, transform.localPosition.z);
                //Debug.Log(transform.localPosition);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, midpoint, transform.localPosition.z);
            }
        }

        void jumpHeadBob()
        {
            if (movementScript.isGrounded)
            {
                waveslice = Mathf.Sin(jumpTimer);
                jumpTimer = jumpTimer - jumpBobbingSpeed;
                if (jumpTimer < Mathf.PI) 
                {
                    jumpTimer = Mathf.PI * 2;
                    jumpFlag = false;
                }

                bobbingAmount = jumpBobbingAmount;
                headBob();
            }
        }
    }
}
