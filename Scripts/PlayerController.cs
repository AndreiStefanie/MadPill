using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour 
{
	Rigidbody playerBody;
	Vector3 velocity;

	void Start()
	{
		playerBody = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	public void LookAt(Vector3 lookPoint)
	{
		Vector3 correctHeight = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt(correctHeight);
	}

	void FixedUpdate()
	{
		playerBody.MovePosition(playerBody.position + velocity * Time.fixedDeltaTime);
	}
}
