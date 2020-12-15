using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class Pistol : Weapon
{
    //public GameObject refer;
    Transform CamHandlerObject;
    public AudioClip firesound;
    AudioSource source;
    public GameObject flarePrefab;
    public GameObject hitPrefab;
    //public bool canhurtplayer = true;

    private PhotonView photonView;

    public Pistol() {
        id = 0;
        name = "Pistol";
        automatic = false;
        fire_rate = 0.1f;
        //maxAmmo = 50;
        TransferVelocity = true;
        TimeToPressTrigger = 0.0f;
        slot = 1;
        //lifeDuration = 30.0f;
        //speed = 10.0f;
        //bulletPrefab = (GameObject)Resources.Load("bullet.prefab");

        // added
        //sourceInner = source;
        //firesoundInner = firesound;
    }

    void Start()
    {
        //bulletPrefab = refer;
       //CamHandlerObject = gameObject.GetComponentInParent<Transform>();
        CamHandlerObject = transform.parent.parent;
        source = GetComponent<AudioSource>();
        ownerid = this.owner.GetComponent<health>().playerid;
        photonView = PhotonView.Get(this);//GetComponent<PhotonView>();
        //photonView = this.owner.GetComponent<PhotonView>();
        //photonView.ObservedComponents.Add(this);
        if (photonView == null)
        {
            Debug.Log("Whoops no photonView for poor Pistol");
        } else { Debug.Log(photonView.ViewID); }
    }

    override public void Shoot() {

        //if (!photonView.IsMine)
        //    return;
        //Ray ScreenVector = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));

        int layer = ~(1 << 9); // маска слоя все кроме физического коллайдера персонажей и триггер-коллайдеров самого игрока
        //if (!canhurtplayer) layer &= ~(1<<12); // и триггер-коллайдеров самого игрока
        Ray ScreenVector = new Ray(CamHandlerObject.position, CamHandlerObject.forward);

        //RaycastHit hit;
        //bool h = Physics.Raycast(ScreenVector,out hit, Mathf.Infinity ,layer);
        //GameObject flare = Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);
        //if (h) /*GameObject shot = */Instantiate(bulletPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        //Debug.Log("Shot with Pistol");

        //Debug.Log(hit.collider);
        //if (h && (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 12)) {
        //    hit.collider.transform.root.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver); // потому что health ставлю на родителя, а хитбоксы - на детях.
        //}

        //----------------------------------------------
        // другой способ

        RaycastHit[] hits = Physics.RaycastAll(ScreenVector, Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();

        //GameObject flare = Instantiate(flarePrefab, BarrelEnd.position, BarrelEnd.rotation);
        GameObject flare = PhotonNetwork.Instantiate(flarePrefab.name, BarrelEnd.position, BarrelEnd.rotation);

        //Debug.Log("Shot with Pistol");
        //source.PlayOneShot(firesound, 0.4f);
        photonView.RPC("playshoot", RpcTarget.All);

        if (hits != null)
        {
            for (int i = 0; i < hits.Length; ++i)
            {
                //Debug.Log(hits[i].collider);
                if (hits[i].collider.transform.root.gameObject != this.owner && hits[i].collider.gameObject.layer != 2)
                {
                    //Instantiate(hitPrefab, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                    PhotonNetwork.Instantiate(hitPrefab.name, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                    if (hits[i].collider.gameObject.layer == 10)
                    {
                        //hits[i].collider.transform.root.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
                        //hits[i].collider.transform.root.SendMessage("DoDamage", new object[2] {/*1.0f*/1, this.owner}, SendMessageOptions.DontRequireReceiver);
                        GameObject target = hits[i].collider.transform.root.gameObject;
                        Debug.Log(photonView.Owner.NickName + " has hit " + target.name);
                        //hits[i].collider.transform.root.SendMessage("DoDamageById", new object[2] {/*1.0f*/1, ownerid }, SendMessageOptions.DontRequireReceiver);

                        var targetphview = target.GetComponent<PhotonView>();

                        Debug.Log("Doing damage to " + targetphview.Owner.NickName + " by player(id) " + ownerid + ", ph view id: " + targetphview.ViewID);

                        targetphview.RPC("DoDamageById", RpcTarget.All/*Others*/, new object[2] { 1, ownerid });
                        /*target.*/
                        //photonView.RPC("DoDamageById", RpcTarget.Others, new object[2] {1, ownerid });
                    }
                    break;
                }

            }
        }

    }

    [PunRPC]
    void playshoot() {
        source.PlayOneShot(firesound, 0.4f);
    }
}
