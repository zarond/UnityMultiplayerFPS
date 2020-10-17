using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Movement_copy : MonoBehaviour
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
    public float SmoothTime = 5f;
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
    public enum aircontrolmode { fps_safe, fps_safe2, not_fps_safe };
    public mode mymode;
    public aircontrolmode aircntrlmode;
    //
    public float spd = 0.0f;//public only for debug purposes

    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<CharacterController>();
        Cam = Camera.main.transform;
        lastRotation = transform.rotation;
        LookTarget = Quaternion.Euler(0f, 0f, 0f);
        //
        //mymode = mode.first;
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

        if (mymode == mode.zero)
        {
            //MoveField = MoveTo;
            if (aircntrlmode == aircontrolmode.not_fps_safe) {
                MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe) {
                MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
            }

            if (Character.isGrounded)
            {
                selfSpeed = MoveField; 
                VerticalSpeed = -0.1f;  //отрицательное значение потому что иначе не может правильно определить isGrounded
                if (jump) {
                    VerticalSpeed = JumpSpeed;
                }
            }
            else
            { 
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
        }
        if (mymode == mode.first)  {    // этот режим зависит от фреймрейта
            //MoveField += agility * MoveTo * Time.deltaTime;
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                MoveField = Vector3.Lerp(MoveField / 0.9f, MoveField + agility * MoveTo * Time.deltaTime, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * MoveTo * Time.deltaTime, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                MoveField += agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * MoveTo * Time.deltaTime;
            }
            //MoveField = Vector3.ClampMagnitude(MoveField, spd);
            MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Lerp(selfSpeed.magnitude,spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity));

            if (Character.isGrounded)
            {
                selfSpeed = MoveField;
                VerticalSpeed = -0.1f;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }

            Character.Move((MoveField + VerticalSpeed * Vector3.up) * Time.deltaTime);
            MoveField = 0.9f * MoveField;   // эта строка зависит от фреймрейта
        }
        if (mymode == mode.first_fps_safe)
        {    // этот режим не должен зависеть от фреймрейта
            //MoveField += agility * MoveTo * Time.deltaTime;
            if (aircntrlmode == aircontrolmode.not_fps_safe)
            {
                MoveField = Vector3.Lerp(MoveField / Mathf.Pow(0.9f, Time.deltaTime / 0.0125f), MoveField + agility * MoveTo * Time.deltaTime, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity); // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * MoveTo * Time.deltaTime, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                MoveField += agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * MoveTo * Time.deltaTime;
            }
            //MoveField = Vector3.ClampMagnitude(MoveField, spd);
            MoveField = Vector3.ClampMagnitude(MoveField, Mathf.Lerp(selfSpeed.magnitude, spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity));

            if (Character.isGrounded)
            {
                selfSpeed = MoveField;
                VerticalSpeed = -0.1f;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }

            Character.Move((MoveField + VerticalSpeed * Vector3.up) * Time.deltaTime);
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
                    MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(MoveField, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField)), (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                } // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                if (1.0f <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField)), (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                if (1.0f <= agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime)
                {
                    MoveField = MoveTo * spd;
                }
                else
                {
                    MoveField = MoveField + agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime * ((MoveTo * spd - MoveField));
                }
            }

            if (Character.isGrounded)
            {
                selfSpeed = MoveField;
                VerticalSpeed = -0.1f;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            Character.Move((MoveField /** spd*/ + VerticalSpeed * Vector3.up) * Time.deltaTime);
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
                    MoveField = Vector3.Lerp(MoveField, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(MoveField, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized), (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                } // эта строка зависит от фреймрейта
            }
            if (aircntrlmode == aircontrolmode.fps_safe)
            {
                if ((MoveTo * spd - MoveField).magnitude <= agility * Time.deltaTime)
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveTo * spd, (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
                else
                {
                    MoveField = Vector3.Lerp(selfSpeed, MoveField + agility * Time.deltaTime * ((MoveTo * spd - MoveField).normalized), (Character.isGrounded) ? 1.0f : AirBourneControlIntensity);
                }
            }
            if (aircntrlmode == aircontrolmode.fps_safe2)
            {
                if ((MoveTo * spd - MoveField).magnitude <= agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime)
                {
                    MoveField = MoveTo * spd;
                }
                else
                {
                    MoveField = MoveField + agility * ((Character.isGrounded) ? 1.0f : AirBourneControlIntensity) * Time.deltaTime * ((MoveTo * spd - MoveField).normalized);
                }
            }

            if (Character.isGrounded)
            {
                selfSpeed = MoveField;
                VerticalSpeed = -0.1f;
                if (jump)
                {
                    VerticalSpeed = JumpSpeed;
                }
            }
            else
            {
                VerticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            Character.Move((MoveField + VerticalSpeed * Vector3.up) * Time.deltaTime);
        }


        Debug.Log(Character.velocity.magnitude);
        //Debug.Log(Character.isGrounded);
        //Debug.Log(MoveTo.magnitude);
    }
}
