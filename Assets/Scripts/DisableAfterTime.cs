using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float time=0.0f;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("hide", time);
        //gameObject.SetActive(false)        
    }
    void hide() { gameObject.SetActive(false); }

}
