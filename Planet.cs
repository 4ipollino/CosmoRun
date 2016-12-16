using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{

	private GameProcess gp;
	private GameObject obstacle, teleport, teleportTarget, money, obstacleAsset, teleportAsset, teleportTargetAsset, moneyAsset;
	public int teleportAngle, obstacleAngle, moneyAngle, teleportTargetAngle = -999;
	public Vector3 coords;
	private float smoothTime;
	//private bool created = false;

	//public bool FirstPlanet;	// Use this for initialization

	//функция для создания объектов на планете
	public void CreatePlanetObjects (bool FirstPlanet)
	{

		/*
		 *********** расчет углов для объектов на планете *************************
		 */

		if (FirstPlanet)
		{
			teleportAngle = 180;			
		} else
		{
			teleportAngle = Random.Range (0, 360);
			teleportTargetAngle = teleportAngle + 180 + Random.Range (-20, 20);
		}
			
		obstacleAngle = teleportAngle + Random.Range (-25, 25);
		moneyAngle = teleportAngle + Random.Range (20, 340);

		/*
		 * ************************************************************************
		 */ 

		//подгружаем ресурсы
		obstacleAsset = Resources.Load<GameObject> ("Obstacle");
		teleportAsset = Resources.Load<GameObject> ("Teleport");
		teleportTargetAsset = Resources.Load<GameObject> ("TeleportTarget");
		moneyAsset = Resources.Load<GameObject> ("Money");

		Quaternion newAngle = this.transform.rotation;
		Vector2 coords = CalculateRandomCoords (obstacleAngle);

		if (!FirstPlanet)
		{
			obstacle = Instantiate (obstacleAsset, this.transform) as GameObject;
            obstacle.GetComponentInChildren<Obstacle>().movingSmooth *= (100.0f + PlayerPrefs.GetInt("speed"))/100.0f; 
            obstacle.GetComponentInChildren<Obstacle> ().SetAngle (obstacleAngle);
			obstacle.GetComponentInChildren<Obstacle> ().SetTarget (this.gameObject);
			obstacle.transform.position = new Vector3 (coords.x, coords.y, 10);
			newAngle.eulerAngles = new Vector3 (0, 0, obstacleAngle);
			obstacle.transform.rotation = newAngle;

            if (Random.Range(0, 100) < 40)
            {
                newAngle.eulerAngles = new Vector3(0, 0, moneyAngle);
                coords = CalculateRandomCoords(moneyAngle);
                money = Instantiate(moneyAsset, new Vector3(coords.x, coords.y, 10), newAngle, this.transform) as GameObject;
            }
        }

		teleport = Instantiate (teleportAsset, this.transform) as GameObject;
		coords = CalculateRandomCoords (teleportAngle);
		teleport.transform.position = new Vector3 (coords.x, coords.y, 10);
		newAngle.eulerAngles = new Vector3 (0, 0, teleportAngle);
		teleport.transform.rotation = newAngle;     

		if (teleportTargetAngle != -999)
		{
			teleportTarget = Instantiate (teleportTargetAsset, this.transform) as GameObject;
			//teleportTarget.GetComponent<TeleportTarget> ().SetParent (this.gameObject);
			coords = CalculateRandomCoords (teleportTargetAngle);
			teleportTarget.transform.position = new Vector3 (coords.x, coords.y, 10);
			newAngle.eulerAngles = new Vector3 (0, 0, teleportTargetAngle);
			teleportTarget.transform.rotation = newAngle;
			teleportTarget.GetComponent<TeleportTarget> ().angle = teleportTargetAngle;
		}

		Debug.Log ("created planet");

		//created = true;
	}

	//считаем координаты случайной точки на окружности
	Vector2 CalculateRandomCoords (int angle)
	{	
		float xPos, yPos;	
		CircleCollider2D targetCollision = this.GetComponentInChildren<CircleCollider2D> ();
		xPos = targetCollision.radius * transform.localScale.x * Mathf.Cos ((angle + 90) * Mathf.PI / 180) + this.transform.position.x;
		yPos = targetCollision.radius * transform.localScale.y * Mathf.Sin ((angle + 90) * Mathf.PI / 180) + this.transform.position.y;
		return new Vector2 (xPos, yPos);
	}

	/*public void DestroyPlanet ()
	{
		Destroy (teleport.gameObject);
        if (obstacle != null)
		Destroy (obstacle.gameObject);
		if (teleportTarget != null)
			Destroy (teleportTarget.gameObject);
		Destroy (this.gameObject);
		if (money != null)
			Destroy (money.gameObject);
	}*/

	public GameObject GetTeleportTarget ()
	{
		return teleportTarget;
	}

	public GameObject GetTeleport ()
	{
		return teleport;
	}
	/*
	public void MovePlanet(Vector3 offset)
	{
		teleport.transform.position += offset;
		teleportTarget.transform.position += offset; 
	}
*/
	void FixedUpdate ()
	{
		if (!gp.IsPaused ())
		{
			this.transform.position = Vector3.Lerp (this.transform.position, coords, Time.fixedDeltaTime * smoothTime);
		}
	}

	void Start ()
	{
		gp = GameObject.Find ("Background").GetComponent<GameProcess> ();
		smoothTime = gp.smooth;
    }

	public void StartMoving ()
	{
            obstacle.GetComponentInChildren<Obstacle> ().moving = true;

		    if (Random.Range (-65, 34) <= 0)
		    {
			    obstacle.GetComponentInChildren<Obstacle> ().direction = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().direction * -1;			
		    } else
		    {
			    obstacle.GetComponentInChildren<Obstacle> ().direction = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().direction;
		    }			
	}
}
