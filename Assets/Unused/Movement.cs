using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Movement : MonoBehaviour
{
    private CharacterController Character;
    private Vector2 input;
    private Vector2 MouseInput;
    public float speed = 2f;
    public Transform Cam;
    private Quaternion lastRotation;
    private Quaternion LookTarget;
    public float max_angle = 50;
    private float cam_rot_v = 0;
    //public float SmoothTime = 5f;
    public float JumpSpeed;
    public float RunSpeed;
    private float VerticalSpeed = 0;
    private Vector3 selfSpeed = new Vector3(0,0,0);
    private bool jump = false;
    [Range(0f, 1f)]
    public float AirBourneControlIntensity;
    //
    public Vector3 MoveField = new Vector3(0, 0, 0);
    public float agility = 50.0f;
    public enum mode { zero, first, first_fps_safe ,second, third };
    public enum aircontrolmode { fps_safe, fps_safe2, not_fps_safe, steady_jump, no_jetpack };
    public mode mymode;
    public aircontrolmode aircntrlmode;
    public bool groundcorrection;
    public float downforce;
    public bool isGrounded { get; private set; }
    private SphereCollider legs;
    private float timer = 0;
    public float timetochangeState;
    //
    //public float spd = 0.0f;//public only for debug purposes
    public float spd { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<CharacterController>();
        legs = GetComponent<SphereCollider>();
        Cam = Camera.main.transform;
        lastRotation = transform.rotation;
        LookTarget = Quaternion.Euler(0f, 0f, 0f);
        //
        //mymode = mode.first;
        Cursor.lockState = CursorLockMode.Locked;//пока что засуну сюда
    }

    // Update is called once per frame
    void Update()
    {
        //input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 ToCam = Cam.forward;
        //Vector3 MoveTo = Quaternion.LookRotation(Vector3.ProjectOnPlane(ToCam, Vector3.up), Vector3.up) * (new Vector3(input.x, 0, input.y));
        Vector3 MoveTo = transform.TransformDirection(new Vector3(input.x, 0, input.y));
        MoveTo = Vector3.ClampMagnitude(MoveTo, 1.0f);

        cam_rot_v -= MouseInput.y;
        if (cam_rot_v > max_angle)
            cam_rot_v = max_angle;
        else if (cam_rot_v < -max_angle)
            cam_rot_v = -max_angle;


        lastRotation *= Quaternion.Euler(0, MouseInput.x, 0);
        LookTarget = Quaternion.Euler(cam_rot_v, 0, 0);
        Cam.localRotation = LookTarget;
        transform.rotation = lastRotation;


        jump = (Input.GetKey("space") || Input.GetKeyDown("space"));
        /*float*/ spd = (Input.GetKey(KeyCode.LeftShift)) ? RunSpeed : speed; 
       // isGrounded = Character.isGrounded;

        RaycastHit hit;
        Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit);
        //Physics.CapsuleCast(transform.position, transform.position, 0.3f, Vector3.down, out hit);
        //Physics.Raycast(transform.position, Vector3.down, out hit);
        Debug.DrawRay(hit.point, hit.normal);
        Quaternion Correction = Quaternion.FromToRotation(Vector3.up, hit.normal);
        //Quaternion Correction = Quaternion.FromToRotation(Vector3.up, (Vector3.Angle(Vector3.up, hit.normal)<=Character.slopeLimit)? hit.normal: (Vector3.up * Mathf.Cos(Character.slopeLimit) + Mathf.Sin(Character.slopeLimit) * (-Vector3.Dot(Vector3.up,hit.normal) * Vector3.up + hit.normal).normalized));

        //isGrounded = (hit.distance <= 0.99f);

        if (Character.isGrounded) timer = timetochangeState;
        else timer -= Time.deltaTime;
        if (timer <= .0f) isGrounded = false; else isGrounded = true;

        Debug.Log(hit.distance);

        if (!isGrounded) {
            Correction = Quaternion.identity;
        }


        //Debug.DrawRay(hit.point, Correction * transform.forward);
        //Debug.Log(Character.enableOverlapRecovery);
        //Character.enableOverlapRecovery = false;

        if (mymode == mode.zero)
        {
            //MoveField = MoveTo;
            if (aircntrlmode == aircontrolmode.not_fps_safe) {
                MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe) {
                MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity);
            }
            if (aircntrlmode == aircontrolmode.steady_jump)
            {
                MoveField = (isGrounded ? 0.0f : 1.0f) * selfSpeed +  MoveTo * 6.0f *  (isGrounded ? 1.0f : AirBourneControlIntensity);
            }

            if (isGrounded)
            {
                selfSpeed = MoveField; 
                //VerticalSpeed = downforce;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (VerticalSpeed < downforce) VerticalSpeed = downforce;
                if (jump) {
                    VerticalSpeed = JumpSpeed;
                }
            }
            //else
            { 
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            if (!groundcorrection) {
               Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            } else {
                Vector3 corrected = Correction * MoveField;
                if (corrected.z >= 0.0f) corrected = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
                Character.Move((corrected /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }

        }
        if (mymode == mode.first)  {    // этот режим зависит от фреймрейта
            //MoveField += agility * MoveTo * Time.deltaTime;
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                MoveField = Vector3.Lerp(MoveField / 0.9f, MoveField + agility * MoveTo * Time.deltaTime, (isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * MoveTo * Time.deltaTime, (isGrounded) ? 1.0f : AirBourneControlIntensity);
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                MoveField += agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * MoveTo * Time.deltaTime;
            }
            //MoveField = Vector3.ClampMagnitude(MoveField, spd);
            MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Lerp(selfSpeed.magnitude,spd, (isGrounded) ? 1.0f : AirBourneControlIntensity));

            if (isGrounded)
            {
                selfSpeed = MoveField;
                //VerticalSpeed = downforce;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (VerticalSpeed < downforce) VerticalSpeed = downforce;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            //else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }

            if (!groundcorrection)
            {
                Character.Move((MoveField + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            else
            {
                Vector3 corrected = Correction * MoveField;
                if (corrected.z >= 0.0f) corrected = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
                Character.Move((corrected /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            MoveField = 0.9f * MoveField;   // эта строка зависит от фреймрейта
        }
        if (mymode == mode.first_fps_safe)
        {    // этот режим не должен зависеть от фреймрейта
            //MoveField += agility * MoveTo * Time.deltaTime;
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                MoveField = Vector3.Lerp(MoveField / Mathf.Pow(0.9f, Time.deltaTime / 0.0125f), MoveField + agility * MoveTo * Time.deltaTime, (isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * MoveTo * Time.deltaTime, (isGrounded) ? 1.0f : AirBourneControlIntensity);
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                MoveField += agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * MoveTo * Time.deltaTime;
            }
            //MoveField = Vector3.ClampMagnitude(MoveField, spd);
            MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Lerp(selfSpeed.magnitude, spd, (isGrounded) ? 1.0f : AirBourneControlIntensity));

            if (isGrounded)
            {
                selfSpeed = MoveField;
                //VerticalSpeed = downforce;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (VerticalSpeed < downforce) VerticalSpeed = downforce;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            //else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }

            if (!groundcorrection)
            {
                Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            else
            {
                Vector3 corrected = Correction * MoveField;
                if (corrected.z >= 0.0f) corrected = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
                Character.Move((corrected /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            //MoveField = 0.9f * MoveField;
            MoveField = Mathf.Pow(0.9f,Time.deltaTime/0.0125f) * MoveField;
        }
        if (mymode == mode.second) 
        {
            /*
            if (1.0f <= agility * Time.deltaTime)
            {
                MoveField = MoveTo * spd;
            }
            else
            {
                MoveField = MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField));
            }
            */
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                if (1.0f <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(MoveField, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField)), (isGrounded) ? 1.0f : AirBourneControlIntensity);
                } // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                if (1.0f <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField)), (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                if (1.0f <= agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime)
                {
                    MoveField = MoveTo * spd;
                }
                else
                {
                    MoveField = MoveField + agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime * ((MoveTo * spd - MoveField));
                }
            }

            if (isGrounded)
            {
                selfSpeed = MoveField;
                //VerticalSpeed = downforce;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (VerticalSpeed < downforce) VerticalSpeed = downforce;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            //else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            if (!groundcorrection)
            {
                Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            else
            {
                Vector3 corrected = Correction * MoveField;
                if (corrected.z >= 0.0f) corrected = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
                Character.Move((corrected /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
        }
        if (mymode == mode.third)
        {
            /*
            if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime) {
                MoveField = MoveTo * spd;
            }
            else
            {
                MoveField = MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized);
            }
            */
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(MoveField, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized), (isGrounded) ? 1.0f : AirBourneControlIntensity);
                } // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized), (isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                if ((MoveTo * spd - MoveField).magnitude <= agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime)
                {
                    MoveField = MoveTo * spd;
                }
                else
                {
                    MoveField = MoveField + agility * ((isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime * ((MoveTo * spd - MoveField).normalized);
                }
            }
            if (aircntrlmode == aircontrolmode.steady_jump)
            {
                if (isGrounded) {
                    if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime)
                    {
                        MoveField = MoveTo * spd;
                    }
                    else
                    {
                        MoveField = MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized);
                    }
                } else {
                    MoveField = MoveField + agility * AirBourneControlIntensity * MoveTo * Time.deltaTime;
                }

            }
            if (aircntrlmode == aircontrolmode.no_jetpack)
            {
                if (isGrounded)
                {
                    if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime)
                    {
                        MoveField = MoveTo * spd;
                    }
                    else
                    {
                        MoveField = MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized);
                        MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Max(speed, RunSpeed));
                    }
                }
                else
                {
                    if (VerticalSpeed >= 0.0f) {                
                            MoveField = MoveField + agility * AirBourneControlIntensity * MoveTo * Time.deltaTime;
                            MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Max(speed, RunSpeed));
                    }
                    else {
                        MoveField = MoveField;
                    }
                }

            }

            if (isGrounded)
            {
                selfSpeed = MoveField;
                //VerticalSpeed = downforce;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (VerticalSpeed < downforce) VerticalSpeed = downforce;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            //else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            if (!groundcorrection)
            {
                Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
            else
            {
                Vector3 corrected = Correction * MoveField;
                if (corrected.z >= 0.0f) corrected = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
                Character.Move((corrected /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
            }
        }


        //Debug.Log(Character.velocity.magnitude);
        Debug.DrawRay(hit.point, Correction * MoveField);

        /*RaycastHit hit;
        Physics.SphereCast(transform.position, 1.0f, Vector3.down, out hit);
        Debug.DrawRay(hit.point,hit.normal);*/

        //Debug.Log(isGrounded);
        //Debug.Log(MoveTo.magnitude);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.red);
            Debug.Log("ddd");
        }
    }
}
