using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMode : MonoBehaviour
{
    public bool firstpersonmode = true;
    public SkinnedMeshRenderer[] firstpersonrig;
    public SkinnedMeshRenderer[] thirdpersonrig;
    WeaponHolder weapon;
    Vector3 originaloffset;
    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponentInChildren<WeaponHolder>();
        originaloffset = this.transform.GetChild(2).transform.localPosition;
        if (firstpersonmode) ChangeMode(true);
        else ChangeMode(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) ChangeMode(true);
        if (Input.GetKeyDown(KeyCode.T)) ChangeMode(false);
        //weapon.receivedWeaponChange;
        //weapon.Weapons[weapon.ActiveWeapon].gameObject.GetComponent<MeshRenderer>().mes;
    }
    // надо получше сделать
    void ChangeMode(bool ch) { 
        if (ch) {
            firstpersonmode = true;
            weapon.WeaponPosCorrection = false;
            //this.transform.GetChild(2).gameObject.SetActive(false); // надо чтобы только рендеринг отключался, а тригеры остались
            this.transform.GetChild(3).gameObject.SetActive(true);
            for (int i = 0; i < firstpersonrig.Length; ++i)
                firstpersonrig[i].enabled = true;
            for (int i = 0; i < thirdpersonrig.Length; ++i)
                thirdpersonrig[i].enabled = false;
        }
        else {
            firstpersonmode = false;
            //this.transform.GetChild(2).transform.localPosition = originaloffset;
            weapon.WeaponPosCorrection = true;
            for (int i = 0; i < firstpersonrig.Length; ++i)
                firstpersonrig[i].enabled = false;
            for (int i = 0; i < thirdpersonrig.Length; ++i)
                thirdpersonrig[i].enabled = true;
            //thirdpersonrig[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
