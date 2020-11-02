using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("here");
        if (other.CompareTag("PickUp") && other.gameObject.GetComponent<PickUp>().PickUpCriteria(this)) {
            other.gameObject.GetComponent<PickUp>().PickUpObject(this);
            //Debug.Log("there");
        }
    }

}
