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
        public Score(int player, int team = 0, int score=0, int K=0, int D=0)
        {
            this.playerid = player;
            this.score = score;
            this.K = K;
            this.D = D;
            this.team = team;
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
        RegisterNewPlayerAndSpawn(0, 0);
        RegisterNewPlayerAndSpawn(0, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterNewPlayerAndSpawn(int team, int mode=0) {
        int playerid = ScoreTable.Count;
        GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(null, mode, playerid, team);
        AddNewPlayerToTable(playerid, team);
    }

    public void AddNewPlayerToTable(int pl, int t=0) {
        //Inventory inventoryItem = inventario.Find((x) => x.name == someString)
        ScoreTable.Add(new Score(pl,t));
    }

    public void RemovePlayer(int pl) {
        int indx = 0;
        for (; indx < ScoreTable.Count; ++indx) if (ScoreTable[indx].playerid == pl) break;
        ScoreTable.RemoveAt(indx);
    }

    public void RegisterKill(int player1, int player2) {
        int indx1 = 0;
        int indx2 = 0;
        for (; indx1 < ScoreTable.Count; ++indx1) if (ScoreTable[indx1].playerid == player1) break;
        for (; indx2 < ScoreTable.Count; ++indx2) if (ScoreTable[indx2].playerid == player2) break;
        if (indx1 < ScoreTable.Count && indx2 < ScoreTable.Count) {
            ScoreTable[indx1].score += 10;
            ScoreTable[indx1].K += 1;
            ScoreTable[indx2].D += 1;
        }
    }
}
