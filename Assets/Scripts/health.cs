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
    public /*float*/int maxhp=10;
    public bool DmgNumbers = true;

    private Animator an;

    void Start()
    {
        gameMode = GameObject.Find("Global").GetComponent<GameMode>();
        an = GetComponentInChildren<Animator>();
        //maxhp = 11;
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

        an.SetTrigger("hit");
    }

    void DoDamageById(object[] obj) // в DoDamage возможны ситуации когда используется ссылка на уничтоженный gameobject стрелявшего, надо передавать playerid
    {
        /*float*/
        int damage;
        int whoDamaged;
        try
        {
            damage = (/*float*/int)obj.GetValue(0);
            whoDamaged = (int)obj.GetValue(1);
        }
        catch { return; }
        int indx = gameMode.findplayerindex(whoDamaged);

        if (!gameMode.friendlyfire && gameMode.ScoreTable[indx].team != -1 && teamid == gameMode.ScoreTable[indx].team)
        {
            if (this.playerid != whoDamaged) return;
        } // обработка friendlyfire, но ракетница сама себя дамажит, а teamid=-1 значит что дамаг наносится всем

        hp -= damage;
        if (DmgNumbers)
        {
            //Debug.Log(this.name + " got damaged by "+damage+"hp, by "+whoDamaged+", "+hp+"hp left"); // надо поменять названия объектов на ники 
            Debug.Log(this.nick + " got damaged by " + damage + "hp, by " + gameMode.ScoreTable[indx].nick + ", " + hp + "hp left");

            //GameObject tmp = new GameObject("DamageText");
            //ui = tmp.AddComponent<>();
            //ui. = "Whatever";
        }

        if (hp <= 0)
        {
            if (gameMode != null)
                gameMode.RegisterKill(whoDamaged, this.playerid);
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
