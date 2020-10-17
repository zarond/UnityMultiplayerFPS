using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystemTrigger : MonoBehaviour
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
        if (other.CompareTag("PickUp") && other.gameObject.GetComponent<PickUpTrigger>().PickUpCriteria(this)) {
            other.gameObject.GetComponent<PickUpTrigger>().PickUpObject(this);
        }
    }


    private void FixedUpdate()
    {
    }
}
