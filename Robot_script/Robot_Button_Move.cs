
using UnityEngine;

public class Robot_Button_Move : MonoBehaviour
{
    public GameObject Body;

    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public GameObject Seven;

    public float postiionSpeed = 2f; // 회전 속도
    public float rotationSpeed = 40f; // 회전 속도


    // Update is called once per frame
    void Update()
    {

        X_Position_Move_Button(KeyCode.T, KeyCode.Y, Body);

        Y_Move_Button(KeyCode.Q, KeyCode.W, One);
        X_Move_Button(KeyCode.O, KeyCode.P, Two);
        X_Move_Button(KeyCode.A, KeyCode.S, Three);
        X_Move_Button(KeyCode.K, KeyCode.L, Four);
        X_Move_Button(KeyCode.Z, KeyCode.X, Five);
        Y_Move_Button(KeyCode.N, KeyCode.M, Six);

        Z_Position_Move_Button(KeyCode.G, KeyCode.H, Seven);
    }

    public void X_Position_Move_Button(KeyCode positiveKey, KeyCode negativeKey, GameObject obj)
    {
        if (Input.GetKey(positiveKey))
        {
            //Debug.Log("Positive Key Pressed: " + obj.transform.position);
            obj.transform.position += Vector3.right * postiionSpeed * Time.deltaTime;
        }

        if (Input.GetKey(negativeKey))
        {
            //Debug.Log("Negative Key Pressed: " + obj.transform.position);
            obj.transform.position -= Vector3.right * postiionSpeed * Time.deltaTime;
        }
    }

    public void Z_Position_Move_Button(KeyCode positiveKey, KeyCode negativeKey, GameObject obj)
    {
        if (Input.GetKey(positiveKey))
        {
            //Debug.Log("Positive Key Pressed: " + transform.position);
            // 현재 위치에서 Z축 방향으로 이동
            obj.transform.Translate(Vector3.forward * postiionSpeed * Time.deltaTime);
        }

        if (Input.GetKey(negativeKey))
        {
            //Debug.Log("Negative Key Pressed: " + transform.position);
            // 현재 위치에서 Z축 반대 방향으로 이동
            obj.transform.Translate(-Vector3.forward * postiionSpeed * Time.deltaTime);
        }
    }

    public void X_Move_Button(KeyCode positiveKey, KeyCode negativeKey, GameObject obj)
    {
        if (Input.GetKey(positiveKey))
        {
            //Debug.Log("Positive Key Pressed: " + obj.transform.position);
            obj.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(negativeKey))
        {
            //Debug.Log("Negative Key Pressed: " + obj.transform.position);
            obj.transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        }
    }

    public void Y_Move_Button(KeyCode positiveKey, KeyCode negativeKey, GameObject obj)
    {
        if (Input.GetKey(positiveKey))
        {
            //Debug.Log("Positive Key Pressed: " + obj.transform.position);
            obj.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(negativeKey))
        {
            //Debug.Log("Negative Key Pressed: " + obj.transform.position);
            obj.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}
