using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Buttons : MonoBehaviour
{
    [SerializeField] private Text timerText;
	[SerializeField] private GameProcess gp;
	[SerializeField] public Sprite pressed;
	[SerializeField] public Sprite normal;
    [SerializeField] private PlayerChooser chooser;
    [SerializeField] private GameObject tutorial;
    [SerializeField]
    private GameObject[] canvases;

    private void Start()
    {
        switch (name)
        {
            case "Sounds":
                this.gameObject.GetComponentInChildren<Text>().text = (PlayerPrefs.GetString("Sounds") != "false") ? "ON" : "OFF";
                break;
            case "Music":
                this.gameObject.GetComponentInChildren<Text>().text = (PlayerPrefs.GetString("Music") != "false") ? "ON" : "OFF";
                break;
            default:
                break;
        }        
    }

	void OnMouseDown ()
	{
		if (pressed != null)
			this.GetComponent<Image> ().sprite = pressed;
        if ((name != "price_2") && (name != "price_2"))
            gp.PlayAudio("ButtonPress");
        else
            gp.PlayAudio("Unlock");
	}

	void OnMouseUp ()
	{	
		if (normal != null)
			this.GetComponent<Image> ().sprite = normal;
	}

	void OnMouseUpAsButton ()
	{
		switch (name)
		{
		    case "Play":
			    gp.BeginGame ();
			    break;
		    case "Restart":
			    gp.BeginGame ();
			    break;
		    case "Settings":
			    gp.OpenSettings (); 
			    break;
		    case "Rate":
			    Application.OpenURL ("https://play.google.com/store/apps/details?id=com.luzaludum.AstronautRun&hl=ru");	
			    break;
		    case "Music":
                if (PlayerPrefs.GetString("Music") != "false")
                    PlayerPrefs.SetString("Music", "false");
                else
                    PlayerPrefs.SetString("Music", "true");
                this.gameObject.GetComponentInChildren<Text>().text = (PlayerPrefs.GetString("Music") != "false") ? "ON" : "OFF";
                break;
		    case "Back":
			    gp.LoadMenu (); 
			    break;
		    case "Sound":
                if (PlayerPrefs.GetString("Sounds") != "false")
                    PlayerPrefs.SetString("Sounds", "false");
                else
                    PlayerPrefs.SetString("Sounds", "true");
			    this.gameObject.GetComponentInChildren<Text> ().text = (PlayerPrefs.GetString("Sounds") != "false") ? "ON" : "OFF";
			    break;
            case "Next":
                chooser.NextSkin();
                break;
            case "Previous":
                chooser.PreviousSkin();
                break;
            case "Accept":
                chooser.ChooseSkin();
                break;
            case "BackScene":
                SceneManager.LoadScene(0);
                break;
            case "Skins":
                SceneManager.LoadScene(1);
                break;
            case "HowTo":
                tutorial.SetActive(true);
                foreach(GameObject canvas in canvases)
                {
                    canvas.SetActive(false);
                }
                break;
            case "price_2":
                if (PlayerPrefs.GetInt("Money") >= 400)
                {
                    PlayerPrefs.SetString("UnlockedSkins", PlayerPrefs.GetString("UnlockedSkins") + " 3");
                    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - 400);
                    Social.ReportProgress("CgkIt8SX4LQYEAIQDg", 100, (bool success) => { });
                }
                break;
            case "price_1":
                if (PlayerPrefs.GetInt("Money") >= 300)
                {
                    PlayerPrefs.SetString("UnlockedSkins", PlayerPrefs.GetString("UnlockedSkins") + " 2");
                    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - 300);
                    Social.ReportProgress("CgkIt8SX4LQYEAIQDg", 100, (bool success) => { });
                }
                break;
            case "Highscore":
                PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIt8SX4LQYEAIQAA");
                break;
            case "Achievements":
                PlayGamesPlatform.Instance.ShowAchievementsUI();
                break;
        }

		if (normal != null)
			this.GetComponentInChildren<Image> ().sprite = normal;
	}    
}
