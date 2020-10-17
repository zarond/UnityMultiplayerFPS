using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLifeCycle : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rgbody;
    public GameObject Impact;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        rgbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(Impact,transform.position,transform.rotation);
        Debug.Log("Hit");
        Destroy(this.gameObject);
    }
}
