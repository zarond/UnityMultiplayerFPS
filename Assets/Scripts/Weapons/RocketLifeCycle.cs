using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLifeCycle : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rgbody;
    public GameObject Impact;
    bool alreadyhit = false;
    public GameObject owner;
    public int ownerid;
    //public bool canhurtplayer = true;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        rgbody = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 10f); // удалить объект если он не попал никуда в течении 10 секунд
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!alreadyhit)
        {
            alreadyhit = true;
            GameObject tmp = Instantiate(Impact, transform.position, transform.rotation);
            tmp.GetComponent<RocketImpact>().owner = this.owner; // задать принадлежность взрыва, может быть ошибка при отложенном попадании
            tmp.GetComponent<RocketImpact>().ownerid = this.ownerid; // задать принадлежность взрыва
            //Debug.Log("Hit");
            Destroy(this.gameObject);
        }
    }

    // если попал прямо в персонажа, но не в себя
    private void OnTriggerEnter(Collider collider1)
    {
        //Debug.Log("On Trigger");
        if (collider1.transform.root.gameObject!=owner && collider1.gameObject.layer == 10)
        {
            //Debug.Log("Hit directly");
            OnCollisionEnter(new Collision());
        }
    }
}
