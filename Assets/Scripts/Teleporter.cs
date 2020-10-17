using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject Destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (transform.InverseTransformVector(other.attachedRigidbody.velocity).y<=0.01f)) {
            Debug.Log("Teleport");
            other.transform.position = Destination.transform.position;
            //Quaternion Rotation = Destination.transform.rotation * Quaternion.Inverse(transform.rotation);
            //other.transform.rotation *= Quaternion.AngleAxis(180f,Destination.transform.forward) * Rotation;
            //other.attachedRigidbody.velocity = Quaternion.AngleAxis(180f, Destination.transform.forward) * Rotation * (other.attachedRigidbody.velocity);

            Quaternion Rotation = Destination.transform.rotation * Quaternion.Inverse(transform.rotation);
            //other.transform.rotation *= Rotation;
            other.transform.rotation = Rotation * other.transform.rotation * Quaternion.AngleAxis(180f, other.transform.up);
            other.attachedRigidbody.velocity = Quaternion.AngleAxis(180f, Destination.transform.forward) * Rotation * (other.attachedRigidbody.velocity);


            //other.attachedRigidbody.velocity = Vector3.Reflect(other.attachedRigidbody.velocity,Destination.transform.up);
        }
    }
}
