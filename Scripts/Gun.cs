using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile bullet;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;

	float nextShotTime;

	public void Shoot()
	{
		if(Time.time > nextShotTime)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			Projectile newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as Projectile;
			newBullet.SetSpeed(muzzleVelocity);
		}
	}
}
