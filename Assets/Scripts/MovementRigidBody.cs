using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking; //

using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]

public class MovementRigidBody : MonoBehaviour, IPunObservable//NetworkBehaviour//MonoBehaviour
{
    public Rigidbody Character { get; private set; }
    private CapsuleCollider collider;
    [HideInInspector]
    public Vector2 input;
    [HideInInspector]
    public Vector2 MouseInput;
    public float speed = 2f;
    public Transform Cam;
    private Quaternion lastRotation;
    private Quaternion LookTarget;
    public float max_angle = 50;
    private float cam_rot_v = 0;
    //public float SmoothTime = 5f;
    public float JumpSpeed;
    public float RunSpeed;
    //private float VerticalSpeed = 0;
    private Vector3 selfSpeed = new Vector3(0, 0, 0);
    [HideInInspector]
    public bool jump = false;
    [Range(0f, 1f)]
    public float AirBourneControlIntensity;

    public Vector3 MoveField = new Vector3(0, 0, 0);
    public float agility = 50.0f;
    public enum mode { zero, const_acceleration, distance_acceleration };
    public enum aircontrolmode { none, steady_jump, no_jetpack, speed_depended };
    public mode mymode;
    public aircontrolmode aircntrlmode;
    public bool groundcorrection;
    public float downforce;
    private Quaternion Correction;
    //
    [HideInInspector]
    public float spd = 1.0f;
    //
    private Vector3 Final_Move;

    public bool isGrounded { get; private set; }

    void Awake() {
        photonView = GetComponent<PhotonView>();
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (photonView.IsMine)
            {
                //Debug.Log("send data about weapon change" + receivedWeaponChange);
                stream.SendNext(isGrounded);
                //stream.SendNext(jump);
            }
            // We own this player: send the others our data
        }
        else
        {
            if (photonView.IsMine == false)
            {
                //ActiveSlot = (int)stream.ReceiveNext();
                isGrounded = (bool)stream.ReceiveNext();
                //jump = (bool)stream.ReceiveNext();
                //Debug.Log("get data about weapon change" + receivedWeaponChange);
            }
            // Network player, receive data

        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        photonView = GetComponent<PhotonView>();

        Character = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        //Cam = Camera.main.transform;
        lastRotation = transform.rotation;
        LookTarget = Quaternion.Euler(0f, 0f, 0f);
        Cursor.lockState = CursorLockMode.Locked;//пока что засуну сюда
    }

