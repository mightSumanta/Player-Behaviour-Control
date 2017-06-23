using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class CameraToggle : MonoBehaviour 
	{
        public GameObject fpsCamera;
        public GameObject weaponCamera;
        public GameObject tpsCamera;

        void OnEnable()
        {
            fpsCamera.SetActive(true);
            weaponCamera.SetActive(true);
            tpsCamera.SetActive(false);
        }

        void Update () 
		{
            if (Input.GetKeyDown(KeyCode.V))
            {
                fpsCamera.SetActive(!fpsCamera.activeSelf);
                weaponCamera.SetActive(!weaponCamera.activeSelf);
                tpsCamera.SetActive(!tpsCamera.activeSelf);
            }
		}

	}

}
