using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class InputController : MonoBehaviour
{
    public MovementRigidBody movement;
    public WeaponHolder weapon;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameMode.Instance.LeaveRoom();
        }

        if (Input.GetKeyDown(KeyCode.R)) // самоуничтожение на R
        {
            //    GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(this.gameObject);
            SendMessage("DoDamageById", new object[2] { 999, GetComponent<health>().playerid }, SendMessageOptions.DontRequireReceiver);
        }

        movement.MouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //weapon.receivedWeaponChange = true;
        if (Input.GetKeyDown(KeyCode.Alpha0)) {weapon.ActiveSlot = 0; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha1)) {weapon.ActiveSlot = 1; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha2)) {weapon.ActiveSlot = 2; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha3)) {weapon.ActiveSlot = 3; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha4)) {weapon.ActiveSlot = 4; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha5)) {weapon.ActiveSlot = 5; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha6)) {weapon.ActiveSlot = 6; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha7)) {weapon.ActiveSlot = 7; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha8)) {weapon.ActiveSlot = 8; weapon.receivedWeaponChange = true; } else
        if (Input.GetKeyDown(KeyCode.Alpha9)) {weapon.ActiveSlot = 9; weapon.receivedWeaponChange = true; }// else
        //    weapon.receivedWeaponChange = false;

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
