using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot_ik_test : MonoBehaviour
{
    public GameObject Body;
    public GameObject Target;

    public float positionSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        X_Position_Move_Button(KeyCode.T, KeyCode.Y, Body);
        MoveTarget();
        TargetMove();
    }

    public void X_Position_Move_Button(KeyCode positiveKey, KeyCode negativeKey, GameObject obj)
    {
        if (Input.GetKey(positiveKey))
        {
            //Debug.Log("Positive Key Pressed: " + obj.transform.position);
            obj.transform.position += Vector3.right * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(negativeKey))
        {
            //Debug.Log("Negative Key Pressed: " + obj.transform.position);
            obj.transform.position -= Vector3.right * positionSpeed * Time.deltaTime;
        }
    }

    public void MoveTarget()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Target.transform.position += Vector3.forward * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Target.transform.position -= Vector3.forward * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Target.transform.position -= Vector3.right * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Target.transform.position += Vector3.right * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Target.transform.position += Vector3.up * positionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            Target.transform.position -= Vector3.up * positionSpeed * Time.deltaTime;
        }
    }

    public void TargetMove()
    {
        Body.transform.position += new Vector3(-4.1f,0,0) * positionSpeed * Time.deltaTime;
        Target.transform.position += new Vector3(-1.8f,1.8f,-2.95f) * positionSpeed * Time.deltaTime;
    }
}
