using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Obstacle : ObjectMovingAround
{
    [SerializeField] float secondsMoreThanPlayer;
	public bool moving = false;

    protected override void FixedUpdate ()
	{

		if ((moving)&&(gameProcessObj != null) &&(!gameProcessObj.GetComponent<GameProcess>().IsPaused()))
           
            if (counter >= secondsPerMove + secondsMoreThanPlayer)
            {
                if (CalculateNextCoords())
                    Move();
                counter = 0;
            }

            counter += Time.fixedDeltaTime;          
    }

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Colliding obstacle..." + " " + movingSmooth + " " + PlayerPrefs.GetInt("speed"));
		if (other.tag == "Player")
		{
			gameProcessObj.GetComponent<GameProcess> ().GameOver ();
            gameProcessObj.GetComponent<GameProcess>().PlayAudio("Death");
		}
	}
}
