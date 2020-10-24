using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public GameObject refer;
    Transform CamHandlerObject;

    public Pistol() {
        id = 0;
        name = "Pistol";
        automatic = false;
        fire_rate = 0.1f;
        //maxAmmo = 50;
        TransferVelocity = true;
        TimeToPressTrigger = 0.0f;
        slot = 1;
        //lifeDuration = 30.0f;
        //speed = 10.0f;
        //bulletPrefab = (GameObject)Resources.Load("bullet.prefab");
    }

    void Start()
    {
        bulletPrefab = refer;
       //CamHandlerObject = gameObject.GetComponentInParent<Transform>();
        CamHandlerObject = transform.parent.parent;
    }

    override public void Shoot() {
        //Ray ScreenVector = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));
        Ray ScreenVector = new Ray(CamHandlerObject.position, CamHandlerObject.forward);
        RaycastHit hit;
        Physics.Raycast(ScreenVector, out hit);
        GameObject flare = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        GameObject shot = Instantiate(bulletPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        Debug.Log("Shot with Pistol");

    }
}
