using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour 
{
	Gun equipedGun;
	public Transform gunHold;
	public Gun initialGun;

	public void Start()
	{
		if(initialGun != null)
		{
			Equip(initialGun);
		}
	}

	public void Equip(Gun newGun)
	{
		if(this.equipedGun != null)
		{
			Destroy(equipedGun.gameObject);
		}

		equipedGun = Instantiate(newGun, gunHold.position, gunHold.rotation) as Gun;
		equipedGun.transform.parent = gunHold;
	}

	public void Shoot()
	{
		if(equipedGun != null)
		{
			equipedGun.Shoot();
		}
	}
}
