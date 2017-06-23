using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MainMenu
{
	public class LoadGameScene : MonoBehaviour 
	{
		
        public void loadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
	}

}
