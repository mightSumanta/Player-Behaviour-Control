using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class PlayerCrouchMovement : MonoBehaviour 
	{

        public float walkAcceleration = 10;
        public float maxCrouchSpeed = 10;
        public bool isCrouching = false;
        public GameObject FPSCameraObject;
        public float cameraOffsetFromGround = 0.5f;
        public Animator FPSArmsController;

        private Vector2 horizontalMovement;
        private Vector3 modifiedSpeed;
        private MouseLookManager mouselookManagerScript;
        private AnimatorController aniControlScript;
        private Rigidbody myRigidbody;
        private float hInput;
        private float vInput;
        

        void OnEnable()
        {
            initiate();
            aniControlScript.isCrouching = true;
            if (isCrouching)
            {
                setCameraPosition();
            }
        }

        void OnDisable()
        {
            aniControlScript.isCrouching = false;
        }


        void FixedUpdate()
        {
            
            checkForMovementInput();
            speedCheck();

            transform.rotation = Quaternion.Euler(0, mouselookManagerScript.currentYRotation, 0);

            if (isCrouching)
            {
                myRigidbody.AddRelativeForce(hInput * walkAcceleration, 0, vInput * walkAcceleration);
            }
        }

        void initiate()
        {
            mouselookManagerScript = GetComponent<MouseLookManager>();
            aniControlScript = GetComponent<AnimatorController>();
            myRigidbody = GetComponent<Rigidbody>();
        }

        void setCameraPosition()
        {
            FPSArmsController.SetBool("crouchEnter", true);
        }

        public void resetCameraPosition()
        {
            FPSArmsController.SetBool("crouchEnter", false);
        }

        void checkForMovementInput()
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
        }

        void speedCheck()
        {
            horizontalMovement = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.z);

            if (isCrouching)
            {
                if (horizontalMovement.magnitude > maxCrouchSpeed)
                {
                    horizontalMovement.Normalize();
                    horizontalMovement *= maxCrouchSpeed;
                }
            }

            modifiedSpeed = new Vector3(horizontalMovement.x, myRigidbody.velocity.y, horizontalMovement.y);

            myRigidbody.velocity = modifiedSpeed;
        }

    }

}
