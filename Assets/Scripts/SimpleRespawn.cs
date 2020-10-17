using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRespawn : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform Origin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            GameObject pl = GameObject.FindWithTag("Player");
            Destroy(pl);
            Instantiate(PlayerPrefab,Origin.position,Origin.rotation);

        }
    }
}
