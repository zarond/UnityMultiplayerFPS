using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class player_or_enemy : MonoBehaviourPun
{
    public bool isplayer = true;
    public bool toAttachAiScript = true;
    public bool bePlayerIfIdIsZero = true; // если это включено, то в зависимости от id==0 префаб становится либо игроком либо противником, перезаписывает isplayer
    public MonoBehaviour[] ChangesComponents;
    public GameObject[] ChangesObjects;

    public GameObject camHandler;
    void Awake()
    {
        /*
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            //camHandler = GetChildWithName(this, "CameraHandler");
            camHandler = this.transform.Find("CameraHandler").gameObject;
            camHandler.SetActive(true);

        }
        else
        {
            camHandler = this.transform.Find("CameraHandler").gameObject;
            camHandler.SetActive(true);
            camHandler.transform.Find("Main Camera").gameObject.SetActive(false);
            camHandler.transform.Find("fpsrig").gameObject.SetActive(false);
        }
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        */
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bePlayerIfIdIsZero) {
            isplayer = (GetComponent<health>().playerid == 0);
        }
        if (photonView.IsMine == false/*isplayer == false*/) { 
            ChangeToEnemy();
            if (!toAttachAiScript) Destroy(GetComponent<ai>());
        }
        else {
            Destroy(GetComponent<ai>());
        }
        Destroy(this);
    }

    void ChangeToEnemy() {
        tag = "Character";
        GetComponent<FPSMode>().firstpersonmode = false;
        for (int i = 0; i < ChangesComponents.Length; ++i) {
            //ChangesComponents[i].enabled = false; // просто отключение, может лучше удалять
            Destroy(ChangesComponents[i]);
        }
        for (int i = 0; i < ChangesObjects.Length; ++i)
        {
            //ChangesObjects[i].SetActive(false); // просто отключение, может лучше удалять
            ChangesObjects[i].transform.parent = null;
            Destroy(ChangesObjects[i]); // просто отключение, может лучше удалять
        }
        //GetComponentInChildren<Camera>().enabled = false;
        //GetComponentInChildren<AudioListener>().enabled = false;
        //Destroy(GetComponentInChildren<Camera>());
        //Destroy(GetComponentInChildren<AudioListener>());
    }

    // не факт что понадобится, не факт что будет возможно делать, если удалять компоненты, а не просто выключать.
    //void ChangeToPlayer() {
    //    tag = "Player";
    //}
}
