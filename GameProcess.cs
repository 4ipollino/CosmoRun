using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GoogleMobileAds.Api;
using System;

public class GameProcess : MonoBehaviour
{
    [SerializeField] public int giftGems = 10;
    [SerializeField] int numberOfPlanets = 5;
	[SerializeField] public int smooth = 1;
	[SerializeField] Vector3 offset;
	[SerializeField] int prevPlanets = 2;
    [SerializeField] GameObject[] skins;
    [SerializeField]
    AudioClip death, pickup, teleport, buttonPress, unlock;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    bool Shop;

	//[SerializeField] GameObject bgAsset;
	//private GameObject[] backgroundTiles;

	private Animator anim;
	public bool bPause = true;
	private GameObject controlCanvas, gameOverCanvas, mainMenuCanvas, settingsCanvas, scoreObj, moneyObj;
	private GameObject goRecord, goScore;
	private GameObject playerObj, planetAsset, tutorial;
	//private Player playerScript;
	private int score = 0;
	private int money = 0;
	private int record = 0;
	private GameObject[] planets;
	private int planetCounter = 0;
	private int upOrDown = 1;
	private AudioSource musicSource;
    private static int runIterator = 0;
    private const string AdsID = "ca-app-pub-7736740257809301/9713700477";
    private bool showingAds = false;
    private InterstitialAd newAd;

    //public bool music = true;
    //public bool sounds = true;

    // Use this for initialization
    void Start ()
	{
		controlCanvas = GameObject.Find ("ControlCanvas");
        if (controlCanvas != null)
		    controlCanvas.SetActive (false);
		gameOverCanvas = GameObject.Find ("GameOverCanvas");
        if(gameOverCanvas != null)
		    gameOverCanvas.SetActive (false);
		settingsCanvas = GameObject.Find ("SettingsMenu");
        if(settingsCanvas != null)
		    settingsCanvas.SetActive (false);
		mainMenuCanvas = GameObject.Find ("MainMenu");
        if(mainMenuCanvas != null)
		    mainMenuCanvas.SetActive (true);
		planetAsset = Resources.Load<GameObject> ("Planet");
		//playerAsset = Resources.Load<GameObject> ("PlayerCharacter");
		planets = new GameObject[numberOfPlanets];
		record = PlayerPrefs.GetInt ("Record");
		musicSource = Camera.main.GetComponent<AudioSource> ();
		tutorial = GameObject.Find ("Tutorial");
        if(tutorial != null)
		    tutorial.SetActive (false);

        if(PlayerPrefs.GetString("FirstLaunch") != "firstLaunchSuccess")
        {
            PlayerPrefs.SetInt("SkinNumber", 1);
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.SetString("FirstLaunch", "firstLaunchSuccess");
            PlayerPrefs.SetString("UnlockedSkins", "1");          
        }        

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {

        });

