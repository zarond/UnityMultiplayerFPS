using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

// здоровье и информация, в какой команде игрок
public class health : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
    public int teamid = 0;
    public int playerid = 0;
    public string nick = "default";
    //public GameMode gameMode = null; //GameMode.Instance; //null;
    private PhotonView photonView = null;

    [Tooltip("The current Health of our player")]
    public /*float*/int hp;
    public /*float*/int maxhp=10;
    public bool DmgNumbers = true;

    private Animator an;

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(hp);
        }
        else
        {
            // Network player, receive data
            this.hp = (int)stream.ReceiveNext();
        }
    }
    #endregion

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView == null) photonView = GetComponent<PhotonView>();
        nick = photonView.Owner.NickName;
        playerid = photonView.Owner.ActorNumber;

        object[] instantiationData = info.photonView.InstantiationData;
        teamid = (int)instantiationData[0];
        Debug.LogWarning("Instantiate" + nick + playerid + teamid + instantiationData);

        int indx = GameMode.Instance.findplayerindex(photonView.Owner.ActorNumber);
        if (indx < 0) return;
        GameMode.Instance.ScoreTable[indx].isAlive = true;
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
    }

    void Start()
    {
        if (photonView == null) photonView = GetComponent<PhotonView>();
        //nick = photonView.Owner.NickName;
        //playerid = photonView.Owner.ActorNumber;
        //gameMode = GameMode.Instance; //Find("Global").GetComponent<GameMode>();
        //if (gameMode == null)
        //{
        //    Debug.Log("Where is gameMode?");
        //}
        an = GetComponentInChildren<Animator>();
        //maxhp = 11;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (hp <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
                //GameMode.Instance.Respawn();
                Debug.Log(nick + " died");
            }

        }

    }

    //void Killed()
    //{
        

    //}

    //void DoDamage(float damage, GameObject whoDamaged = null)
    void DoDamage(object[] obj)
    {
        /*float*/int damage;
        GameObject whoDamaged=null;
        try {
            damage = (/*float*/int)obj.GetValue(0);
            whoDamaged = (GameObject)obj.GetValue(1);
        } catch { return; }

        if (!GameMode.Instance.friendlyfire && whoDamaged.GetComponent<health>().teamid!=-1 && teamid == whoDamaged.GetComponent<health>().teamid) { 
            if (this.gameObject != whoDamaged) return; 
        } // обработка friendlyfire, но ракетница сама себя дамажит, а teamid=-1 значит что дамаг наносится всем

        hp -= damage;
        if (DmgNumbers) {
            //Debug.Log(this.name + " got damaged by "+damage+"hp, by "+whoDamaged+", "+hp+"hp left"); // надо поменять названия объектов на ники 
            Debug.Log(this.nick + " got damaged by " + damage + "hp, by " + whoDamaged.GetComponent<health>().nick + ", " + hp + "hp left");

            //GameObject tmp = new GameObject("DamageText");
            //ui = tmp.AddComponent<>();
            //ui. = "Whatever";
        }

        if (hp <= 0) {
            if (GameMode.Instance != null)
                GameMode.Instance.RegisterKill(whoDamaged.GetComponent<health>().playerid, this.playerid);
        };

        an.SetTrigger("hit");
    }

    [PunRPC]
    void DoDamageById(object[] obj) // в DoDamage возможны ситуации когда используется ссылка на уничтоженный gameobject стрелявшего, надо передавать playerid
    {
        //Debug.Log(/*photonView.Owner.NickName*/nick + " is damaged!");
        
        /*float*/
        int damage;
        int whoDamaged;
        try
        {
            damage = (/*float*/int)obj.GetValue(0);
            whoDamaged = (int)obj.GetValue(1);
        }
        catch { return; }
        int indx = GameMode.Instance.findplayerindex(whoDamaged);

        if (indx == -1) { Debug.LogWarning("No index found"); return; }

        if (!GameMode.Instance.friendlyfire && GameMode.Instance.ScoreTable[indx].team != -1 && teamid == GameMode.Instance.ScoreTable[indx].team)
        {
            if (this.playerid != whoDamaged) return;
        } // обработка friendlyfire, но ракетница сама себя дамажит, а teamid=-1 значит что дамаг наносится всем

        hp -= damage;
        Debug.Log(nick + " received DAMAGE. Its hp is " + hp);
        if (DmgNumbers)
        {
            //Debug.Log(this.name + " got damaged by "+damage+"hp, by "+whoDamaged+", "+hp+"hp left"); // надо поменять названия объектов на ники 

            //Debug.Log(this.nick + " got damaged by " + damage + "hp, by " + gameMode.ScoreTable[indx].nick + ", " + hp + "hp left");

            //GameObject tmp = new GameObject("DamageText");
            //ui = tmp.AddComponent<>();
            //ui. = "Whatever";
        }

        if (hp <= 0)
        {
            if (GameMode.Instance != null)
                //GameMode.Instance.RegisterKill(whoDamaged, this.playerid);
                if (PhotonView.Get(this).IsMine)
                    GameMode.Instance.photonView.RPC("RegisterKillViaObject", RpcTarget.All, new object[2] { whoDamaged, this.playerid });
        };

        an.SetTrigger("hit");
    }

    public void Heal(int hlth)
    {
        if (hp > 0) {
            hp += hlth;
            if (hp > maxhp) hp = maxhp;
        }
    }
}
