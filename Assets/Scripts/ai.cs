using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
    health hlth;

    System.Random r = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<MovementRigidBody>();
        hlth = GetComponent<health>();
        ChoseCharacterTarget();
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
        Vector3 vec = trgt - weapon.transform.position;
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
        //charactertarget = GameObject.FindWithTag("Player");
        //return;

        GameObject tmp1 = GameObject.FindWithTag("Player");
        GameObject[] tmp2 = GameObject.FindGameObjectsWithTag("Character");
       //Debug.Log(tmp1 + " "+tmp2.Length);
        tmp2 = tmp2.Append(tmp1).ToArray();
        //Debug.Log(tmp2.Length);
        //for (int i = tmp2.Length - 1; i >= 0; --i) { 
        //    if (tmp2[i] == this.gameObject || (tmp2[i].GetComponent<health>().teamid == this.hlth.teamid)) System.Array.FindAll()
        //}
        GameObject[] tmp3 = System.Array.FindAll(tmp2, x => (x != this.gameObject && (x.GetComponent<health>().teamid != this.hlth.teamid || this.hlth.teamid == -1)));
        int n = r.Next(0,tmp3.Length);

        //Debug.Log(tmp3.Length +" "+ tmp3[n]);
        if (tmp3.Length == 0) return;
        charactertarget = tmp3[n];
    }

    void JumpOver()
    {
        RaycastHit hit;
        //Physics.SphereCast(transform.TransformPoint(new Vector3(0, 2, 1f)), 0.35f, Vector3.down, out hit); // потому что слишком дорогой
        Physics.Raycast(transform.TransformPoint(new Vector3(0, 2, 1f)), Vector3.down, out hit);
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
                    target = currentpath.corners[currentnode] + Vector3.up * 1.6f;
                    looktarget = currentpath.corners[currentnode] + Vector3.up * 1.6f;
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
        if (Physics.Raycast(weapon.transform.position,(charactertarget.transform.position + Vector3.up * 0.5f - weapon.transform.position),out hit, 15f, ~(1 << 9))) {
            if (hit.collider.gameObject.layer == 10){//charactertarget) {
                looktarget = charactertarget.transform.position+Vector3.up*0.5f;
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
            } //else Debug.Log("hit missed");

        }
        else {
            timeshoot = 0.0f;
            //Debug.Log("cant hit");
        }
    }

}
