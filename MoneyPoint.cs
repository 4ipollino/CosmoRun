using UnityEngine;
using System.Collections;

public class MoneyPoint : MonoBehaviour
{

	GameObject gameProcessObj;

	// Use this for initialization
	void Start ()
	{
		gameProcessObj = GameObject.Find ("Background");	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Colliding obstacle...");
		if (other.tag == "Player")
		{
			gameProcessObj.GetComponent<GameProcess> ().IncreaseMoney (1);
            gameProcessObj.GetComponent<GameProcess>().PlayAudio("Pickup");
			Destroy (this.gameObject);
		}
	}
}
