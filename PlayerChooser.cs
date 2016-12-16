using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChooser : MonoBehaviour {
    [SerializeField] float offset;
    [SerializeField] float smooth;
    [SerializeField]
    GameObject buttonPrev, buttonNext, buttonChoose, moneyObj;
    [SerializeField] Sprite inactive, active;
    GameObject[] skins;
    [SerializeField]
    GameObject[] priceButtons;
    float posX;
    private int index = 1;

    private void Start()
    {
        Advertisement.Initialize("1202997");
        skins = new GameObject[3];
        skins[0] = GameObject.Find("player_blue");
        skins[1] = GameObject.Find("player_default");
        skins[2] = GameObject.Find("player_gold");
        posX = this.transform.localPosition.x;
        buttonPrev.SetActive(false);
        buttonPrev.GetComponent<Buttons>().normal = inactive;
        buttonPrev.GetComponent<Image>().sprite = inactive;
        moneyObj.GetComponent<Text>().text = PlayerPrefs.GetInt("Money").ToString();
    }

    private void Update()
    {
        string[] unlocked = PlayerPrefs.GetString("UnlockedSkins").Split(' ');
        bool isUnlocked = false;
        foreach (string number in unlocked)
        {
            if (number == index.ToString())
            {
                isUnlocked = true;

                foreach (GameObject button in priceButtons)
                {
                    button.GetComponent<Image>().enabled = false;
                    button.SetActive(false);
                }

                if (index == PlayerPrefs.GetInt("SkinNumber"))
                {
                    buttonChoose.GetComponent<Image>().enabled = false;
                    buttonChoose.SetActive(false);
                }

                else
                {
                    buttonChoose.GetComponent<Image>().enabled = true;
                    buttonChoose.SetActive(true);
                }

                break;
            }
        }

        if (!isUnlocked)
        {
            buttonChoose.GetComponent<Image>().enabled = false;
            buttonChoose.SetActive(false);
            if(index > 1)
            {
                foreach (GameObject button in priceButtons)
               {
                        button.GetComponent<Image>().enabled = false;
                        button.SetActive(false);
               }

                    priceButtons[index - 2].GetComponent<Image>().enabled = true;
                    priceButtons[index - 2].SetActive(true);
            }                
        }               
                      

        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(posX, this.transform.localPosition.y, this.transform.localPosition.z), Time.deltaTime * smooth);
        GameObject moneyText = GameObject.Find("money");
        moneyText.GetComponent<Text>().text = PlayerPrefs.GetInt("Money").ToString();
    }

	public void PreviousSkin()
    {
        if (index > 1)
        {
            buttonNext.SetActive(true);
            buttonNext.GetComponent<Buttons>().normal = active;
            buttonNext.GetComponent<Image>().sprite = active;
            posX += offset * this.transform.localScale.x;

            index--;
            Debug.Log("index " + index); 

            if (index < 2)
            {
                buttonPrev.SetActive(false);
                buttonPrev.GetComponent<Buttons>().normal = inactive;
                buttonPrev.GetComponent<Image>().sprite = inactive;
            }                                     
        }
              
    }

    public void NextSkin()
    {
        if(index < 3)
        {
            buttonPrev.SetActive(true);
            buttonPrev.GetComponent<Buttons>().normal = active;
            buttonPrev.GetComponent<Image>().sprite = active;
            posX -= offset * this.transform.localScale.x;

            index++;
            Debug.Log("index " + index);

            if (index > 2)
            {
                buttonNext.GetComponent<Buttons>().normal = inactive;
                buttonNext.GetComponent<Image>().sprite = inactive;
                buttonNext.SetActive(false);
            }                
        }        
    }

    public void ChooseSkin()
    {
        PlayerPrefs.SetInt("SkinNumber", index);
    }
}
