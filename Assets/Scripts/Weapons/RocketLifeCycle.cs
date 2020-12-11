using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class RocketLifeCycle : MonoBehaviour, IPunInstantiateMagicCallback
{
    private SphereCollider col;
    private Rigidbody rgbody;
    public GameObject Impact;
    bool alreadyhit = false;
    public GameObject owner;
    public int ownerid;
    //public bool canhurtplayer = true;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        rgbody = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 10f); // удалить объект если он не попал никуда в течении 10 секунд
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        rgbody = GetComponent<Rigidbody>();
        object[] instantiationData = info.photonView.InstantiationData;
        ownerid = (int)instantiationData[0]; // задать принадлежность снаряда
        rgbody.velocity = (Vector3)instantiationData[1];

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!alreadyhit)
        {
            alreadyhit = true;
            //GameObject tmp = Instantiate(Impact, transform.position, transform.rotation);
            //tmp.GetComponent<RocketImpact>().owner = this.owner; // задать принадлежность взрыва, может быть ошибка при отложенном попадании
            //tmp.GetComponent<RocketImpact>().ownerid = this.ownerid; // задать принадлежность взрыва
            //Debug.Log("Hit");
            //Destroy(this.gameObject);

            // for photon
            if ( PhotonView.Get(this).IsMine)
            {
                object[] myCustomInitData = { ownerid };
                GameObject tmp = PhotonNetwork.Instantiate(Impact.name, transform.position, transform.rotation, 0, myCustomInitData);
                //if (PhotonView.Get(this)) PhotonNetwork.Destroy(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    // если попал прямо в персонажа, но не в себя
    private void OnTriggerEnter(Collider collider1)
    {
        //Debug.Log("On Trigger");
        if (collider1.transform.root.gameObject!=owner && collider1.gameObject.layer == 10)
        {
            //Debug.Log("Hit directly");
            OnCollisionEnter(new Collision());
        }
    }
}
