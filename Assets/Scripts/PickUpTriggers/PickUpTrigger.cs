using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    virtual public void PickUpObject(PickUpSystemTrigger WhoPicked) { }
    /*
    virtual public bool DistanceCriteria(PickUpSystemTrigger WhoPicked)
    {
        return false;
    }
    */
    virtual public bool PickUpCriteria(PickUpSystemTrigger WhoPicked)
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
