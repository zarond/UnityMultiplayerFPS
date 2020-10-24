using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketImpact : MonoBehaviour
{
    //public SphereCollider trigger;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Boom");
        Collider[] coll = Physics.OverlapSphere(transform.position, 3.0f);
        for (int i = 0; i < coll.Length; ++i) {
            coll[i].SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
        }
        //Invoke("Destr",1.0f);

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    private void Destr()
    {
        Destroy(this.gameObject);
    }
    */

}
