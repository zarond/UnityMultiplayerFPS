using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Vector2 input;
    private Vector2 rawinput;
    public Transform DbgJ;
    public Transform DbgJr;
    private Vector2 minput;
    private Vector2 mrawinput;
    public Transform DbgM;
    public Transform DbgMr;
    public Transform DbgMoveField;
    public Vector3 MoveField = new Vector3(0, 0, 0);

    private bool paused = false;
    public GameObject PauseMenu;

    public GameObject GroundedIndicator;
    public GameObject cube;
    private Rigidbody cube_rigidbody;
    private MovementRigidBody cube_mov;

    private health Health;
    public Text Health_text;
    public Text Ammo_text;
    private WeaponHolder weapons;

    public GameObject Score;
    public GameObject KillTable;
    public GameMode gameMode = null;
    public GameObject scoreEntryPrefab;
    public GameObject killEntryPrefab;
    public List<GameObject> scoreEntries = new List<GameObject>();
    //public List<GameObject> KillEntries = new List<GameObject>();
    public int maxKillEntries = 5;

    public bool dbguienabled = false;

    //-/ пока не работает
    //-/ private UnityEngine.UI.Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cube_rigidbody = cube.GetComponent<Rigidbody>();
        cube_mov = cube.GetComponent<MovementRigidBody>();
        Health = this.GetComponentInParent<health>();
        weapons = this.transform.parent.GetComponentInChildren<WeaponHolder>();
        gameMode = GameObject.Find("Global").GetComponent<GameMode>();
        dbguisetenabled(dbguienabled);

        gameMode.OnKillRegistered += ShowKillMessage;

        UpdateKillBoardUI();

        //-/exitButton = GameObject.Find("Global").GetComponentInChildren<UnityEngine.UI.Button>();
        //-/exitButton.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (gameMode == null) return;
        gameMode.OnKillRegistered -= ShowKillMessage;
    }

    void dbguisetenabled(bool t) {
        dbguienabled = t;
        if (t) {
            DbgMoveField.gameObject.SetActive(true);
            DbgJ.gameObject.SetActive(true); ;
            DbgJr.gameObject.SetActive(true); ;
            DbgM.gameObject.SetActive(true); ;
            DbgMr.gameObject.SetActive(true); ;
        }
        else {
            GroundedIndicator.SetActive(false);
            PauseMenu.SetActive(false);
            DbgMoveField.gameObject.SetActive(false);
            DbgJ.gameObject.SetActive(false); ;
            DbgJr.gameObject.SetActive(false); ;
            DbgM.gameObject.SetActive(false); ;
            DbgMr.gameObject.SetActive(false); ;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (dbguienabled) {
            input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rawinput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            MoveField = cube_rigidbody.velocity / cube_mov.spd;
            MoveField = cube.transform.InverseTransformVector(MoveField);
            DbgMoveField.localPosition = 50 * new Vector2(MoveField.x, MoveField.z);
            DbgJ.localPosition = 50 * input;
            DbgJr.localPosition = 50 * rawinput;

            minput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mrawinput.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            DbgM.localPosition = 50 * minput;
            DbgMr.localPosition = 50 * mrawinput;

            if (!paused && Input.GetKeyDown("p"))
            {
                paused = true;
                PauseMenu.SetActive(paused);
                Time.timeScale = 0;
            }
            else if (paused && Input.GetKeyDown("p"))
            {
                paused = false;
                PauseMenu.SetActive(paused);
                Time.timeScale = 1;
            }

            GroundedIndicator.SetActive(cube_mov.isGrounded);
        }

    

        if (Health != null) {
            Health_text.text = "Health: " + (Health.hp).ToString();
        }
        if (weapons != null)
        {
            Ammo_text.text = "Ammo: " + (weapons.Ammo[weapons.Weapons[weapons.ActiveWeapon].id]).ToString();
        }

        if (gameMode != null) {
            Score.SetActive(Input.GetKey(KeyCode.Tab));
            if (Input.GetKey(KeyCode.Tab)) { 
                UpdateScoreBoardUI();
                //-/exitButton.gameObject.SetActive(true);
            }
            //else
            //{
            //-/    exitButton.gameObject.SetActive(false);
            //}
        }


    }

    void UpdateScoreBoardUI() {
        int diff = scoreEntries.Count - gameMode.ScoreTable.Count;
        if (diff < 0) {
            // добавить UI
            for (int i = 0; i < -diff; ++i) {
                GameObject tmp = Instantiate(scoreEntryPrefab);
                tmp.transform.SetParent(Score.transform.GetChild(0),false);
                tmp.transform.position.Set(0,0,0);
                tmp.transform.Translate(23*Vector3.down * (i + scoreEntries.Count));
                scoreEntries.Add(tmp);
            }
        }
        else if (diff > 0) {
            // удалить лишний UI
            for (int i = 0; i < diff; ++i) Destroy(scoreEntries[gameMode.ScoreTable.Count + i]);
            scoreEntries.RemoveRange(gameMode.ScoreTable.Count, diff);
        }
        for (int i=0;i< scoreEntries.Count; ++i) {
            scoreEntries[i].transform.GetChild(0).GetComponent<Text>().text = (gameMode.ScoreTable[i].playerid).ToString();
            scoreEntries[i].transform.GetChild(1).GetComponent<Text>().text = gameMode.ScoreTable[i].nick;
            scoreEntries[i].transform.GetChild(2).GetComponent<Text>().text = (gameMode.ScoreTable[i].score).ToString();
            scoreEntries[i].transform.GetChild(3).GetComponent<Text>().text = (gameMode.ScoreTable[i].K).ToString();
            scoreEntries[i].transform.GetChild(4).GetComponent<Text>().text = (gameMode.ScoreTable[i].D).ToString();
        }

    }

    void ShowKillMessage(int player1, int player2) {
        if (player1 == player2) { // mistakes were made
        }
        else { // x pwned by y
        }
        if (player1 == Health.playerid){ // show message in the center of the screen
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "you killed " +
            gameMode.ScoreTable[gameMode.findplayerindex(player2)].nick;
        } 
        else if (player2 == Health.playerid) { // show message in the center of the screen
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "pwned by " +
            gameMode.ScoreTable[gameMode.findplayerindex(player1)].nick;
        }
        UpdateKillBoardUI();
        Debug.Log(gameMode.ScoreTable[gameMode.findplayerindex(player2)].nick
        + " pwned by " +gameMode.ScoreTable[gameMode.findplayerindex(player1)].nick);
    }

    void UpdateKillBoardUI()
    {
        //int diff = gameMode.KillTable.Count;
        int lim = (gameMode.KillTable.Count < maxKillEntries)? gameMode.KillTable.Count: maxKillEntries;
        if (KillTable.transform.childCount < lim)
        {
            // добавить UI
            for (int i = KillTable.transform.childCount; i < lim; ++i)
            {
                GameObject tmp = Instantiate(killEntryPrefab);
                tmp.transform.SetParent(KillTable.transform, false);
                tmp.transform.position.Set(0, 0, 0);
                tmp.transform.Translate(30 * Vector3.down * i);
            }
        }
        for (int i = 0; i < KillTable.transform.childCount; ++i)
        {
            int player1 = gameMode.KillTable[gameMode.KillTable.Count - 1 - i].x;
            int player2 = gameMode.KillTable[gameMode.KillTable.Count - 1 - i].y;
            KillTable.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = 
            gameMode.ScoreTable[gameMode.findplayerindex(player2)].nick + " pwned by "
            //KillTable.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = 
            + gameMode.ScoreTable[gameMode.findplayerindex(player1)].nick;

        }

    }
}
