using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    public int id { get; protected set; }
    [SerializeField]
    public string name { get; protected set; }
    [SerializeField]
    public bool automatic { get; protected set; }
    [SerializeField]
    public float fire_rate { get; protected set; }
//    [SerializeField]
//    public float maxAmmo { get; protected set; }    // не уверен что этот параметр должен быть в классе пушки
    [SerializeField]
    public bool TransferVelocity { get; protected set; }
    [SerializeField]
    public float lifeDuration { get; protected set; }
    [SerializeField]
    public float speed { get; protected set; }
    /*([SerializeField]
    public float damage { get; private set; }
    [SerializeField]
    public float splashRadius { get; private set; }
    [SerializeField]
    public float shockwave { get; private set; }
    [SerializeField]
    public bool jet { get; private set; }*/
    [SerializeField]
    public GameObject bulletPrefab { get; set; }
    [SerializeField]
    public int slot { get; protected set; }
    [SerializeField]
    public float TimeToPressTrigger { get; protected set; } = 0.0f;

    private float timer = 0.0f;
    private float shotLastTime = 0.0f;
    private int ShotsInARow = 0;

    public Transform BarrelEnd;
    public Transform leftHandPoint;
    public Transform rightHandPoint;
    public enum states { idle, triggerpress, firing, empty, reloading };
    public GameObject owner;
    public int ownerid;

    public Weapon() {
        //timer = -TimeToPressTrigger;
    }

    private void setuptimer() {
        timer = 0.0f;
        ShotsInARow = 0;
    }

    public bool TryShoot(bool down, bool hold, bool up) {
        bool res = false;
        //if (down) { setuptimer(); }
        if (down && (Time.time - shotLastTime >= fire_rate)) { setuptimer(); }
        if (hold && ((timer - TimeToPressTrigger) >= 0.0f) && (ShotsInARow == 0 || automatic)) { 
            Shoot();
            shotLastTime = Time.time; // возможно лучше так не делать, потому что чем дальше, тем меньше точность float
            timer -= fire_rate;
            ShotsInARow += 1;
            res = true; 
        }

        timer += Time.deltaTime; // ? либо fixedDeltaTime
        return res; // не выстрелил
    }

    virtual public void Shoot() {
        Debug.Log("shot");
    }

}
