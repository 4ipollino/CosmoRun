using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : ObjectMovingAround
{
    [SerializeField] float startupDeltaTime;

	override protected void Start ()
	{
        secondsPerMove = startupDeltaTime;
        base.Start ();
	}

	// Update is called once per frame
	void Update ()
	{

		if (!gameProcessObj.GetComponent<GameProcess> ().IsPaused ())
		{
			if (Input.GetTouch (0).phase == TouchPhase.Began)
			{
				ChangeMoveDirection ();			
			}
	    }
	}

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (counter2 >= accelStep)
        {
            if (secondsPerMove > targetSecondsPerMove)
                secondsPerMove -= timeDecreaseStep;
            Debug.Log("secondsPerMove: " + secondsPerMove);
            counter2 = 0;
        }

        counter2 += Time.fixedDeltaTime;
    }

    /*public void SetAngle(int angle){
		this.angle = angle;
		Update ();
	}*/
}
