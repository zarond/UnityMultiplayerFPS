using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [HideInInspector]
    public class /*struct*/ Score {
        public int playerid;
        public int score;
        public int K;
        public int D;
        public int team;
        public string nick;
        public bool isAlive = false;
        public Score(int player, int team = 0, string nick = "default",int score=0, int K=0, int D=0)
        {
            this.playerid = player;
            this.score = score;
            this.K = K;
            this.D = D;
            this.team = team;
            this.nick = nick;
        }
    }
    // сюда идет таблица очков и настройки режима игры
    public bool friendlyfire = false;
    public List<Score> ScoreTable = new List<Score>();
    [HideInInspector]
    public List<Vector3Int> KillTable = new List<Vector3Int>();
    public int SpawnNumberOfEnemies;

    public event System.Action<int,int> OnKillRegistered;

    System.Random r = new System.Random();
    public SimpleRespawn[] respawns;// точки респавна не меняются в течении матча

    public int thisclientid = -100; // id пользователя, которому принадлежит этот клиент. на каждом клиенте свое значение

    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        respawns = FindObjectsOfType<SimpleRespawn>();
        thisclientid = 0; // для примера, надо назначать через интернет
        //AddNewPlayerToTable(0);
        //AddNewPlayerToTable(1);

        //RegisterNewPlayerAndSpawn(0, 0,"player");
        //RegisterNewPlayerAndSpawn(-1, 1,"enemy");

        //RegisterNewPlayerAndSpawn(2, 1);
        //RegisterNewPlayerAndSpawn(3, 1);
        //StartGame();
    }

    public void StartGame() {
        Debug.Log("starting game");
        RegisterNewPlayerAndSpawn(0, 0, "player"); // пока что персонаж под номером 0 всегда - игрок
        //RegisterNewPlayerAndSpawn(-1, 1, "enemy");
        //RegisterNewPlayerAndSpawn(1, 1, "enemy1");
        //RegisterNewPlayerAndSpawn(2, 1, "enemy2");
        for (int i = 0; i < SpawnNumberOfEnemies; ++i) { RegisterNewPlayerAndSpawn(i-1, 1, "enemy"+i.ToString()); }

        GetComponentInChildren<Camera>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        for (int i = 0; i < gameObject.transform.childCount; ++i) { this.gameObject.transform.GetChild(i).gameObject.SetActive(false); }

    }

    // Update is called once per frame
    void Update()
    {
        int indx = ScoreTable.FindIndex(x => x.isAlive == false);
        if (indx>=0 && indx< ScoreTable.Count) {
            //GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>()
            //.Respawn(null, (ScoreTable[indx].playerid==0)? 0:1 , ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
            respawns[r.Next(0, respawns.Length)].Respawn(null, (ScoreTable[indx].playerid == thisclientid) ? 0 : 1, ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
            ScoreTable[indx].isAlive = true; // данный респавн предполагает что игрок локальной машины имеет id = 0, неправильно  
        } // respawn characters
    }

    public void RegisterNewPlayerAndSpawn(int team, int mode=0, string nick = "default") {
        //int playerid = ScoreTable.Count; // не учитывает если игрок выйдет из игры и список игроков уменьшиться
        int playerid = counter++;

        //GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(null, mode, playerid, team, nick);
        respawns[r.Next(0, respawns.Length)].Respawn(null, mode, playerid, team, nick);
        AddNewPlayerToTable(playerid, team, nick);
        ScoreTable[playerid].isAlive = true;
    }

    public void AddNewPlayerToTable(int pl, int t=0, string nick = "default") {
        //Inventory inventoryItem = inventario.Find((x) => x.name == someString)
        ScoreTable.Add(new Score(pl,t,nick));
    }

    public void RemovePlayer(int pl) {
        int indx = -1;
        //for (; indx < ScoreTable.Count; ++indx) if (ScoreTable[indx].playerid == pl) break;
        indx = ScoreTable.FindIndex(x => x.playerid == pl);
        if (indx < ScoreTable.Count && indx >=0)
            ScoreTable.RemoveAt(indx);
    }

    public int findplayerindex(int pl) { return ScoreTable.FindIndex(x => x.playerid == pl); }

    public void RegisterKill(int player1, int player2) {
        int indx1 = 0;
        int indx2 = 0;

        indx1 = ScoreTable.FindIndex(x => x.playerid == player1);
        indx2 = ScoreTable.FindIndex(x => x.playerid == player2);

        //for (; indx1 < ScoreTable.Count; ++indx1) if (ScoreTable[indx1].playerid == player1) break;
        //for (; indx2 < ScoreTable.Count; ++indx2) if (ScoreTable[indx2].playerid == player2) break;
        if (indx1 < ScoreTable.Count && indx2 < ScoreTable.Count) {
            if (player1!=player2)
            {
                ScoreTable[indx1].score += 10;
                ScoreTable[indx1].K += 1;
            }
            ScoreTable[indx2].D += 1;
            ScoreTable[indx2].isAlive = false;
        }
        else { return; }
        KillTable.Add(new Vector3Int(player1, player2, 0));
        if (OnKillRegistered == null) return;
        OnKillRegistered(player1,player2);
    }
}
