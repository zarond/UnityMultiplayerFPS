using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;
    private Vector2 RotCoords = new Vector2(0, 0);
    private Vector2 MouseInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        RotCoords.Set(Mathf.Repeat(RotCoords.x + MouseInput.x,360), Mathf.Clamp(RotCoords.y + MouseInput.y, -80, 80));
        RaycastHit ray;
        //Physics.Raycast(Target.position, transform.position - Target.position,  out ray);
        //if (ray.distance < 10) offset.z = -ray.distance; else offset.z = -5;
        if (Physics.Raycast(Target.position, transform.position - Target.position, out ray))
            offset.z = -Mathf.Clamp(ray.distance, 1.5f, 6);
        else offset.z = -6;
        //Debug.Log(ray.distance);
    }
    
    void LateUpdate()
    {
        transform.position = Target.position + Quaternion.Euler(-RotCoords.y, RotCoords.x, 0) * offset;
        transform.rotation = Quaternion.Euler(-RotCoords.y, RotCoords.x,0);
    }
}
