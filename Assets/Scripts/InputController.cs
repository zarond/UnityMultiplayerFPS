using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public MovementRigidBody movement;
    public WeaponHolder weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
        //    GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(this.gameObject);
        }

        movement.MouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        weapon.receivedWeaponChange = true;
        if (Input.GetKeyDown(KeyCode.Alpha0)) weapon.ActiveSlot = 0; else
        if (Input.GetKeyDown(KeyCode.Alpha1)) weapon.ActiveSlot = 1; else
        if (Input.GetKeyDown(KeyCode.Alpha2)) weapon.ActiveSlot = 2; else
        if (Input.GetKeyDown(KeyCode.Alpha3)) weapon.ActiveSlot = 3; else
        if (Input.GetKeyDown(KeyCode.Alpha4)) weapon.ActiveSlot = 4; else
        if (Input.GetKeyDown(KeyCode.Alpha5)) weapon.ActiveSlot = 5; else
        if (Input.GetKeyDown(KeyCode.Alpha6)) weapon.ActiveSlot = 6; else
        if (Input.GetKeyDown(KeyCode.Alpha7)) weapon.ActiveSlot = 7; else
        if (Input.GetKeyDown(KeyCode.Alpha8)) weapon.ActiveSlot = 8; else
        if (Input.GetKeyDown(KeyCode.Alpha9)) weapon.ActiveSlot = 9; else
            weapon.receivedWeaponChange = false;

        weapon.shootStates[0] = Input.GetMouseButtonDown(0);
        weapon.shootStates[1] = Input.GetMouseButton(0);
        weapon.shootStates[2] = Input.GetMouseButtonUp(0);

    }

    void FixedUpdate() {

        movement.input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.jump = (Input.GetKey("space") || Input.GetKeyDown("space"));
        movement.spd = (Input.GetKey(KeyCode.LeftShift)) ? movement.RunSpeed : movement.speed;

    }

    //private void OnDestroy()
    //{
    //    GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Invoke("Respawn(this.gameObject)",0.5f);// Respawn(this.gameObject);
    //}
}
