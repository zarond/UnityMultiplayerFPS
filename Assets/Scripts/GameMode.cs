using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEngine.SceneManagement;
//using System.Linq;

using Photon.Pun;
using Photon.Realtime;

public class GameMode : MonoBehaviourPunCallbacks, IPunObservable
{
    /// <summary>
    /// То, что было в GameManager
    /// </summary>

    public static GameMode Instance;

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            AddNewPlayerToTable(other.ActorNumber, -1, other.NickName);


            //    LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            RemovePlayer(other.ActorNumber);

        //    LoadArena();
        }
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//Disconnect();
        PhotonNetwork.LoadLevel("Launcher");
        //SceneManager.LoadScene("Launcher");
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        /// PhotonNetwork.LoadLevel() should only be called if we are the MasterClient
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("PhotonNetwork : Loading map");
        /// We use PhotonNetwork.LoadLevel() to load the level we want, we don't use Unity directly, 
        /// because we want to rely on Photon to load this level on all connected clients in the room, 
        /// since we've enabled PhotonNetwork.AutomaticallySyncScene for this Game.
        PhotonNetwork.LoadLevel("map");
    }

    #endregion

    /// <summary>
    /// Конец того, что было в GameManager
    /// </summary>


    //[HideInInspector]
    [System.Serializable]
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
    //[HideInInspector]
    public List<Vector3Int> KillTable = new List<Vector3Int>();
    public int SpawnNumberOfEnemies;

    public event System.Action<int,int> OnKillRegistered;

    System.Random r = new System.Random();
    public SimpleRespawn[] respawns;// точки респавна не меняются в течении матча

    public int thisclientid = -100; // id пользователя, которому принадлежит этот клиент. на каждом клиенте свое значение

    int counter = 0;

    //private PhotonView photonView;

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        Wrapper test = new Wrapper();
        test.List = ScoreTable;
        string data = JsonUtility.ToJson(test);
        Debug.Log(data);*/
        //List<Score> Tmp = (JsonUtility.FromJson<Wrapper>(data)).List;
        //List<Score> TmpL = (JsonUtility.FromJson<Score[]>(data)).ToList();
        //ScoreTable = (JsonUtility.FromJson<Wrapper>(data)).List;

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient) {
                Wrapper test = new Wrapper();
                test.List = ScoreTable;
                string data = JsonUtility.ToJson(test);
                //Debug.Log(data + "sending");
                stream.SendNext(data);
            }
            // We own this player: send the others our data
            //string data = JsonUtility.ToJson(ScoreTable);
            //stream.SendNext(data);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient == false) {
                string data = (string)stream.ReceiveNext();
                ScoreTable = (JsonUtility.FromJson<Wrapper>(data)).List;
                //Debug.Log(data + "receving");
            }
            // Network player, receive data
            //this.hp = (int)stream.ReceiveNext();
            //string data = (string)stream.ReceiveNext();
            //Debug.Log(data);
            //ScoreTable = JsonUtility.FromJson<List<Score>>(data);
        }
    }
    #endregion

    void Awake()
    {
        //thisclientid = PhotonNetwork.LocalPlayer.ActorNumber;
        OnJoinedRoomCustom();
        StartGame();
    }
    // Start is called before the first frame update
    
    /*public void Respawn()
        {
                Debug.Log("ClientState=" + PhotonNetwork.NetworkClientState);
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, respawns[0].Origin.position, Quaternion.identity);
                //respawns[0].Respawn(playerPrefab.gameObject);
                RegisterNewPlayerAndSpawn(0, 0, "player"); // пока что персонаж под номером 0 всегда - игрок
        }
        */
    public void OnJoinedRoomCustom()
    {
        thisclientid = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber + "joined room");
        if (PhotonNetwork.IsMasterClient)
            RegisterNewPlayerAndSpawn(-1, 0, PhotonNetwork.LocalPlayer.NickName);
    }

    void Start()
    {
        Instance = this;

        respawns = FindObjectsOfType<SimpleRespawn>();
        thisclientid = PhotonNetwork.LocalPlayer.ActorNumber;//0; // для примера, надо назначать через интернет

        /*
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            

            bool notavailable = PhotonNetwork.NetworkClientState == ClientState.Leaving || PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer || PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer;
            bool available = PhotonNetwork.NetworkClientState == ClientState.Joined;
            if ((PlayerManager.LocalPlayerInstance == null) && available && !notavailable)
            {
                
                Respawn();

            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

        }
        */
        

        //AddNewPlayerToTable(0);
        //AddNewPlayerToTable(1);

        //RegisterNewPlayerAndSpawn(0, 0,"player");
        //RegisterNewPlayerAndSpawn(-1, 1,"enemy");

        //RegisterNewPlayerAndSpawn(2, 1);
        //RegisterNewPlayerAndSpawn(3, 1);
        //StartGame();
    }

    //public void StartGame() {
    //    Debug.Log("starting game");
    //    RegisterNewPlayerAndSpawn(0, 0, "player"); // пока что персонаж под номером 0 всегда - игрок
    //    //RegisterNewPlayerAndSpawn(-1, 1, "enemy");
    //    //RegisterNewPlayerAndSpawn(1, 1, "enemy1");
    //    //RegisterNewPlayerAndSpawn(2, 1, "enemy2");
    //    for (int i = 0; i < SpawnNumberOfEnemies; ++i) { RegisterNewPlayerAndSpawn(i-1, 1, "enemy"+i.ToString()); }

    //    GetComponentInChildren<Camera>().enabled = false;
    //    GetComponentInChildren<Canvas>().enabled = false;
    //    GetComponentInChildren<UnityEngine.UI.Button>().enabled = false;
    //    for (int i = 0; i < gameObject.transform.childCount; ++i) { this.gameObject.transform.GetChild(i).gameObject.SetActive(false); }

    //}

    public class Wrapper {
        public List<Score> List;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Wrapper test = new Wrapper();
        test.List = ScoreTable;
        string data = JsonUtility.ToJson(test);
        Debug.Log(data);
        List<Score> Tmp = (JsonUtility.FromJson<Wrapper>(data)).List;
        //List<Score> TmpL = (JsonUtility.FromJson<Score[]>(data)).ToList();
        Debug.Log(Tmp[0].nick);
        */

        if (PlayerManager.LocalPlayerInstance == null)
        {
            //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, respawns[0].Origin.position, Quaternion.identity);
            //respawns[0].Respawn(playerPrefab.gameObject);
            //RegisterNewPlayerAndSpawn(0, 0, "player"); // пока что персонаж под номером 0 всегда - игрок

            //int indxt = ScoreTable.FindIndex(x => x.playerid == thisclientid);
            //respawns[r.Next(0, respawns.Length)].Respawn(null, (ScoreTable[indxt].playerid == thisclientid) ? 0 : 1, ScoreTable[indxt].playerid, ScoreTable[indxt].team, ScoreTable[indxt].nick);
            //ScoreTable[indxt].isAlive = true;

        }

        int indx = ScoreTable.FindIndex(x => x.isAlive == false);
        if (indx>=0 && indx< ScoreTable.Count) {
            //GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>()
            //.Respawn(null, (ScoreTable[indx].playerid==0)? 0:1 , ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
            if (ScoreTable[indx].playerid == PhotonNetwork.LocalPlayer.ActorNumber) {
                respawns[r.Next(0, respawns.Length)].Respawn(null, (ScoreTable[indx].playerid == thisclientid) ? 0 : 1, ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
                ScoreTable[indx].isAlive = true;
            }
            //respawns[r.Next(0, respawns.Length)].Respawn(null, (ScoreTable[indx].playerid == thisclientid) ? 0 : 1, ScoreTable[indx].playerid, ScoreTable[indx].team, ScoreTable[indx].nick);
            //ScoreTable[indx].isAlive = true; // данный респавн предполагает что игрок локальной машины имеет id = 0, неправильно  
        } // respawn characters
    }

    //public void RegisterNewPlayerAndSpawn(int team, int mode=0, string nick = "default") {
    //    //int playerid = ScoreTable.Count; // не учитывает если игрок выйдет из игры и список игроков уменьшиться
    //    int playerid = counter++;

    //    //GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(null, mode, playerid, team, nick);
    //    respawns[r.Next(0, respawns.Length)].Respawn(null, mode, playerid, team, nick);
    //    AddNewPlayerToTable(playerid, team, nick);
    //    ScoreTable[playerid].isAlive = true;
    //}

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
    [PunRPC]
    public void RegisterKillViaObject(object[] par) {
        int player1, player2;
        try
        {
            player1 = (int)par.GetValue(0);
            player2 = (int)par.GetValue(1);
        }
        catch { return; }
        RegisterKill(player1,player2);
    }


    /// -------------------------------------

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public void StartGame()
    {
        Debug.Log("starting game");
        // Debug.Log(playerPrefab.name); // "player"
        //PhotonNetwork.Instantiate(playerPrefab.name, respawns[0].Origin.position, Quaternion.identity);
        //RegisterNewPlayerAndSpawn(0, 0, "player"); // пока что персонаж под номером 0 всегда - игрок

        for (int i = 0; i < SpawnNumberOfEnemies; ++i) { RegisterNewPlayerAndSpawn(i - 1, 1, "enemy" + i.ToString()); }

        // убирает интерфейс с подсказкой и всем таким
        GetComponentInChildren<Camera>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        GetComponentInChildren<UnityEngine.UI.Button>().enabled = false;
        for (int i = 0; i < gameObject.transform.childCount; ++i) { this.gameObject.transform.GetChild(i).gameObject.SetActive(false); }

    }

    public void RegisterNewPlayerAndSpawn(int team, int mode = 0, string nick = "default")
    {
        //int playerid = ScoreTable.Count; // не учитывает если игрок выйдет из игры и список игроков уменьшится
        int playerid = PhotonNetwork.LocalPlayer.ActorNumber;//counter++;

        //GameObject.FindWithTag("Respawn").GetComponent<SimpleRespawn>().Respawn(null, mode, playerid, team, nick);
        //respawns[r.Next(0, respawns.Length)].Respawn(null, mode, playerid, team, nick);
        AddNewPlayerToTable(playerid, team, nick);
        //ScoreTable[playerid].isAlive = true;
    }
}
