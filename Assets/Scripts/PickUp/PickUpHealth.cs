using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PickUpHealth : PickUp
{
    //public float distance;
    //public int weaponid;
    public int hp;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        photonView.SetOwnerInternal(PhotonNetwork.MasterClient, PhotonNetwork.MasterClient.ActorNumber);
    }

    override public void PickUpObject(PickUpSystem WhoPicked)
    {
        health cmp = WhoPicked.gameObject.GetComponent<health>();
        Debug.Log(WhoPicked.name + "Picked Up HP");

        if (cmp.hp + hp <= cmp.maxhp)
        {
            //cmp.Ammo[weaponid] += numberofammo;
            cmp.Heal(hp);
            //WhoPicked.Global.PickUps.Remove(this);
            if (photonView.IsMine) //Destroy(this.gameObject);
            {
                PhotonNetwork.Destroy(this.gameObject); //Destroy(this.gameObject);
            }
            else // Вроде и так работает, но на всякий
            {
                //photonView.RequestOwnership();
                //PhotonNetwork.Destroy(this.gameObject);

                Debug.Log("transfering ownership from " + photonView.Owner.NickName);
                //photonView.RequestOwnership();
                //photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                //Debug.Log("owner is now " + photonView.Owner.NickName + " / should be " + PhotonNetwork.LocalPlayer.NickName);
                photonView.SetOwnerInternal(PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
                Debug.Log("owner is now " + photonView.Owner.NickName + " / should be " + PhotonNetwork.LocalPlayer.NickName);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        else
        {
            hp -= cmp.maxhp - cmp.hp;
            cmp.Heal(cmp.maxhp - cmp.hp);
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
        health cmp = WhoPicked.gameObject.GetComponent<health>();
        if (cmp.hp >= cmp.maxhp) return false;
        return true;
    }
}
