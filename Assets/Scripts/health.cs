using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// здоровье и информация, в какой команде игрок
public class health : MonoBehaviour
{
    public int teamid = 0;

    public float hp;
    public bool DmgNumbers = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) Destroy (this.gameObject);
    }

    //void DoDamage(float damage, GameObject whoDamaged = null)
    void DoDamage(object[] obj)
    {
        float damage;
        GameObject whoDamaged=null;
        try {
            damage = (float)obj.GetValue(0);
            whoDamaged = (GameObject)obj.GetValue(1);
        } catch { return; }

        //if (teamid == whoDamaged.GetComponent<health>().teamid) return; //friendlyfire off, надо добавить настройку и ракетница все-таки должна дамажить себя

        hp -= damage;
        if (DmgNumbers) {
            Debug.Log(this.name + " got damaged by "+damage+"hp, by "+whoDamaged+", "+hp+"hp left");
            //GameObject tmp = new GameObject("DamageText");
            //ui = tmp.AddComponent<>();
            //ui. = "Whatever";
        }
    }
}
