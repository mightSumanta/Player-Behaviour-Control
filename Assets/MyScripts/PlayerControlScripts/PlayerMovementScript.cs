using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl  
{
	public class PlayerMovementScript : MonoBehaviour 
	{

		public float walkAcceleration = 10;
		public float maxWalkSpeed = 20;
		public float runAcceleration = 10;
		public float maxRunSpeed = 50;
        public float jumpPower = 2;
        public GameObject[] RaycastOrigins;
		public LayerMask groundLayers;
        public float stepOffset;
        public float airTime = 5;
		public bool isWalking;
		public bool isRunning;
        public bool isJumping;
		public bool isGrounded;
//		public float currentSpeed;

		private Vector2 horizontalMovement;
		private Vector3 modifiedSpeed;
		private MouseLookManager mouselookManagerScript;
        private PlayerMovementManager playerMovementManagerScript;
        private AnimatorController aniControlScript;
		private Rigidbody myRigidbody;
        private float hInput;
		private float vInput;
		private bool runFlag;
        private bool jumpFlag;

		void OnEnable()
		{
			initiate ();
            aniControlScript.isStanding = true;
            aniControlScript.resetJumpDownTrigger();
        }

        void OnDisable()
        {
            aniControlScript.isStanding = false;
            resetBools();
        }

        void FixedUpdate () 
		{

			checkForIsGrounded ();
			speedCheck ();
            updateAnimator();

			transform.rotation = Quaternion.Euler (0, mouselookManagerScript.currentYRotation, 0);

            if (isGrounded)
            {
                checkForWalkRunToggle();
                checkForJump();
            }

            if (isRunning && isGrounded)
            {
                if (vInput < 0 || hInput != 0)
                {
                    myRigidbody.AddRelativeForce(hInput * walkAcceleration, 0, vInput * walkAcceleration);
                    isRunning = false;
                    isWalking = true;
                }
                else
                {
                    myRigidbody.AddRelativeForce(hInput * runAcceleration, 0, vInput * runAcceleration);
                }
            }
            else if (isWalking && isGrounded)
            {
                myRigidbody.AddRelativeForce(hInput * walkAcceleration, 0, vInput * walkAcceleration);
            }
		}
			
		void initiate()
		{
			mouselookManagerScript = GetComponent<MouseLookManager> ();
            playerMovementManagerScript = GetComponent<PlayerMovementManager>();
            aniControlScript = GetComponent<AnimatorController>();
			myRigidbody = GetComponent<Rigidbody> ();
			//myCollider = GetComponent<Collider> ();
		}

        void resetBools()
        {
            isWalking = false;
            isRunning = false;
            isJumping = false;
        }

        void updateAnimator()
        {
            aniControlScript.isIdle = (!isWalking && !isRunning && isGrounded);
            aniControlScript.isWalking = isWalking;
            aniControlScript.isRunning = isRunning;
            aniControlScript.isJumping = isJumping;
        }

        void checkForIsGrounded()
		{
			bool[] tempCheck;
			tempCheck = new bool[RaycastOrigins.Length];

			for (int i = 0; i < RaycastOrigins.Length; i++) 
			{
				tempCheck [i] = false;
			}

			for (int i = 0; i < RaycastOrigins.Length; i++) 
			{
				Debug.DrawRay (RaycastOrigins [i].transform.position, Vector3.down * stepOffset, Color.red);
				tempCheck [i] = Physics.Raycast (RaycastOrigins [i].transform.position, Vector3.down, stepOffset, groundLayers);
			}

			for (int i = 0; i < RaycastOrigins.Length; i++) 
			{
				if (tempCheck [i]) 
				{
                    if (!jumpFlag) 
                    {
                        updateAnimator();
                        StopAllCoroutines();
                        if (isJumping)
                        {
                            aniControlScript.playerAnimator.SetBool("jumpDown", true);
                            aniControlScript.resetJumpDownTrigger();
                        }
                        isJumping = false;
                        isGrounded = true;
                        return;
                    }
				}
			}

			//myRigidbody.AddRelativeForce (Vector3.zero);
			if (myRigidbody.velocity.y > 0 && !isJumping) 
			{
				myRigidbody.velocity = Vector3.zero;
			}

            if (!isJumping)
            {
                myRigidbody.velocity = new Vector3(0, myRigidbody.velocity.y, 0);
            }

            StartCoroutine(calculateAirTime());
			isGrounded = false;
		}

		void checkForWalkRunToggle()
		{
			if (Input.GetKey (KeyCode.LeftShift)) 
			{
				runFlag = true;
				checkForMovementInput ();
			}
			else 
			{
				runFlag = false;
				checkForMovementInput ();
			}
		}

        void checkForJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isWalking = false;
                isRunning = false;
                isGrounded = false;
                updateAnimator();
                isJumping = true;
                jumpFlag = true;
                aniControlScript.playerAnimator.SetTrigger("jumpUp");
                jump();
            }
        }

        void jump()
        {
            checkForMovementInput();
            float speed = myRigidbody.velocity.sqrMagnitude;
            if (speed <= 2)
            {
                myRigidbody.AddRelativeForce(hInput * speed, jumpPower, vInput * speed);
            }

            else
            {
                myRigidbody.AddRelativeForce(hInput * speed, jumpPower * 2 / maxWalkSpeed, vInput * speed);
            }
            StartCoroutine(jumpBoolReset());
        }

		void checkForMovementInput()
		{
			hInput = Input.GetAxis("Horizontal");
			vInput = Input.GetAxis ("Vertical");
//			Debug.Log (hInput);
//			Debug.Log (vInput);

			if (hInput == 0 && vInput == 0) 
			{
				isWalking = false;

				if (isGrounded) 
				{
					myRigidbody.velocity = Vector3.zero;
				}
			} 

			if (runFlag && isGrounded) 
			{
				if (hInput != 0 || vInput != 0) 
				{
					isRunning = true;
					isWalking = false;
				} 
				else 
				{
					isRunning = false;
					isWalking = false;
				}
			}

			if ((hInput != 0 || vInput != 0) && !runFlag && isGrounded)
			{
				isWalking = true;
				isRunning = false;
			}
		}

		void speedCheck()
		{
			horizontalMovement = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.z);

			if (isWalking) 
			{
				if (horizontalMovement.magnitude > maxWalkSpeed) 
				{	
					horizontalMovement.Normalize();
					horizontalMovement *= maxWalkSpeed;
				}
			}

			if (isRunning) 
			{
				if (horizontalMovement.magnitude > maxRunSpeed) 
				{	
					horizontalMovement.Normalize();
					horizontalMovement *= maxRunSpeed;
				}
			}

			modifiedSpeed = new Vector3 (horizontalMovement.x, myRigidbody.velocity.y, horizontalMovement.y);

			myRigidbody.velocity = modifiedSpeed;
		}

        IEnumerator jumpBoolReset()
        {
            yield return new WaitForSeconds(0.4f);
            jumpFlag = false;
        }

        IEnumerator calculateAirTime()
        {
            yield return new WaitForSeconds(airTime);
            StopAllCoroutines();
            if (!isGrounded)
            {
                //playerMovementManagerScript.isGravityOn = true;
                playerMovementManagerScript.toggleScript();
            }
        }

    }

}
