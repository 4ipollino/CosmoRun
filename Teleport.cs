using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	private GameObject target, player;
	private GameProcess gp;

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Colliding teleport...");
		if (other.tag == "Player") {
			Teleportation ();		
		}
	}

	public void SetTarget(GameObject target, GameObject player, GameProcess gp)
	{
		this.target = target;	
		this.player = player;
		this.gp = gp;
	}

	void Teleportation()
	{
		Debug.Log ("teleportation function");
		player.GetComponent<Player> ().SetTarget (target);
		player.GetComponent<Player>().SetAngle(target.GetComponent<Planet>().GetTeleportTarget().GetComponent<TeleportTarget> ().angle);
		Destroy (target.GetComponent<Planet> ().GetTeleportTarget ().GetComponent<TeleportTarget> ().gameObject);
		target.GetComponent<Planet> ().StartMoving ();
        //gp.PlayAudio("Teleport");
		gp.IncreaseScore (1);
		gp.CreatePlanet ();
	}
}
