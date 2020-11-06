using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_or_enemy : MonoBehaviour
{
    public bool isplayer = true;
    public bool toAttachAiScript = true;
    public bool bePlayerIfIdIsZero = true; // если это включено, то в зависимости от id==0 префаб становится либо игроком либо противником, перезаписывает isplayer
    public MonoBehaviour[] ChangesComponents;
    public GameObject[] ChangesObjects;
    // Start is called before the first frame update
    void Start()
    {
        if (bePlayerIfIdIsZero) {
            isplayer = (GetComponent<health>().playerid == 0);
        }
        if (isplayer == false) { 
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
