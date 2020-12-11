using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class WeaponHolder : MonoBehaviour, IPunObservable
{
    public List<Weapon> Weapons = new List<Weapon>();
    //public List<Component> Weapons = new List<Component>();
    //public Weapon[] Weapons;// = new Weapon[6];
    public bool[] Equiped = new bool[Constants.GlobalNumberOfWeapons];
    public int[] Ammo = new int[Constants.GlobalNumberOfWeapons];
    public int[] maxAmmo = new int[Constants.GlobalNumberOfWeapons];
    //[HideInInspector]
    public int ActiveWeapon { get; private set; } = 0 ;
    //[HideInInspector]
    public int ActiveSlot = 0;
    [HideInInspector]
    public bool receivedWeaponChange = false;
    [HideInInspector]
    public Transform leftHandPoint;
    [HideInInspector]
    public Transform rightHandPoint;
    public bool WeaponPosCorrection;
    public Transform WeaponHolderObject;
    public Transform ChestBone;
    RaycastHit RayCast;
    //[SerializeField]
    //Camera Cam;
    [HideInInspector]
    public bool[] shootStates = new bool[3];

    Vector3 originalPosWeapon;

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonView.Get(this).IsMine)
            {
                //Debug.Log("send data about weapon change" + receivedWeaponChange);
                stream.SendNext(ActiveSlot);
                stream.SendNext(receivedWeaponChange);
            }
            // We own this player: send the others our data
        }
        else
        {
            if (PhotonView.Get(this).IsMine == false)
            {
                //ActiveSlot = (int)stream.ReceiveNext();
                int tmp = (int)stream.ReceiveNext();
                receivedWeaponChange = (bool)stream.ReceiveNext();
                if (tmp != ActiveSlot) receivedWeaponChange = true;
                ActiveSlot = tmp;
                //Debug.Log("get data about weapon change" + receivedWeaponChange);
            }
            // Network player, receive data

        }
    }
    #endregion

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
        leftHandPoint = Weapons[0].leftHandPoint;
        rightHandPoint = Weapons[0].rightHandPoint;
        originalPosWeapon = WeaponHolderObject.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponPosCorrection) {
           //WeaponHolderObject.position = transform.TransformVector(transform.localRotation * (0.6f * Vector3.forward))+ChestBone.position;
            WeaponHolderObject.position = transform.forward*0.5f + ChestBone.position+transform.right*0.2f;
        }
        else {
            WeaponHolderObject.localPosition = originalPosWeapon;
        }
        /*
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
        */
        if (receivedWeaponChange) {
            for (int i = (ActiveWeapon + 1) % (Weapons.Count), c = 0; (c <= Weapons.Count) ; i = (i+1) % (Weapons.Count), ++c) {
                if (Weapons[i].slot == ActiveSlot && Equiped[Weapons[i].id]) {
                    ActiveWeapon = i;
                    break;
                }
            }

            for (int i=0;  i < Weapons.Count ;++i) Weapons[i].gameObject.SetActive(false);
            Weapons[ActiveWeapon].gameObject.SetActive(true);
            leftHandPoint = Weapons[ActiveWeapon].leftHandPoint;
            rightHandPoint = Weapons[ActiveWeapon].rightHandPoint;

            receivedWeaponChange = false; // new
            Debug.Log("weapon change");
        }

        if (PhotonView.Get(this).IsMine == false) return;

        //Ray ScreenVector = Cam.ScreenPointToRay(new Vector3(Screen.width *0.5f, Screen.height*0.5f,0.0f));
        int layer = ~(1 << 9); // маска слоя все кроме физического коллайдера персонажей
        Ray ScreenVector = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ScreenVector,out hit, Mathf.Infinity, layer);
        Physics.Raycast(Weapons[ActiveWeapon].BarrelEnd.position, (hit.point- Weapons[ActiveWeapon].BarrelEnd.position), out RayCast, Mathf.Infinity, layer);

        Debug.DrawRay(/*Cam.*/transform.position, ScreenVector.direction*10f,Color.yellow,0.0f,true);
        Debug.DrawRay(RayCast.point,RayCast.normal, Color.red);
        //ActiveWeapon = 

        //if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
        if (shootStates[0] || shootStates[1] || shootStates[2]) {
            //Weapons[ActiveWeapon].TryShoot(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetMouseButtonUp(0));
            if (Ammo[Weapons[ActiveWeapon].id] > 0)
            {
                bool shot = Weapons[ActiveWeapon].TryShoot(shootStates[0], shootStates[1], shootStates[2]);
                if (shot) Ammo[Weapons[ActiveWeapon].id]--;
            }
        } //по хорошему в отдельную функцию бы shootgun()

        //receivedWeaponChange = false; // new

    }
}
