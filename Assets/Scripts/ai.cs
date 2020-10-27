using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai : MonoBehaviour
{
    // здесь будет логика и управление бота
    Vector3 target;
    public MovementRigidBody movement;
    public WeaponHolder weapon;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform.position;
        movement = GetComponent<MovementRigidBody>();
        //movement.input.Set(1.0f, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement.spd = (Input.GetKey(KeyCode.LeftShift)) ? movement.RunSpeed : movement.speed;
        target = GameObject.FindWithTag("Player").transform.position;
        Vector3 vec = target - transform.position;
        vec = transform.InverseTransformVector(vec);
        //vec.y = 0f;
        //vec = Vector3.ClampMagnitude(vec, 1.0f);
        movement.input.Set(vec.x,vec.z);
        Debug.Log(movement.input);
    }

    UnityEngine.AI.NavMeshPath pathFindPathtoPlayer() {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        UnityEngine.AI.NavMesh.CalculatePath(transform.position, target, -1, path);
        //Debug.Log(path);
        return path;
    }


}
