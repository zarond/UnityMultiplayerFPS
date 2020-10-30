using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// здоровье и информация, в какой команде игрок
public class health : MonoBehaviour
{
    public int teamid = 0;
    public int playerid = 0;
    public string nick = "default";
    public GameMode gameMode=null;

    public /*float*/int hp;
    public bool DmgNumbers = true;


    void Start()
    {
        gameMode = GameObject.Find("Global").GetComponent<GameMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) Destroy (this.gameObject);
    }

    //void DoDamage(float damage, GameObject whoDamaged = null)
    void DoDamage(object[] obj)
    {
        /*float*/int damage;
        GameObject whoDamaged=null;
        try {
            damage = (/*float*/int)obj.GetValue(0);
            whoDamaged = (GameObject)obj.GetValue(1);
        } catch { return; }

        if (!gameMode.friendlyfire && whoDamaged.GetComponent<health>().teamid!=-1 && teamid == whoDamaged.GetComponent<health>().teamid) { 
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
            if (gameMode!=null)
            gameMode.RegisterKill(whoDamaged.GetComponent<health>().playerid, this.playerid);
        };
    }
}
