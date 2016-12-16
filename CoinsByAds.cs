using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class CoinsByAds : MonoBehaviour {

    [SerializeField]
    GameProcess gp;

    public void ShowAd(string zone = "rewardedVideo")
    {
        if (CheckTime())
        {
            Advertisement.Initialize("1202984");
            ShowOptions options = new ShowOptions();
            options.resultCallback = AdCallbackhanler;

            if (Advertisement.IsReady(zone))
            {
                Advertisement.Show(zone, options);
                PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
            }
        }
    }

    private bool CheckTime()
    {
        string lastTimeStr = PlayerPrefs.GetString("LastTime", "12/6/2016 12:17:18 AM");

        DateTime lastTime = DateTime.Parse(lastTimeStr);

        if (DateTime.Now > lastTime.AddMinutes(10))
            return true;
        else return false;
    }

    void AdCallbackhanler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Ad Finished. Rewarding player...");
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + gp.giftGems);
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad Skipped");
                break;
            case ShowResult.Failed:
                Debug.Log("Ad failed");
                break;
        }
    }

    private void FixedUpdate()
    {
            if (!CheckTime())
            {
                this.gameObject.GetComponentInChildren<Text>().text = "WAIT 10 MINS!";
            }

            else
            {
                this.gameObject.GetComponentInChildren<Text>().text = "FREE COINS!";
            }
    }
}
