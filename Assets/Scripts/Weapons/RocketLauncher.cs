using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketLauncher : Weapon
{
    public GameObject refer;

    public RocketLauncher() {
        id = 1;
        name = "Rocket Launcher";
        automatic = true;
        fire_rate = 1.0f;
        //maxAmmo = 10;
        TransferVelocity = true;
        lifeDuration = 30.0f;
        speed = 50.0f;
        //bulletPrefab = (GameObject)Resources.Load("/Prefabs/bullet.prefab");
        TimeToPressTrigger = 0.0f; //0.1f;
        slot = 2;
    }

    void Start()
    {
        bulletPrefab = refer;
    }

    override public void Shoot() {
        Debug.Log("Shot with RocketLauncher");
        GameObject temp = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        Vector3 tempvelocity = Vector3.zero;
        if (TransferVelocity) tempvelocity = GameObject.FindWithTag("Player").GetComponent<Rigidbody>().velocity;
        temp.GetComponent<Rigidbody>().velocity = speed * temp.transform.forward + tempvelocity;
    }
}
