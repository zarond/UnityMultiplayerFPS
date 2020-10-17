using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject bullet;
    public bool allowAutomatic;
    public float fireRate;

    void Start()
    {
        
    }

    float cooldown = 0;
    bool finishedShooting = true;
    void Update()
    {
        if(cooldown > 0) cooldown -= Time.deltaTime;
        if(Input.GetMouseButtonUp(0)) finishedShooting = true;

        if(Input.GetMouseButton(0))
        {
            if((allowAutomatic || finishedShooting) && cooldown <= 0)
            {
                    cooldown = 1f / fireRate;
                    shoot();
            }
            finishedShooting = false;
        }
    }

    void shoot() 
    {
        GameObject barrelEnd = GameObject.Find("barrel end");
        bullet.transform.position = barrelEnd.transform.position;
        bullet.transform.rotation = transform.rotation;
        Instantiate(bullet);
    }
}
