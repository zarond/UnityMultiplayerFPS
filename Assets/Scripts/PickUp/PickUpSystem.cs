using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// этот компонент висит на игроке и пытается подобрать пикапы

public class PickUpSystem : MonoBehaviour
{
    //public List<PickUp> PickUps = new List<PickUp>();
    public GlobalSceneStuff Global;
    // Start is called before the first frame update
    void Start()
    {
        Global = FindObjectOfType<GlobalSceneStuff>();
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        for (int i=0; i < Global.PickUps.Count; ++i) {
            if (Global.PickUps[i].DistanceCriteria(this) && Global.PickUps[i].PickUpCriteria(this)) {
                Global.PickUps[i].PickUpObject(this);
            }
        }
    }
}
