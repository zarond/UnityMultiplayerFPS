using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//это базовый класс для пикапов

public class PickUp : MonoBehaviour
{
    virtual public void PickUpObject(PickUpSystem WhoPicked) { }

    virtual public bool DistanceCriteria(PickUpSystem WhoPicked) {
        return false;
    }

    virtual public bool PickUpCriteria(PickUpSystem WhoPicked) {
        return false;
    }

    public PickUp() {
        // необходимо при спавне новых пикапов стучаться и обновлять список пикапов в global scene stuff
        // не уверен пока как это делать
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GlobalSceneStuff>().PickUps.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
