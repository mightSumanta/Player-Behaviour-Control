using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class FlyBooster : MonoBehaviour 
	{

        public float boosterFuel = 100;
        public float maxBoosterFuel = 100;
        public float boostSpendRate = 0.8f;
        public float checkRate = 0.3f;
        public GameObject boostUI;
        public bool rechargeFlag;

        private float nextCheck;
        private PlayerFlyMovement flyScript;
        private PlayerMovementScript movementScript;
       

		void OnEnable()
		{
            initiate();
		}

        void Update () 
		{
            checkFlyStatus();
            checkBoostStatus();
        }

		void initiate()
		{
			flyScript = GetComponent<PlayerFlyMovement>();
            movementScript = GetComponent<PlayerMovementScript>();
            flyScript.isBoostFuelAvailable = true;
            rechargeFlag = true;
		}

        void checkFlyStatus()
        {
            if (flyScript.isElevating)
            {
                boostSpend();
            }

            else if (!flyScript.isElevating || movementScript.isGrounded)
            {
                StartCoroutine(delayBoostRecharge());
                boostRecharge();
            }
        }

        void checkBoostStatus()
        {
            if (boosterFuel <= 0)
            {
                boosterFuel = 0;
                rechargeFlag = false;
                flyScript.isBoostFuelAvailable = false;
            }

            if (boosterFuel >= 5)
            {
                flyScript.isBoostFuelAvailable = true;
            }

            if (boosterFuel >= 100)
            {
                boosterFuel = 100;
                rechargeFlag = false;
            }

            boostUI.transform.localScale = new Vector3(boosterFuel / maxBoosterFuel, 
                boostUI.transform.localScale.y, boostUI.transform.localScale.z); 
        }

        void boostSpend()
        {
            boosterFuel -= boostSpendRate;
        }

        void boostRecharge()
        {
            if (rechargeFlag)
            {
                boosterFuel += boostSpendRate * 2;
            }
        }

        IEnumerator delayBoostRecharge()
        {
            yield return new WaitForSeconds(1.5f);
            rechargeFlag = true;
        }
	}

}
