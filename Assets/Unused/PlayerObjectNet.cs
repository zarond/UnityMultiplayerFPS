using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObjectNet : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) {
            return;
        }

        Debug.Log("Spawn");
        //Instantiate(PlayerUnitPrefab);

        CmdSpawnMyUnity();
    }

    public GameObject PlayerUnitPrefab;

    // Update is called once per frame
    void Update()
    {

    }

    ///////////////////////////// COMMANDS
    /// Commands are special functions that ONLY get executed on the server
    /// 

    [Command]
    void CmdSpawnMyUnity() {
        // this code is on the server.
        GameObject go = Instantiate(PlayerUnitPrefab);

        // now that the object exists on the server, propagate it to all the clients
        // (and wire up the NetworkIdentity)

        //NetworkServer.Spawn(go);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
