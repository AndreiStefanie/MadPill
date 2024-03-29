﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public LayerMask collisionMask;
	float speed = 10;
    float damage = 1;
    float lifetime = 3;

    public void Start()
    {
        GameObject.Destroy(gameObject, lifetime);
    }

    public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	void Update()
	{
		float moveDistance = Time.deltaTime * speed;
		CheckCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}

	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
		{
            OnHitObject(hit);
		}
	}

    void OnHitObject(RaycastHit hit)
    {
        IDamageable dmgObject = hit.collider.GetComponent<IDamageable>();
        if(dmgObject != null)
        {
            dmgObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}
