using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
    [RequireComponent(typeof(PlayerMovementScript))]
    [RequireComponent(typeof(PlayerFlyMovement))]
    [RequireComponent(typeof(PlayerCrouchMovement))]
    [RequireComponent(typeof(FlyBooster))]
    [RequireComponent(typeof(AnimatorController))]

    public class PlayerMovementManager : MonoBehaviour 
	{
        public bool lockFlyStatus;
        public PlayerMovementScript playerMovementScript;
        public PlayerFlyMovement playerFlyMovement;
        public PlayerCrouchMovement playerCrouchMovementScript;
        public AnimatorController aniControlScript;

        void OnEnable()
        {
            initiate();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(delayFlyInputCheck());
            }

            else if (Input.GetKeyDown(KeyCode.C) && !playerFlyMovement.enabled && playerMovementScript.isGrounded)
            {
                toggleCrouchMovement();
                playerMovementScript.enabled = !playerMovementScript.enabled;
                if (playerCrouchMovementScript.enabled)
                {
                    aniControlScript.playerAnimator.SetTrigger("crouch");
                }
            }
        }

        void initiate()
        {
            playerMovementScript = GetComponent<PlayerMovementScript>();
            playerFlyMovement = GetComponent<PlayerFlyMovement>();
            playerCrouchMovementScript = GetComponent<PlayerCrouchMovement>();
            aniControlScript = GetComponent<AnimatorController>();
            playerCrouchMovementScript.isCrouching = false;
            playerFlyMovement.enabled = false;
            playerCrouchMovementScript.enabled = false;
            playerMovementScript.enabled = true;
        }

        void toggleCrouchMovement()
        {
            playerCrouchMovementScript.isCrouching = !playerCrouchMovementScript.isCrouching;
            if (!playerCrouchMovementScript.isCrouching)
            {
                playerCrouchMovementScript.resetCameraPosition();
            }
            playerCrouchMovementScript.enabled = !playerCrouchMovementScript.enabled;
        }

        IEnumerator delayFlyInputCheck()
        {
            yield return new WaitForSeconds(1);
            if (Input.GetKey(KeyCode.Space))
            {
                if(!lockFlyStatus && !playerCrouchMovementScript.isCrouching)
                {
                    playerMovementScript.enabled = false;
                    playerFlyMovement.enabled = true;
                    lockFlyStatus = true;
                    aniControlScript.playerAnimator.SetTrigger("fly");
                }
            }
        }

        public void toggleScript()
        {
            playerFlyMovement.enabled = !playerFlyMovement.enabled;
            playerMovementScript.enabled = !playerMovementScript.enabled;
        }
        
    }

}
