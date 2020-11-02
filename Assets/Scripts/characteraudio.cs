using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class characteraudio : MonoBehaviour
{
    public AudioClip[] steps;
    public AudioClip jump;
    public AnimationCurve volumestepscurve;
    AudioSource source;

    MovementRigidBody mov;
    WeaponHolder wep;
    Animator anim;
    float maxspd;
    float minspd;
    public float[] stepstime;
    //private bool[] stepsbool= new bool[2];
    int s = 0;

    int c = 0;
    float jumpedlasttime = 0.0f; // чтобы звук прыжка не спамился
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        mov = GetComponent<MovementRigidBody>();
        wep = GetComponentInChildren<WeaponHolder>();
        anim = GetComponentInChildren<Animator>();
        maxspd = mov.speed;
        maxspd = mov.RunSpeed;
        //InvokeRepeating("playstep", 0, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float time= anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        time = Mathf.Repeat(time, 1);
        //Debug.Log(time);
        if (time >= stepstime[s] && (time - stepstime[s])<0.4f) 
        { 
            if (mov.isGrounded) 
                playstep(); 
            s = (s+1) % stepstime.Length; 
        }
        if (mov.jump && mov.isGrounded) { 
            if (Time.time - jumpedlasttime>0.1f) playjump();
            jumpedlasttime = Time.time;
        }

    }

    void playstep() {
        //source.clip = steps[(++c)%steps.Length];
        //source.volume = 0.5f;
        //maxspd = mov.speed;
        //maxspd = mov.RunSpeed;
        //anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float vol = Vector3.ProjectOnPlane(mov.Character.velocity, Vector3.up).magnitude;
        vol = Mathf.Clamp(vol, 0, maxspd)/maxspd;
        vol = volumestepscurve.Evaluate(vol);//Mathf.Pow(vol,8);
        source.PlayOneShot(steps[(++c) % steps.Length],vol);
    }
    void playjump()
    {
        source.PlayOneShot(jump, 0.5f);
    }
}
