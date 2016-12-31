using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            PlayerPrefs.SetString("Auth", success.ToString());
            SceneManager.LoadScene(1);            
        });
    }
}
