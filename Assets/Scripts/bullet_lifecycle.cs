using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bullet_lifecycle : MonoBehaviour
{
    public float lifeDuration;
    public float speed, damage, splashRadius, shockwave;
    public bool jet;

    float timer;


    Rigidbody rigidbody;
    Vector3 direction;
    GameObject weapon, player;
    CharacterController character;
    Dictionary<Collider, float> distances;

    void Start()
    {
        weapon = GameObject.Find("weapon");
        player = GameObject.FindWithTag("Player");

        timer = lifeDuration;
        rigidbody = GetComponent<Rigidbody>();

        character = player.GetComponent<CharacterController>();
        rigidbody.velocity = character.velocity;

        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        direction = weapon.transform.up;
    }

    bool movable = true;
    void Update()
    {
        Debug.DrawRay(transform.position, weapon.transform.up);

        timer -= Time.deltaTime;
        //Debug.Log(timer);
        if (timer <= 0) Destroy(this.gameObject);

        if (jet)
        {
            rigidbody.freezeRotation = true;
            /*
            float x = weapon.transform.up[0] * Time.deltaTime * speed,
                  y = weapon.transform.up[1] * Time.deltaTime * speed,
                  z = weapon.transform.up[2] * Time.deltaTime * speed;
            rigidbody.AddForce(new Vector3 (x, y, z));*/
            rigidbody.AddForce(direction * Time.deltaTime * speed);
        }
        else
        {
            rigidbody.freezeRotation = false;

            if (movable)
            {
                /*
                float x = weapon.transform.up[0] * Time.deltaTime * speed,
                      y = weapon.transform.up[1] * Time.deltaTime * speed,
                      z = weapon.transform.up[2] * Time.deltaTime * speed;
                rigidbody.AddForce(new Vector3 (x, y, z));*/
                rigidbody.AddForce(direction * Time.deltaTime * speed);
                movable = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //foreach (Collider collider in distances.Keys)
        //{
        //    collider.gameObject.SendMessage("DoDamage", distances[collider]);
        //}

        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        foreach (GameObject obj in rootObjects)
        {
            if (obj.GetComponent<CapsuleCollider>() != null)
            {
                CapsuleCollider col = obj.GetComponent<CapsuleCollider>();

                float dmg;
                if (Vector3.Distance(collision.contacts[0].point, col.ClosestPoint(collision.contacts[0].point)) <= splashRadius)
                    dmg = Vector3.Distance(collision.contacts[0].point, col.ClosestPoint(collision.contacts[0].point)) * (-1f / splashRadius) + damage;
                else
                    dmg = 0;

                Debug.Log(dmg);

                if (obj == player) dmg /= 2;

                obj.SendMessage("DoDamage", dmg, SendMessageOptions.DontRequireReceiver);

                if (obj.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = obj.GetComponent<Rigidbody>();

                    direction = col.ClosestPoint(collision.contacts[0].point) - collision.contacts[0].point;
                    direction.Normalize();
                    rb.AddForce(direction * dmg * shockwave);
                }
            }

            

            Destroy(this.gameObject);
        }
    }
}