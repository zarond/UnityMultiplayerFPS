using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rig_animator : MonoBehaviour
{
    public Animator anim;
    public MovementRigidBody controller;
    public WeaponHolder weaponHolder;
    private float forward;
    private float side;
    //public Transform leftHandPoint;
    //public Transform rightHandPoint;

    // Start is called before the first frame update
    void Start()
    {
        forward = 0.0f;
        side = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetFloat("forward", 1.0f);
        //anim.SetFloat("side", 0.0f);
        //controller.transform.InverseTransformVector(controller.Character.velocity);
        forward = Mathf.Lerp(forward, controller.transform.InverseTransformVector(controller.Character.velocity).z,7f*Time.deltaTime);
        side = Mathf.Lerp(side, controller.transform.InverseTransformVector(controller.Character.velocity).x, 7f * Time.deltaTime);
        anim.SetFloat("forward", forward);
        anim.SetFloat("side", side);
    }

    void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        anim.SetIKPosition(AvatarIKGoal.RightHand, weaponHolder.rightHandPoint.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, weaponHolder.rightHandPoint.rotation);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, weaponHolder.leftHandPoint.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, weaponHolder.leftHandPoint.rotation);
        anim.SetLookAtWeight(1.0f);
        anim.SetLookAtPosition(weaponHolder.leftHandPoint.position);
    }
}
