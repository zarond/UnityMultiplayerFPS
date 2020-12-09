using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class SimpleRespawn : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public Transform Origin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    void Update() // неправильный подход
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn(GameObject.FindWithTag("Player"), 0, -1);
        }
    }*/

    // playerid=-1 значит что respawn постарается унаследовать id от pl, любое другое значение - создается с таким id 
    // teamid=-2 - значит что respawn постарается унаследовать teamid от pl, teamid=-1 - будет для режима free4all
    public void Respawn(GameObject pl=null, int mode=0, int playerid=-1, int teamid=-2, string nick = "default") {
        //if (pl != null) Destroy(pl);  // в конец
        GameObject tmp=null;
        if (mode == 0) //respawn playerprefab
            Debug.Log("...");
            //tmp = Instantiate(PlayerPrefab, Origin.position, Origin.rotation); // надо сделать чтобы оно могло респавнить не только игрока
            //tmp = PhotonNetwork.Instantiate(PlayerPrefab.name, Origin.position, Origin.rotation);
        else if (mode == 1 && EnemyPrefab != null) // respawn enemy
            tmp = Instantiate(EnemyPrefab, Origin.position, Origin.rotation);
        if (tmp == null) return;

        health hlth = tmp.GetComponent<health>();
        if (pl != null)
        {
            health hlth_pl = pl.GetComponent<health>();
            if (playerid == -1) { hlth.playerid = hlth_pl.playerid; hlth.nick = hlth_pl.nick; }
            else hlth.playerid = playerid;
            if (teamid == -2) hlth.teamid = hlth_pl.teamid;
            else hlth.teamid = teamid;
            Destroy(pl);
        }
        else { hlth.playerid = playerid; hlth.teamid = teamid; hlth.nick = nick; }

        //if (pl != null) Destroy(pl);
    }
}
