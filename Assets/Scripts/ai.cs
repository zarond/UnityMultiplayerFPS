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
    // Update is called once per frame
    void FixedUpdate()
    {
        movement.spd = (Input.GetKey(KeyCode.LeftShift)) ? movement.RunSpeed : movement.speed;

        if (currentpath == null) {
            target = charactertarget.transform.position + charactertarget.transform.forward * 2f;
            looktarget = charactertarget.transform.position;
            if (Vector3.Distance(target, transform.position)>5.0f)
                FindPathtoTarget(target);
        }
        else {
            if (currentnode < 0) currentnode = 0;
            if (currentnode >= currentpath.corners.Length || Vector3.Distance(currentpath.corners[currentpath.corners.Length-1], transform.position) < 1.0f) { 
                Debug.Log("arived");
                currentpath = null;
            }
            else
            {
                if (Vector3.Distance(currentpath.corners[currentnode], transform.position) < 1.0f) currentnode += 1;
                if (currentnode < currentpath.corners.Length)
                {
                    target = currentpath.corners[currentnode];
                    looktarget = currentpath.corners[currentnode];
                    Debug.DrawLine(transform.position,target);
                    if (movement.Character.velocity.sqrMagnitude < 0.05f) timerstuck+=Time.fixedDeltaTime;
                    if (timerstuck >= 1.0f) { currentpath = null; timerstuck = 0.0f; Debug.Log("been stuck"); }
                }
            }
        }

        GoTowardsTo(target);
        LookTo(looktarget);
    }

    void FindPathtoTarget(Vector3 trgt) {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        UnityEngine.AI.NavMesh.CalculatePath(transform.position, trgt, -1, path);
        //Debug.Log(path);
        //return path;
        Debug.Log("finding path");
        currentpath = path;
    }

}
