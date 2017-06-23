using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
    public class AnimatorController : MonoBehaviour
    {

        public bool isStanding;
        public bool isIdle;
        public bool isWalking;
        public bool isRunning;
        public bool isJumping;
        public bool isCrouching;
        public bool isFlying;
        public Animator playerAnimator;

        private float hInput;
        private float vInput;
        private float mhInput;
        private bool ignoreFlag;
        //private string prevTrigger;
        //private string currentTrigger;

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        void Start()
        {

        }

        void Update()
        {
            //if (prevTrigger != currentTrigger)
            //{
            //    playerAnimator.ResetTrigger(currentTrigger);
            //    playerAnimator.SetTrigger("standIdle");
            //}
            resetTriggers();
            checkForDirectionInput();
            controlAnimation();
        }

        void initiate()
        {

        }

        void checkForDirectionInput()
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
            mhInput = Input.GetAxis("Mouse X");
        }

        void controlAnimation()
        {
            if (isStanding)
            {
                //resetTriggers();
                isCrouching = false;
                isFlying = false;

                walkRunJumpAnimations();
            }

            if (isCrouching)
            {
                //resetTriggers();
                isStanding = false;
                isFlying = false;

                if (hInput > 0)
                {
                    playerAnimator.SetTrigger("crouchRight");
                }

                else if (hInput < 0)
                {
                    playerAnimator.SetTrigger("crouchLeft");
                }

                else if (vInput > 0)
                {
                    playerAnimator.SetTrigger("crouchForward");
                }

                else if (vInput < 0)
                {
                    playerAnimator.SetTrigger("crouchBackward");
                }

                else
                {
                    if (mhInput > 0.1f)
                    {

                        playerAnimator.SetTrigger("crouchRight");
                    }

                    else if (mhInput < -0.1f)
                    {
                        playerAnimator.SetTrigger("crouchLeft");
                    }

                    else
                    {
                        playerAnimator.SetTrigger("crouch");
                    }
                }

            }

            if (isFlying)
            {
                //resetTriggers();
                isStanding = false;
                isCrouching = false;

                if (hInput > 0)
                {
                    playerAnimator.SetTrigger("flyRight");
                }

                else if (hInput < 0)
                {
                    playerAnimator.SetTrigger("flyLeft");
                }

                else if (vInput > 0)
                {
                    playerAnimator.SetTrigger("flyForward");
                }

                else if (vInput < 0)
                {
                    playerAnimator.SetTrigger("flyBackward");
                }

                else
                {
                    if (mhInput > 0.1f)
                    {

                        playerAnimator.SetTrigger("flyRight");
                    }

                    else if (mhInput < -0.1f)
                    {
                        playerAnimator.SetTrigger("flyLeft");
                    }

                    else
                    {
                        playerAnimator.SetTrigger("fly");
                    }
                }
            }
        }

        void walkRunJumpAnimations()
        {
            if (isWalking && !isJumping)
            {
                if (hInput > 0)
                {
                    playerAnimator.SetTrigger("walkRight");
                }

                else if (hInput < 0)
                {
                    playerAnimator.SetTrigger("walkLeft");
                }

                else if (vInput > 0)
                {
                    playerAnimator.SetTrigger("walkForward");
                }

                else if (vInput < 0)
                {
                    playerAnimator.SetTrigger("walkBackward");
                }

            }

            if (isRunning && !isJumping)
            {
                playerAnimator.SetTrigger("run");
            }

            if (isJumping)
            {
                //playerAnimator.SetTrigger("standIdle");
                playerAnimator.SetTrigger("jump");
            }


            if (isIdle && !playerAnimator.GetBool("jumpDown"))
            {
                if (hInput == 0 && vInput == 0)
                {
                    if (mhInput > 0.1f)
                    {

                        playerAnimator.SetTrigger("walkRight");
                    }

                    else if (mhInput < -0.1f)
                    {
                        playerAnimator.SetTrigger("walkLeft");
                    }

                    else
                    {
                        playerAnimator.SetTrigger("standIdle");
                    }
                }


            }
        }

        void resetTriggers()
        {
            playerAnimator.ResetTrigger("standIdle");
            playerAnimator.ResetTrigger("walkLeft");
            playerAnimator.ResetTrigger("walkRight");
            playerAnimator.ResetTrigger("walkForward");
            playerAnimator.ResetTrigger("walkBackward");
            playerAnimator.ResetTrigger("crouch");
            playerAnimator.ResetTrigger("crouchLeft");
            playerAnimator.ResetTrigger("crouchRight");
            playerAnimator.ResetTrigger("crouchForward");
            playerAnimator.ResetTrigger("crouchBackward");
            playerAnimator.ResetTrigger("fly");
            playerAnimator.ResetTrigger("flyLeft");
            playerAnimator.ResetTrigger("flyRight");
            playerAnimator.ResetTrigger("flyForward");
            playerAnimator.ResetTrigger("flyBackward");
            playerAnimator.ResetTrigger("run");
            playerAnimator.ResetTrigger("jump");
        }

        public void resetJumpDownTrigger()
        {
            resetTriggers();
            StartCoroutine(resetJumpTrigger());
        }

        IEnumerator resetJumpTrigger()
        {
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("jumpDown", false);
        }

    }

}
