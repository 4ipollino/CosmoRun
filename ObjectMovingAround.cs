using UnityEngine;
using System.Collections;

public class ObjectMovingAround : MonoBehaviour {

	[SerializeField] protected const float zPos = 10;
	[SerializeField] protected int step = 2;
    //[SerializeField] protected float timeDecreaseStep = .01f;
    //[SerializeField] protected float accelStep = .01f;
    protected GameObject gameProcessObj;
	protected GameObject target;

	protected float xPos = 99;
	protected float yPos = 99;
	protected int angle = 0;
	protected float counter = 0;
    //protected float counter2 = 0;
    
    public float secondsPerMove = .3f;
    //public float targetSecondsPerMove = .02f;
	public float movingSmooth = 200;
	public int direction = 1;

	//сразу считаем координаты и ставим игрока на начальную позицию
	virtual protected void Start() {
		if(CalculateNextCoords ())
			this.transform.position = new Vector3 (xPos, yPos, zPos);
		gameProcessObj = GameObject.Find ("Background");        
	}

	// Update is called once per frame
	virtual protected void FixedUpdate () {
		
		if (gameProcessObj != null) 
		{					
            //проверяем запущена ли игра
			if (!gameProcessObj.GetComponent<GameProcess> ().bPause)
			{
                /*** счетчик времени для анимации движения по окружности ***/

			    if (counter >= secondsPerMove) 
				{
					if (CalculateNextCoords ())
						Move ();
					counter = 0;
				}

                counter += Time.fixedDeltaTime;                
            }
		}
	}

	/*** выставляем целью планету вокруг которой движемся ***/
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	/*** перемещение к следующим координатам ***/
	virtual protected void Move()	{
		this.transform.position = Vector3.Lerp(this.transform.position, new Vector3 (xPos, yPos, zPos), Time.deltaTime*movingSmooth);	
		Quaternion newAngle = this.transform.rotation;
		newAngle.eulerAngles = new Vector3 (0, 0, angle);
		this.transform.rotation = newAngle;
	}

	//считаем координаты следующей точки на окружности
	virtual protected bool CalculateNextCoords() {
		if (target != null) {
			CircleCollider2D targetCollision = target.GetComponentInChildren<CircleCollider2D> ();
			xPos = targetCollision.radius * target.transform.localScale.x * Mathf.Cos ((angle + 90) * Mathf.PI / 180) + target.transform.position.x;
			yPos = targetCollision.radius * target.transform.localScale.y * Mathf.Sin ((angle + 90) * Mathf.PI / 180) + target.transform.position.y;
			angle += step * direction;
			if (angle == 360)
				angle = 0;
			return true;
		} else
			return false;
	}

	//смена направления движения
	virtual public void ChangeMoveDirection() {
		direction *= -1;
		if (this.GetComponentInChildren<SpriteRenderer> ().flipX == true)
			this.GetComponentInChildren<SpriteRenderer> ().flipX = false;
		else
			this.GetComponentInChildren<SpriteRenderer> ().flipX = true;
	}

	virtual public void SetAngle(int angle){
		this.angle = angle;
	}
}
	

