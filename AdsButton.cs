using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour {

    [SerializeField] GameObject restoreButton;
    [SerializeField] Text restoreText;

	public void removeAds()
    {
        PlayerPrefs.SetString("Ads", "disabled");
    }
	
	// Update is called once per frame
	void Update () {
        if(PlayerPrefs.GetString("Ads") == "disabled")
        {
            restoreButton.SetActive(false);
            restoreButton.GetComponent<Image>().enabled = false;
            restoreText.enabled = false;
        }                    		
	}
}
