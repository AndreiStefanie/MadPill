using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : Entity 
{
	
	public float moveSpeed = 5;
	PlayerController controller;
	Camera mainCamera;
	GunController gunController;

	protected override void Start () 
	{
        base.Start();
        controller = GetComponent<PlayerController>();
		mainCamera = Camera.main;
		gunController = GetComponent<GunController>();
	}

	void Update () 
	{
		//Movement Input
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;

		controller.Move(moveVelocity);

		//Look Input
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayDistance;

		if(groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 intersectionPoint = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, intersectionPoint, Color.cyan);
			controller.LookAt(intersectionPoint);
		}

		//Weapon Input
		if(Input.GetMouseButton(0))
		{
			gunController.Shoot();
		}
	}
}
