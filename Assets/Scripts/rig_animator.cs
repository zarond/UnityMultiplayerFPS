using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rig_animator : MonoBehaviour
{
    public Animator anim;
    public MovementRigidBody controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetFloat("forward", 1.0f);
        //anim.SetFloat("side", 0.0f);
        //controller.transform.InverseTransformVector(controller.Character.velocity);
        anim.SetFloat("forward", controller.transform.InverseTransformVector(controller.Character.velocity).z);
        anim.SetFloat("side", controller.transform.InverseTransformVector(controller.Character.velocity).x);
    }
}
