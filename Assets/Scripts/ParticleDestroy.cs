using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public void Start()
    {
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}