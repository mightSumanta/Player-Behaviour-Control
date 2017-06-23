using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class PlayerFlyMovement : MonoBehaviour 
	{

        public float flyWalkAcceleration = 10;
        public float flyMaxWalkSpeed = 20;
        public float flyRunAcceleration = 10;
        public float flyMaxRunSpeed = 50;
        public float maxElevationSpeed = 5;
        public float elevationPower = 2;
        public float touchDownOffset = 1;
        public GameObject[] RaycastOrigins;
        public LayerMask groundLayers;
        public bool isFlyWalking;
        public bool isFlyRunning;
        public bool isElevating;
        public bool isGrounded;
        public bool isBoostFuelAvailable;
        //		public float currentSpeed;

        private Vector2 horizontalMovement;
        private float verticalMovement;
        private Vector3 modifiedSpeed;
        private MouseLookManager mouselookManagerScript;
        private PlayerMovementManager playerMovementManagerScript;
        private AnimatorController aniControlScript;
        private FlyBooster boostScript;
        private Rigidbody myRigidbody;
        //private Collider myCollider;
        private float hInput;
        private float vInput;
        private bool runFlag;
        private bool jumpFlag;
        private bool isGravityOn = false;


        void OnEnable()
		{
            initiate();
            aniControlScript.isFlying = true;
            if (!isGravityOn)
            {
                GetComponent<Rigidbody>().useGravity = false;
            }
		}

		void OnDisable()
		{
            aniControlScript.isFlying = false;
            playerMovementManagerScript.lockFlyStatus = false;
            //aniControlScript.playerAnimator.SetBool("jumpDown", true);
            GetComponent<Rigidbody>().useGravity = true;
		}
	
		void FixedUpdate () 
		{
            checkForIsGrounded();
            speedCheck();

            transform.rotation = Quaternion.Euler(0, mouselookManagerScript.currentYRotation, 0);

            //checkForWalkRunToggle();
            checkForMovementInput();
            checkForJump();
            checkForFreeFall();

            if (isFlyRunning)
            {
                myRigidbody.AddRelativeForce(hInput * flyRunAcceleration, 0, vInput * flyRunAcceleration);
            }
            else if (isFlyWalking)
            {
                myRigidbody.AddRelativeForce(hInput * flyWalkAcceleration, 0, vInput * flyWalkAcceleration);
            }
        }

		void initiate()
		{
            mouselookManagerScript = GetComponent<MouseLookManager>();
            playerMovementManagerScript = GetComponent<PlayerMovementManager>();
            aniControlScript = GetComponent<AnimatorController>();
            myRigidbody = GetComponent<Rigidbody>();
            boostScript = GetComponent<FlyBooster>();
            
        }


        void checkForIsGrounded()
        {
            bool[] tempCheck;
            tempCheck = new bool[RaycastOrigins.Length];

            for (int i = 0; i < RaycastOrigins.Length; i++)
            {
                tempCheck[i] = false;
            }

            for (int i = 0; i < RaycastOrigins.Length; i++)
            {
                Debug.DrawRay(RaycastOrigins[i].transform.position, Vector3.down * touchDownOffset, Color.red);
                tempCheck[i] = Physics.Raycast(RaycastOrigins[i].transform.position, Vector3.down, touchDownOffset, groundLayers);
            }

            for (int i = 0; i < RaycastOrigins.Length; i++)
            {
                if (tempCheck[i])
                {
                    isElevating = false;
                    isGrounded = true;
                    StartCoroutine(delayToggleScript());
                }
            }

            isGrounded = false;
        }

        void checkForFreeFall()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isGravityOn = true;
                myRigidbody.useGravity = true;
            }
        }

        //void checkForWalkRunToggle()
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        runFlag = true;
        //        checkForMovementInput();
        //    }
        //    else
        //    {
        //        runFlag = false;
        //        checkForMovementInput();
        //    }
        //}

        void checkForJump()
        {
            if (Input.GetKey(KeyCode.Space) && isBoostFuelAvailable)
            {
                isElevating = true;
                myRigidbody.AddRelativeForce(0, elevationPower, 0);
                //Debug.Log("W");
            }

            if (Input.GetKeyUp(KeyCode.Space) || !isBoostFuelAvailable)
            {
                isElevating = false;
                boostScript.rechargeFlag = false;
                myRigidbody.AddRelativeForce(0, -3, 0);
            }

            else if (!Input.GetKey(KeyCode.Space))
            {
                isElevating = false;
                myRigidbody.AddRelativeForce(0, -2, 0);
                speedCheck();
            }
        }

        void checkForMovementInput()
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
     
            if (hInput == 0 && vInput == 0)
            {
                isFlyWalking = false;
            }

            //if (runFlag)
            //{
            //    if (hInput != 0 || vInput != 0)
            //    {
            //        isFlyRunning = true;
            //        isFlyWalking = false;
            //    }
            //    else
            //    {
            //        isFlyRunning = false;
            //        isFlyWalking = false;
            //    }
            //}

            if ((hInput != 0 || vInput != 0) && !runFlag)
            {
                isFlyWalking = true;
                isFlyRunning = false;
            }
        }

        void speedCheck()
        {
            horizontalMovement = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.z);
            verticalMovement = myRigidbody.velocity.y;

            if (isFlyWalking)
            {
                if (horizontalMovement.magnitude > flyMaxWalkSpeed)
                {
                    horizontalMovement.Normalize();
                    horizontalMovement *= flyMaxWalkSpeed;
                }
            }

            if (isFlyRunning)
            {
                if (horizontalMovement.magnitude > flyMaxRunSpeed)
                {
                    horizontalMovement.Normalize();
                    horizontalMovement *= flyMaxRunSpeed;
                }
            }

            if (verticalMovement > maxElevationSpeed)
            {
                verticalMovement = maxElevationSpeed;
            }

            if (verticalMovement < -maxElevationSpeed && !isGravityOn)
            {
                verticalMovement = -maxElevationSpeed;
            }

            modifiedSpeed = new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.y);
            myRigidbody.velocity = modifiedSpeed;
        }

        IEnumerator delayToggleScript()
        {
            yield return new WaitForSeconds(0.05f);
            StopAllCoroutines();
            if (!isElevating)
            {
                isGravityOn = false;
                playerMovementManagerScript.toggleScript();
            }
        }
    }

}
