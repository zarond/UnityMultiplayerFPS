using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[System.Serializable]
public class RocketLauncher : Weapon
{
    public GameObject refer;
    public AudioClip firesound;
    AudioSource source;
    //public bool canhurtplayer = false;

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
        source = GetComponent<AudioSource>();
        ownerid = this.owner.GetComponent<health>().playerid;
    }

    override public void Shoot() {
        //Debug.Log("Shot with RocketLauncher");
        source.PlayOneShot(firesound, 0.4f);
        GameObject temp = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        temp.GetComponent<RocketLifeCycle>().owner = this.owner; // задать принадлежность снаряда, может быть ошибка при отложенном попадании
        temp.GetComponent<RocketLifeCycle>().ownerid = this.ownerid; // задать принадлежность снаряда
        Vector3 tempvelocity = Vector3.zero;
        if (TransferVelocity) tempvelocity = owner.GetComponent<Rigidbody>().velocity;
        temp.GetComponent<Rigidbody>().velocity = speed * temp.transform.forward + tempvelocity;
    }
}
