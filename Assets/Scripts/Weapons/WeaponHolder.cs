using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();
    //public List<Component> Weapons = new List<Component>();
    //public Weapon[] Weapons;// = new Weapon[6];
    public bool[] Equiped = new bool[Constants.GlobalNumberOfWeapons];
    public int[] Ammo = new int[Constants.GlobalNumberOfWeapons];
    public int[] maxAmmo = new int[Constants.GlobalNumberOfWeapons];
    int ActiveWeapon = 0;
    int ActiveSlot = 0;
    bool receivedWeaponChange = false;

    // Start is called before the first frame update
    void Start()
    {
        //Weapons[0] = new Pistol();
        //Weapons[1] = new RocketLauncher();

        //Weapons.Add(new Pistol());
        //Weapons.Add(new RocketLauncher());

        /*
        Equiped[0] = true;
        Equiped[1] = true;
        Ammo[0] = 10;
        Ammo[1] = 10;
        maxAmmo[0] = 50;
        maxAmmo[1] = 10;
        */
        for (int i = 0; i < Weapons.Count; ++i) Weapons[i].gameObject.SetActive(false);
        Weapons[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        receivedWeaponChange = true;

        if (Input.GetKeyDown(KeyCode.Alpha0)) ActiveSlot = 0; else
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActiveSlot = 1; else
        if (Input.GetKeyDown(KeyCode.Alpha2)) ActiveSlot = 2; else
        if (Input.GetKeyDown(KeyCode.Alpha3)) ActiveSlot = 3; else
        if (Input.GetKeyDown(KeyCode.Alpha4)) ActiveSlot = 4; else
        if (Input.GetKeyDown(KeyCode.Alpha5)) ActiveSlot = 5; else
        if (Input.GetKeyDown(KeyCode.Alpha6)) ActiveSlot = 6; else
        if (Input.GetKeyDown(KeyCode.Alpha7)) ActiveSlot = 7; else
        if (Input.GetKeyDown(KeyCode.Alpha8)) ActiveSlot = 8; else
        if (Input.GetKeyDown(KeyCode.Alpha9)) ActiveSlot = 9; else
            receivedWeaponChange = false;

        if (receivedWeaponChange) {
            for (int i = (ActiveWeapon + 1) % (Weapons.Count), c = 0; (c <= Weapons.Count) ; i = (i+1) % (Weapons.Count), ++c) {
                if (Weapons[i].slot == ActiveSlot && Equiped[Weapons[i].id]) {
                    ActiveWeapon = i;
                    break;
                }
            }

            for (int i=0;  i < Weapons.Count ;++i) Weapons[i].gameObject.SetActive(false);
            Weapons[ActiveWeapon].gameObject.SetActive(true);

        }
        //ActiveWeapon = 

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            //Weapons[ActiveWeapon].TryShoot(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetMouseButtonUp(0));
            if (Ammo[Weapons[ActiveWeapon].id] > 0)
            {
                bool shot = Weapons[ActiveWeapon].TryShoot(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetMouseButtonUp(0));
                if (shot) Ammo[Weapons[ActiveWeapon].id]--;
            }
        }

    }
}
