using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public float hp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) Destroy (this.gameObject);
    }

    void DoDamage(float damage)
    {
        hp -= damage;
    }
}
