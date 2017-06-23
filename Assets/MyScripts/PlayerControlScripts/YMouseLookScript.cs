using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
    public class YMouseLookScript : MonoBehaviour
    {

		public GameObject mouseLookManagerHolder;
		private MouseLookManager mouseLookManagerScript;

        void OnEnable()
        {
			mouseLookManagerScript = mouseLookManagerHolder.GetComponent<MouseLookManager>();
        }

        void LateUpdate()
        {

            mouseLookManagerScript.currentYRotation = Mathf.SmoothDamp(mouseLookManagerScript.currentYRotation,
                mouseLookManagerScript.yRotation, ref mouseLookManagerScript.yRotationVelocity, mouseLookManagerScript.lookSmoothDampTime);

            transform.rotation = Quaternion.Euler(0, mouseLookManagerScript.currentYRotation, 0);
        }

    }

}
