using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMode : MonoBehaviour
{
    public bool firstpersonmode = true;
    public SkinnedMeshRenderer[] firstpersonrig;
    public SkinnedMeshRenderer[] thirdpersonrig;
    // Start is called before the first frame update
    void Start()
    {
        if (firstpersonmode) ChangeMode(true);
        else ChangeMode(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // надо получше сделать
    void ChangeMode(bool ch) { 
        if (ch) {
            firstpersonmode = true;
            for (int i = 0; i < firstpersonrig.Length; ++i)
                firstpersonrig[i].enabled = true;
            for (int i = 0; i < thirdpersonrig.Length; ++i)
                thirdpersonrig[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
        else {
            firstpersonmode = false;
            for (int i = 0; i < firstpersonrig.Length; ++i)
                firstpersonrig[i].enabled = false;
            for (int i = 0; i < thirdpersonrig.Length; ++i)
                thirdpersonrig[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
