using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float pause = 0.0f;
    public void Start()
    {
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration+pause);
    }
}