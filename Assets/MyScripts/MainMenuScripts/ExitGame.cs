using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainMenu
{
	public class ExitGame : MonoBehaviour 
	{
		
		public void exitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }
	}

}
