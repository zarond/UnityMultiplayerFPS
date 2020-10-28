using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [HideInInspector]
    public struct Score {
        int playerid;
        int score;
        int K;
        int D;
        public Score(int player,int score,int K,int D)
        {
            this.playerid = player;
            this.score = score;
            this.K = K;
            this.D = D;
        }
    }
    // сюда идет таблица очков и настройки режима игры
    public bool friendlyfire = false;
    public List<Score> ScoreTable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewPlayerToTable(int pl) {
        //Inventory inventoryItem = inventario.Find((x) => x.name == someString)
        ScoreTable.Insert(0, new Score(pl, 0, 0, 0));
    }
}
