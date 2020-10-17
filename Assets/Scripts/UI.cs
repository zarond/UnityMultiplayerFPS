using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Vector2 input;
    private Vector2 rawinput;
    public Transform DbgJ;
    public Transform DbgJr;
    private Vector2 minput;
    private Vector2 mrawinput;
    public Transform DbgM;
    public Transform DbgMr;
    public Transform DbgMoveField;
    public Vector3 MoveField = new Vector3(0, 0, 0);
    private bool paused = false;
    public GameObject PauseMenu;
    public GameObject GroundedIndicator;
    public GameObject cube;
    private Rigidbody cube_rigidbody;
    private MovementRigidBody cube_mov;
    private health Health;
    public Text Health_text;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cube_rigidbody = cube.GetComponent<Rigidbody>();
        cube_mov = cube.GetComponent<MovementRigidBody>();
        Health = this.GetComponentInParent<health>();
    }

    // Update is called once per frame
    void Update()
    {
        input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawinput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveField = cube_rigidbody.velocity / cube_mov.spd;
        MoveField = cube.transform.InverseTransformVector(MoveField);
        DbgMoveField.localPosition = 50 * new Vector2(MoveField.x,MoveField.z);
        DbgJ.localPosition = 50 * input;
        DbgJr.localPosition = 50 * rawinput;

        minput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mrawinput.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        DbgM.localPosition = 50 * minput;
        DbgMr.localPosition = 50 * mrawinput;

        if (!paused && Input.GetKeyDown("p"))
        {
            paused = true;
            PauseMenu.SetActive(paused);
            Time.timeScale = 0;
        }
        else if (paused && Input.GetKeyDown("p")) {
            paused = false;
            PauseMenu.SetActive(paused);
            Time.timeScale = 1;
        }

        GroundedIndicator.SetActive(cube_mov.isGrounded);


        if (Health != null) {
            Health_text.text = "Health: " + (Health.hp).ToString();
        }

    }
}