    void SetLook()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) // Чтобы при повороте мыши поворачивался только свой персонаж
        {
            return;
        }

        //MouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 ToCam = Cam.forward;

        cam_rot_v -= MouseInput.y;
        if (cam_rot_v > max_angle)
            cam_rot_v = max_angle;
        else if (cam_rot_v < -max_angle)
            cam_rot_v = -max_angle;

        //lastRotation *= Quaternion.Euler(0, MouseInput.x, 0);
        LookTarget = Quaternion.Euler(cam_rot_v, 0, 0);
        Cam.localRotation = LookTarget;
        //transform.rotation = lastRotation;
        transform.rotation *= Quaternion.Euler(0, MouseInput.x, 0);
    }


    void RestoreBalance()
    {
        if (transform.up != Vector3.up) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(Cam.forward, Vector3.up),Vector3.up),1);
            //Debug.Log("Restoring balance");
        }
    }

    /*
    void WalkOver() {
        RaycastHit hit;
        Physics.SphereCast(transform.TransformPoint(new Vector3(0,2,1)), 0.35f, Vector3.down, out hit);
        Debug.DrawRay(hit.point, hit.normal,Color.yellow);
    }*/

    private void OnCollisionStay(Collision collision)
    {
        //isGrounded = false;
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
            //if (transform.InverseTransformPoint(contact.point).y <= -0.5f) isGrounded = true;
            if (photonView.IsMine)
                if (transform.InverseTransformPoint(contact.point).y < -(collider.height/2-collider.radius) && Vector3.Dot(contact.normal,transform.up)>0.5f) isGrounded = true;
        }
        
    }

    // Update is called once per frame
    void Update() {
        SetLook();
    }



    private PhotonView photonView;

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        RestoreBalance();

        //WalkOver();

        //input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 MoveTo = transform.TransformDirection(new Vector3(input.x, 0, input.y));
        MoveTo = Vector3.ClampMagnitude(MoveTo, 1.0f);
        //jump = (Input.GetKey("space") || Input.GetKeyDown("space"));
        //spd = (Input.GetKey(KeyCode.LeftShift)) ? RunSpeed : speed;

        //isGrounded = true;

        /*     //слишком дорого и не используется
        RaycastHit hit;
        Physics.SphereCast(transform.position+Vector3.up * 0.1f, 0.35f, Vector3.down, out hit);
        //Physics.CapsuleCast(transform.position, transform.position, 0.3f, Vector3.down, out hit);
        //Physics.Raycast(transform.position, Vector3.down, out hit);
        Debug.DrawRay(hit.point, hit.normal);
        Correction = Quaternion.FromToRotation(Vector3.up, hit.normal);
        //Quaternion Correction = Quaternion.FromToRotation(Vector3.up, (Vector3.Angle(Vector3.up, hit.normal)<=Character.slopeLimit)? hit.normal: (Vector3.up * Mathf.Cos(Character.slopeLimit) + Mathf.Sin(Character.slopeLimit) * (-Vector3.Dot(Vector3.up,hit.normal) * Vector3.up + hit.normal).normalized));
        */

        //isGrounded = (hit.distance <= 0.36f+0.1f);
        /*
        if (Character.isGrounded) timer = timetochangeState;
        else timer -= Time.deltaTime;
        if (timer <= .0f) isGrounded = false; else isGrounded = true;
        */
        //Debug.Log(hit.distance);

        if (!isGrounded)
        {
            Correction = Quaternion.identity;
        }

        //Debug.DrawRay(hit.point, Correction * transform.forward);
        MoveField = MoveTo * spd;

        if (groundcorrection) {
            Vector3 corrected = Correction * MoveField;
            if (corrected.z>=0) MoveField = MoveField * Vector3.Dot(MoveField.normalized, corrected.normalized);
            else MoveField = corrected;
        }


        if (mymode == mode.zero) { 
            //if (isGrounded) Character.velocity = MoveField + Vector3.up * Character.velocity.y;
            if (isGrounded) Character.AddForce((MoveField + Vector3.up * Character.velocity.y)
             - Character.velocity, ForceMode.VelocityChange);
        }
        if (mymode == mode.const_acceleration) {
            if (isGrounded)
            {
                if ((MoveField - Vector3.ProjectOnPlane(Character.velocity, Vector3.up)).magnitude <= agility * Time.fixedDeltaTime)
                {
                    //Character.velocity = MoveField + Vector3.up * Character.velocity.y;
                    Character.AddForce(MoveField + Vector3.up * Character.velocity.y
                        - Character.velocity, ForceMode.VelocityChange);
                }
                else //Character.velocity = Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * Time.fixedDeltaTime * (MoveField - Vector3.ProjectOnPlane(Character.velocity, Vector3.up)).normalized + Vector3.up * Character.velocity.y;
                    Character.AddForce(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * Time.fixedDeltaTime * (MoveField - Vector3.ProjectOnPlane(Character.velocity, Vector3.up)).normalized + Vector3.up * Character.velocity.y
                        - Character.velocity, ForceMode.VelocityChange);
            }
        }
        if (mymode == mode.distance_acceleration) {
            if (isGrounded)
            {
                if (1.0f <= agility * Time.fixedDeltaTime)
                {
                    //Character.velocity = MoveField + Vector3.up * Character.velocity.y;
                    Character.AddForce(MoveField + Vector3.up * Character.velocity.y
                    -Character.velocity, ForceMode.VelocityChange);
                }
                else //Character.velocity = Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * Time.fixedDeltaTime * (MoveField - Vector3.ProjectOnPlane(Character.velocity, Vector3.up)) + Vector3.up * Character.velocity.y;
                    Character.AddForce(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * Time.fixedDeltaTime * (MoveField - Vector3.ProjectOnPlane(Character.velocity, Vector3.up)) + Vector3.up * Character.velocity.y
                        - Character.velocity, ForceMode.VelocityChange);
            }
        }

        if (!isGrounded) {
            if (aircntrlmode == aircontrolmode.none) { }
            if (aircntrlmode == aircontrolmode.steady_jump) {
                //Character.velocity = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y;
                Character.AddForce(Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y
                    - Character.velocity, ForceMode.VelocityChange);
            }
            if (aircntrlmode == aircontrolmode.no_jetpack) {
                if (Character.velocity.y >= 0.0f)
                {
                    //Character.velocity = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y;
                    Character.AddForce(Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y
                    - Character.velocity, ForceMode.VelocityChange);
                }
            }
            if (aircntrlmode == aircontrolmode.speed_depended)
            {
                float multiplier = Mathf.Exp(-(Character.velocity.y) * (Character.velocity.y)/2.0f);    // нормальное распределение лул
                //Character.velocity = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + multiplier * agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y;
                Character.AddForce(Vector3.ClampMagnitude(Vector3.ProjectOnPlane(Character.velocity, Vector3.up) + multiplier * agility * AirBourneControlIntensity * Time.fixedDeltaTime * (MoveField).normalized, Mathf.Max(speed, RunSpeed)) + Vector3.up * Character.velocity.y
                    - Character.velocity, ForceMode.VelocityChange);
            }
        }


        //if (isGrounded && MoveField.sqrMagnitude < Mathf.Epsilon) Character.velocity = Vector3.zero;

        if (isGrounded && jump) Character.AddForce(Vector3.up * (JumpSpeed - Character.velocity.y), ForceMode.VelocityChange);
        //if (isGrounded && jump) Character.velocity = new Vector3(Character.velocity.x, JumpSpeed, Character.velocity.z);
        //Debug.Log(Character.velocity.magnitude);

        if (photonView.IsMine)
            isGrounded = false;
    }

}
