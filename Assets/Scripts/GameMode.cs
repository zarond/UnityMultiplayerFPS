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
    // Start is called before the first frame update
    void Start()
    {
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
        RegisterNewPlayerAndSpawn(0, 0, "player");
        RegisterNewPlayerAndSpawn(-1, 1, "enemy");
        GetComponentInChildren<Camera>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        for (int i = 0; i < gameObject.transform.childCount; ++i) { this.gameObject.transform.GetChild(i).gameObject.SetActive(false); }

    }

    // Update is called once per frame
    void Update()
    {
        int indx = ScoreTable.FindIndex(x => x.isAlive == false);
        if (indx>=0 && indx< ScoreTable.Count) {
            GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>()
            .Respawn(null, (ScoreTable[indx].playerid==0)? 0:1 , ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
            ScoreTable[indx].isAlive = true;
        } // respawn characters
    }

    public void RegisterNewPlayerAndSpawn(int team, int mode=0, string nick = "default") {
        int playerid = ScoreTable.Count;
        GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(null, mode, playerid, team, nick);
        AddNewPlayerToTable(playerid, team, nick);
        ScoreTable[playerid].isAlive = true;
    }

    public void AddNewPlayerToTable(int pl, int t=0, string nick = "default") {
        //Inventory inventoryItem = inventario.Find((x) => x.name == someString)
        ScoreTable.Add(new Score(pl,t,nick));
    }

    public void RemovePlayer(int pl) {
        int indx = 0;
        for (; indx < ScoreTable.Count; ++indx) if (ScoreTable[indx].playerid == pl) break;
        ScoreTable.RemoveAt(indx);
    }

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
    }
}
