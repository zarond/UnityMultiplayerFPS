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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn(GameObject.FindWithTag("Player"));
        }
    }

    public void Respawn(GameObject pl) {
        if (pl != null) Destroy(pl);
        Instantiate(PlayerPrefab, Origin.position, Origin.rotation); // надо сделать чтобы оно могло респавнить не только игрока
    }
}
