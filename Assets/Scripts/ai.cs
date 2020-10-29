using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai : MonoBehaviour
{
    // здесь будет логика и управление бота
    Vector3 target;
    Vector3 looktarget;
    GameObject charactertarget;
    UnityEngine.AI.NavMeshPath currentpath = null;
    int currentnode=0;
    float timerstuck = 0.0f;
    float timeshoot = 0.0f;

    public MovementRigidBody movement;
    public WeaponHolder weapon;
    // Start is called before the first frame update
    void Start()
    {
        ChoseCharacterTarget();
        movement = GetComponent<MovementRigidBody>();
        //movement.input.Set(1.0f, 1.0f);
    }

    void GoTowardsTo(Vector3 trgt)
    {
        Vector3 vec = trgt - transform.position;
        vec = transform.InverseTransformVector(vec);
        movement.input.Set(vec.x, vec.z);
        //Debug.Log(movement.input);
    }

    void LookTo(Vector3 trgt) {
        Vector3 vec = trgt - transform.position;
        vec = weapon.transform.InverseTransformVector(vec);
        Quaternion rot = Quaternion.LookRotation(vec, Vector3.up);
        rot = Quaternion.RotateTowards(Quaternion.identity, rot, 5);
        Vector2 inp = new Vector2(rot.eulerAngles.y, -rot.eulerAngles.x);
        if (inp.x > 180) inp.x -= 360;
        else if (inp.x < -180) inp.x += 360;
        if (inp.y > 180) inp.y -= 360;
        else if (inp.y < -180) inp.y += 360;
        movement.MouseInput = inp;
        //Debug.Log(inp);
    }

    void ChoseCharacterTarget() {
        charactertarget = GameObject.FindWithTag("Player");
    }

    void JumpOver()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.TransformPoint(new Vector3(0, 2, 1f)), 0.35f, Vector3.down, out hit);
        Debug.DrawRay(hit.point, hit.normal, Color.yellow);
        if (hit.distance > 3.0f) movement.jump = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        movement.spd = (Input.GetKey(KeyCode.LeftShift)) ? movement.RunSpeed : movement.speed;
        movement.jump = false;
        JumpOver();

        if (charactertarget == null) { ChoseCharacterTarget(); if (charactertarget == null) return; }

        if (currentpath == null) {
            target = charactertarget.transform.position + charactertarget.transform.forward * 2f;
            looktarget = charactertarget.transform.position;
            if (Vector3.Distance(target, transform.position) > 5.0f) {
                FindPathtoTarget(target); 
                if (currentpath.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid) FindPathtoTarget(charactertarget.transform.position);
                if (currentpath.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid) currentpath = null;
            }
        }
        else {
            if (currentnode < 0) currentnode = 0;
            if (currentnode >= currentpath.corners.Length || Vector3.Distance(currentpath.corners[currentpath.corners.Length-1], transform.position) < 1.0f) { 
                //Debug.Log("arived");
                currentpath = null;
            }
            else
            {
                if (Vector3.Distance(currentpath.corners[currentnode], transform.position) < 1.0f) currentnode += 1;
                if (currentnode < currentpath.corners.Length)
                {
                    target = currentpath.corners[currentnode] + Vector3.up * 0.7f;
                    looktarget = currentpath.corners[currentnode] + Vector3.up * 0.7f;
                    Debug.DrawLine(transform.position,target);
                    if (movement.Character.velocity.sqrMagnitude < 0.05f) timerstuck+=Time.fixedDeltaTime;
                    if (timerstuck >= 1.0f) { currentpath = null; timerstuck = 0.0f; Debug.Log("been stuck"); }
                }
            }
        }

        GoTowardsTo(target);
        LockOnAndShoot();
        LookTo(looktarget);
    }

    void FindPathtoTarget(Vector3 trgt) {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        UnityEngine.AI.NavMesh.CalculatePath(transform.position, trgt, -1, path);
        //Debug.Log(path);
        //return path;
        Debug.Log("finding path "+path.status);
        currentpath = path;
        currentnode = 0;
    }

    void LockOnAndShoot() {
        weapon.receivedWeaponChange = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position,(charactertarget.transform.position- transform.position),out hit, 15f)) {
            if (hit.collider.gameObject == charactertarget) {
                looktarget = charactertarget.transform.position;
                timeshoot += Time.fixedDeltaTime;
                if (timeshoot >= 3.0f)
                {
                    timeshoot = 0.0f;

                    if (weapon.Ammo[weapon.Weapons[weapon.ActiveWeapon].id] > 0)
                    {
                        bool shot = weapon.Weapons[weapon.ActiveWeapon].TryShoot(true, true, false);
                        if (shot) weapon.Ammo[weapon.Weapons[weapon.ActiveWeapon].id]--;
                    }
                    else {
                        weapon.receivedWeaponChange = true;
                        weapon.ActiveSlot += 1;
                    }
                }
            }

        }
        else {
            timeshoot = 0.0f;
        }
    }

}
