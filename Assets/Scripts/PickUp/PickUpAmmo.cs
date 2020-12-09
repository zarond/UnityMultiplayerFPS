using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

//[RequireComponent(typeof(SphereCollider))]

public class PickUpAmmo : PickUp
{
    //public float distance;
    public int weaponid;
    public int numberofammo;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    override public void PickUpObject(PickUpSystem WhoPicked)
    {
        WeaponHolder cmp = WhoPicked.gameObject.GetComponentInChildren<WeaponHolder>();
        Debug.Log(WhoPicked.name + "Picked Up Ammo");

        if (cmp.Ammo[weaponid] + numberofammo <= cmp.maxAmmo[weaponid])
        {
            cmp.Ammo[weaponid] += numberofammo;
            //WhoPicked.Global.PickUps.Remove(this);
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject); //Destroy(this.gameObject);
            }
            else
            {
                // Вызов мастер-клиента и просьба ему прибить pick up

            }
        }
        else
        {
            numberofammo -= cmp.maxAmmo[weaponid] - cmp.Ammo[weaponid];
            cmp.Ammo[weaponid] = cmp.maxAmmo[weaponid];
        }

    }
    /*
    override public bool DistanceCriteria(PickUpSystemTrigger WhoPicked)
    {
        if ((WhoPicked.transform.position - this.transform.position).magnitude <= distance) return true;
        return false;
    }
    */
    override public bool PickUpCriteria(PickUpSystem WhoPicked)
    {
        WeaponHolder cmp = WhoPicked.gameObject.GetComponentInChildren<WeaponHolder>();
        if (cmp.Ammo[weaponid] >=
        cmp.maxAmmo[weaponid]) return false;
        return true;
    }
}
