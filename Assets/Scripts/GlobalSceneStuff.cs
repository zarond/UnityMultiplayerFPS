using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSceneStuff : MonoBehaviour
{
    public List<PickUp> PickUps = new List<PickUp>();
    // Start is called before the first frame update
    void Start()
    {
        //UpdatePickUpList()
    }

    void Awake()
    {
        //UpdatePickUpList();
    }

    public void UpdatePickUpList() {
        PickUps.Clear();
        PickUps.AddRange(FindObjectsOfType<PickUp>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
