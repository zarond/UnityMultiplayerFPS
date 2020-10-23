using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    virtual public void PickUpObject(PickUpSystem WhoPicked) { }
    /*
    virtual public bool DistanceCriteria(PickUpSystemTrigger WhoPicked)
    {
        return false;
    }
    */
    virtual public bool PickUpCriteria(PickUpSystem WhoPicked)
    {
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
