using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
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

    override public void Shoot() {
        Debug.Log("Shot with Pistol");

    }
}
