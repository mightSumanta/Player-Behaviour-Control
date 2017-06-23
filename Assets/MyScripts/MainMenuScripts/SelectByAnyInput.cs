using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MainMenu
{
	public class SelectByAnyInput : MonoBehaviour 
	{
        public EventSystem mainMenuEventSystem;
        public GameObject selectedGameObject;

        private bool isButtonSelected;

        void Update()
        {
            if ((Input.GetAxisRaw("Vertical") != 0) && !isButtonSelected)
            {
                mainMenuEventSystem.SetSelectedGameObject(selectedGameObject);
                isButtonSelected = true;
            }
        }

        void OnDisable()
        {
            isButtonSelected = false; 
        }
    }

}
