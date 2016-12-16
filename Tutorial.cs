using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
	[SerializeField] GameObject process, controlCanvas;

	void OnMouseUpAsButton ()
	{
        this.gameObject.SetActive (false);
        if(controlCanvas.activeInHierarchy)
        {
            PlayerPrefs.SetString ("Tutorial", "no");
		    process.GetComponent<GameProcess> ().bPause = false;
        }
        else
        {
            SceneManager.LoadScene(0);
        }		
	}
}
