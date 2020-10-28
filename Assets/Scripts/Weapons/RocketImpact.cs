using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketImpact : MonoBehaviour
{
    public GameObject owner;
    //public SphereCollider trigger;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Boom");
        int layer = (1 << 10) | (1<<12); // маска слоя триггера-хитбокса (чтобы не учитывать обычные объекты)
        Collider[] coll = Physics.OverlapSphere(transform.position, 3.0f, layer); //если на объекте 2 коллайднра, то он получает дамаг дважды, надо исправить
        //yourList = yourList.Distinct().ToList();
        List<Transform> list = new List<Transform>(); 

        for (int i = 0; i < coll.Length; ++i) {
            //list[i] = coll[i].transform.root;
            list.Insert(0,coll[i].transform.root);
            //coll[i].gameObject.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
            //coll[i].SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver); // было
            //coll[i].transform.root.SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver); // стало, потому что health ставлю на родителя, а хитбоксы - на детях.
        }
        list = list.Distinct().ToList();
        for (int i = 0; i < list.Count; ++i)
        {
            //list[i].SendMessage("DoDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
            list[i].SendMessage("DoDamage", new object[2] { 1.0f, this.owner }, SendMessageOptions.DontRequireReceiver);
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
