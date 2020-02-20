using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour {

    public Transform target;
    public Transform throwPoint;
    public GameObject ball;
    public float timeTillHit = 1f;

	void Start () {
        Throw();
	}

    void Throw()
    {
        float xdistance;
        xdistance = target.position.x -throwPoint.position.x;

        float ydistance;
        ydistance = target.position.y - throwPoint.position.y;

        float throwAngle; // in radian
		//OLD
		//throwAngle = Mathf.Atan ((ydistance + 4.905f) / xdistance);
       	//UPDATED
		throwAngle = Mathf.Atan ((ydistance + 4.905f*(timeTillHit*timeTillHit)) / xdistance);
		//OLD
		//float totalVelo = xdistance / Mathf.Cos(throwAngle) ;
		//UPDATED
		float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);

        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos (throwAngle);
		yVelo = totalVelo * Mathf.Sin (throwAngle);

        GameObject bulletInstance = Instantiate (ball, throwPoint.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
        Rigidbody2D rigid;
        rigid = bulletInstance.GetComponent<Rigidbody2D> ();

        rigid.velocity = new Vector2 (xVelo, yVelo);
    }
}