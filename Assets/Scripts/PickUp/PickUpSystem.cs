using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && other.gameObject.GetComponent<PickUp>().PickUpCriteria(this)) {
            other.gameObject.GetComponent<PickUp>().PickUpObject(this);
        }
    }


    private void FixedUpdate()
    {
    }
}
