using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject prefab;
    public int numberofobjectperpress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            for (int i = 0; i < numberofobjectperpress; ++i) {
                Instantiate(prefab, new Vector3(Random.value * 100, Random.value*100, Random.value*100), Quaternion.identity);
            }

        }
    }
}
