using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetUpPlayerUnit : NetworkBehaviour
{
    private int alreadyenabled = 0;
    public List<Behaviour> ComponentsToDisable = new List<Behaviour>();
    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Log(hasAuthority);
        if (!hasAuthority) {
            GetComponent<MovementRigidBody>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hasAuthority);
        //Debug.Log(isLocalPlayer);
        /*
        if (!hasAuthority)
        {
            alreadydisabled = true;
            GetComponent<MovementRigidBody>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }
        */
        if (alreadyenabled == 1) {  // костыль
            for (int i = 0; i < ComponentsToDisable.Count; ++i) {
                ComponentsToDisable[i].enabled = hasAuthority;
            }
            //alreadyenabled = 2;
        }
        ++alreadyenabled;

        //GetComponent<MovementRigidBody>().enabled = hasAuthority;
        //GetComponentInChildren<Camera>().enabled = hasAuthority;

    }
}
