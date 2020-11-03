using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    GameMode gm;

    private void Start()
    {
        gm = GameObject.Find("Global").GetComponent<GameMode>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        GameObject go = collision.transform.root.gameObject;
        if (go.layer == 9) {
            go.SendMessage("DoDamageById", new object[2] { 999, go.GetComponent<health>().playerid }, SendMessageOptions.DontRequireReceiver);
        }
    }
}