        /*backgroundTiles = new GameObject[5];
		Vector3 position = new Vector3 (0, 0, 10);

		for (int i = 0; i < 5; i++) {

			backgroundTiles[i] = Instantiate (bgAsset, position, Quaternion.identity, this.gameObject.transform) as GameObject;
			position = new Vector3 (16*i, 0, 10);
		}*/
    }

	//создаем новую планету и уничтожаем предыдущую
	public void CreatePlanet ()
	{		
		if ((planets [0] != null) && (planetCounter > prevPlanets - 1))
			Destroy(planets [0].GetComponent<Planet> ().gameObject);		
		MoveQueue ();
		MovePlanets ();
		planets [numberOfPlanets - 1] = Instantiate (planetAsset) as GameObject;
		planets [numberOfPlanets - 1].transform.localScale = new Vector3 (0.45f, 0.45f);
		planets [numberOfPlanets - 1].transform.position = new Vector3 (planets [numberOfPlanets - 2].GetComponent<Planet> ().coords.x - offset.x, upOrDown * UnityEngine.Random.Range (2.0f, 4.0f), 10);
		planets [numberOfPlanets - 1].GetComponent<Planet> ().coords = planets [numberOfPlanets - 1].transform.position;
		planets [numberOfPlanets - 1].GetComponent<Planet> ().CreatePlanetObjects (false);
		Quaternion angle = planets [numberOfPlanets - 1].GetComponentInChildren<SpriteRenderer> ().transform.rotation;
		angle.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
		planets [numberOfPlanets - 1].GetComponentInChildren<SpriteRenderer> ().transform.rotation = angle;
		GameObject teleportPrev = planets [numberOfPlanets - 2].GetComponent<Planet> ().GetTeleport ();
		teleportPrev.GetComponent<Teleport> ().SetTarget (planets [numberOfPlanets - 1], playerObj, this);
		planetCounter++;
		upOrDown *= -1;
    }

    private void onAdLoaded(object sender, EventArgs e)
    {
        newAd.Show();
    }

    //начальные планеты
    void CreateStartUpPlanets ()
	{
		for (int i = 0; i < (numberOfPlanets - prevPlanets); i++)
		{
			planets [numberOfPlanets - 1] = Instantiate (planetAsset) as GameObject;
			planets [numberOfPlanets - 1].transform.localScale = new Vector3 (0.45f, 0.45f);

			//planets [numberOfPlanets - 1].GetComponent<Planet> ().Start ();
			if (i == 0)
			{
				planets [numberOfPlanets - 1].transform.position = new Vector3 (-2, 0, 10);
				planets [numberOfPlanets - 1].GetComponent<Planet> ().coords = planets [numberOfPlanets - 1].transform.position;
				planets [numberOfPlanets - 1].GetComponent<Planet> ().CreatePlanetObjects (true);
				playerObj = Instantiate (skins[PlayerPrefs.GetInt("SkinNumber")-1]) as GameObject;
				playerObj.transform.position = planets [numberOfPlanets - 1].transform.position;
				playerObj.GetComponent<Player> ().SetTarget (planets [numberOfPlanets - 1]);
				anim = playerObj.GetComponentInChildren<Animator> ();
				//planets [numberOfPlanets - 1].GetComponent<Planet> ().StartMoving ();
			} else
			{		
				planets [numberOfPlanets - 1].transform.position = new Vector3 (planets [numberOfPlanets - 2].GetComponent<Planet> ().coords.x - offset.x, upOrDown * UnityEngine.Random.Range (2.0f, 4.0f), 10);
				planets [numberOfPlanets - 1].GetComponent<Planet> ().coords = planets [numberOfPlanets - 1].transform.position;
				planets [numberOfPlanets - 1].GetComponent<Planet> ().CreatePlanetObjects (false);
				Quaternion angle = planets [numberOfPlanets - 1].GetComponentInChildren<SpriteRenderer> ().transform.rotation;
				angle.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
				planets [numberOfPlanets - 1].GetComponentInChildren<SpriteRenderer> ().transform.rotation = angle;
				GameObject teleportPrev = planets [numberOfPlanets - 2].GetComponent<Planet> ().GetTeleport ();
				teleportPrev.GetComponent<Teleport> ().SetTarget (planets [numberOfPlanets - 1], playerObj, this);
				upOrDown *= -1;
			}
			if ((i != (numberOfPlanets - prevPlanets - 1)))
				MoveQueue ();
		}
	}

	//оставлю на всякий случай) для объема скрипта xD

	/*public void CreatePlanet(float x, float y)
	{
		GameObject planetAsset = Resources.Load<GameObject> ("Planet");			
		planet = Instantiate (planetAsset) as GameObject;
		planet.transform.position = new Vector3 (x, y, 10);
		planet.GetComponent<Planet> ().StartupPlanet = true;
		//planet.GetComponent<Planet> ().CreatePlanetObjects ();
		//playerObj.GetComponent<Player> ().SetTarget (planet);
		if (prevPlanet != null)
			prevPlanet.GetComponent<Planet> ().DestroyPlanet ();
		prevPlanet = planet;
	}*/

	//функция для добавления очков
	public void IncreaseScore (int amount)
	{
		score += amount;
    }

	public void IncreaseMoney (int amount)
	{
		money += amount;
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQCQ", 1, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQDA", 1, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQDQ", 1, (bool success) => { });
    }

	/* выполняется при завершении забега (проигрыше) 
	 * 
	 * P.S. нужно будет сюда вставить запись очков и игровой валюты */
	public void GameOver ()
	{
        runIterator++;

        if((runIterator % 5 == 0)&&(PlayerPrefs.GetString("Ads") != "disabled"))
        {
            showingAds = true;
            newAd = new InterstitialAd(AdsID);            
            AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("37513B0AC9A42628").Build();
            newAd.LoadAd(request);
            newAd.OnAdClosed += onAdClosed;
            newAd.OnAdLoaded += onAdLoaded;
        }            

        musicSource.Stop();
		controlCanvas.SetActive (false);
		mainMenuCanvas.SetActive (false);
		gameOverCanvas.SetActive (true);
		bPause = true;
		anim.SetFloat ("Speed", 0);
		goScore = GameObject.Find ("Score");
		goRecord = GameObject.Find ("Record");
		if (score > record)
		{
			record = score;
			PlayerPrefs.SetInt ("Record", record);
            PlayGamesPlatform.Instance.ReportScore(record, "CgkIt8SX4LQYEAIQAA", (bool success) => { });
		}
		goScore.GetComponent<Text> ().text = "SCORE: " + score;
		goRecord.GetComponent<Text> ().text = "HIGHSCORE: " + record;
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + money);
        Social.ReportProgress("CgkIt8SX4LQYEAIQAQ", 100, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQEg", 1, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQEw", 1, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQFA", 1, (bool success) => { });
        PlayGamesPlatform.Instance.IncrementAchievement("CgkIt8SX4LQYEAIQFQ", 1, (bool success) => { });
        Social.ReportProgress("CgkIt8SX4LQYEAIQBQ", 100 / 5 * record, (bool success) => { });
        Social.ReportProgress("CgkIt8SX4LQYEAIQCg", 100 / 15 * record, (bool success) => { });
        Social.ReportProgress("CgkIt8SX4LQYEAIQCw", 100 / 25 * record, (bool success) => { });
        Social.ReportProgress("CgkIt8SX4LQYEAIQDw", 100 / 50 * record, (bool success) => { });
        Social.ReportProgress("CgkIt8SX4LQYEAIQEA", record, (bool success) => { });
    }

    public void BeginGame ()
	{
        if(!showingAds)
		    StartCoroutine ("beginGame");
	}

    private void onAdClosed(object Sender, EventArgs e)
    {
        showingAds = false;
    }

	IEnumerator beginGame ()
	{		
		Clear ();
		planetCounter = 0;
		mainMenuCanvas.SetActive (false);
		controlCanvas.SetActive (true);
		gameOverCanvas.SetActive (false);
		settingsCanvas.SetActive (false);
		CreateStartUpPlanets ();        
		bPause = false;
        ShowTutorial ();
		anim.SetFloat ("Speed", 1);
		if (PlayerPrefs.GetString("Music") != "false")
			musicSource.Play ();
		yield return new WaitForSeconds (0);		
	}

	public void ShowTutorial ()
	{
		string firstPlay = PlayerPrefs.GetString ("Tutorial", "yes");
		if (firstPlay != "no")
		{
            Pause();
			tutorial.SetActive (true);			
		}			
	}

	//чистим все созданные объекты
	private void Clear ()
	{
		MoneyPoint[] money = GameObject.FindObjectsOfType<MoneyPoint> ();
		Planet[] planets = GameObject.FindObjectsOfType<Planet> ();
		Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle> ();
		Teleport[] teleports = GameObject.FindObjectsOfType<Teleport> ();
		TeleportTarget[] targets = GameObject.FindObjectsOfType<TeleportTarget> ();

		foreach (Teleport teleport in teleports)
		{
			Destroy (teleport.gameObject);
		}
		foreach (Planet planet in planets)
		{
			Destroy (planet.gameObject);
		}
		foreach (Obstacle obstacle in obstacles)
		{
			Destroy (obstacle.gameObject);
		}
		foreach (TeleportTarget target in targets)
		{
			Destroy (target.gameObject);
		}
		foreach (MoneyPoint coin in money)
		{
			Destroy (coin.gameObject);
		}
		if (playerObj != null)
			Destroy (playerObj.gameObject);

		this.money = 0;
		score = 0;
	}

	public void Pause ()
	{
		bPause = true;
        anim.SetFloat("Speed", 0);
    }

    public void UnPause()
    {
        bPause = false;
        anim.SetFloat("Speed", 1);
    }

    /* 
	 * функция для очереди планет, Queue не пашет(
	 */
    void MoveQueue ()
	{
		//GameObject[] clone = planets.Clone;
		for (int i = 0; i < numberOfPlanets - 1; i++)
		{
			planets [i] = planets [i + 1];
		}
	}

	/*
	 * двигаем планеты вдоль экрана
	 */
	void MovePlanets ()
	{
		Debug.Log ("moving planets");

		for (int i = 0; i < numberOfPlanets - 1; i++)
		{
			if (planets [i] != null)
				planets [i].GetComponent<Planet> ().coords.x += offset.x;
		}

		//MoveBackground ();
	}

	/*void MoveBackground()
	{
		foreach (GameObject tile in backgroundTiles) {
			tile.GetComponent<Background> ().coords.x -= 16;
		}

		Destroy (backgroundTiles [0], 3);

		for (int i = 0; i < 4; i++) {
			backgroundTiles[i] = backgroundTiles[i+1];
		}

		backgroundTiles[4] = Instantiate (bgAsset, new Vector3(4*16, 0, 10), Quaternion.identity, this.gameObject.transform) as GameObject;
	}*/

	void FixedUpdate ()
	{
		scoreObj = GameObject.Find ("score");
		moneyObj = GameObject.Find ("money");

		if ((scoreObj != null)&&(!Shop))
			scoreObj.GetComponent<Text> ().text = score.ToString ();

		if ((moneyObj != null)&&(!Shop))
			moneyObj.GetComponent<Text> ().text = money.ToString ();
	}

	public void OpenSettings ()
	{
		Clear ();
		controlCanvas.SetActive (false);
		mainMenuCanvas.SetActive (false);
		gameOverCanvas.SetActive (false);
		settingsCanvas.SetActive (true);
		bPause = true;
	}

	public void LoadMenu ()
	{
        if(!showingAds)
        {
            Clear();
            controlCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            gameOverCanvas.SetActive(false);
            settingsCanvas.SetActive(false);
            bPause = true;
            musicSource.Stop();
        }		
	}

    public void PlayAudio(string type)
    {
        switch (type)
        {
            case "Death":
                source.clip = death;
                break;
            case "Pickup":
                source.clip = pickup;
                break;
            case "Teleport":
                source.clip = teleport;
                break;
            case "ButtonPress":
                source.clip = buttonPress;
                break;
            case "Unlock":
                source.clip = unlock;
                break;
        }

        if(PlayerPrefs.GetString("Sounds") != "false")
            source.Play();
    }
}
