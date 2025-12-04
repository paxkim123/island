using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Auto_Move : MonoBehaviour
{
    public GameObject Frame;

    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public GameObject Seven;

    public float positionSpeed = 2f;
    public float rotationSpeed = 0.5f;

    private int MoveStatus = 111;

    void Start()
    {

    }

    void Update()
    {
        Car_1();
        Packing();
    }

    public void Button_Packing()
    {
        if (MoveStatus == 111)
        {
            Debug.Log("111∆–≈∑ ≈¿¿Ω");
            MoveStatus = 0;
        }
    }

    public void Button_car_1()
    {
        if (MoveStatus == 111)
        {
            MoveStatus = 10;
        }
    }

    public void Packing()
    {
        if (MoveStatus == 0)
        {
            AutoMove_P(Seven, new Vector3(0, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 1)
        {
            AutoMove_R(Six, Quaternion.Euler(0, 180, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 2)
        {
            AutoMove_R(Five, Quaternion.Euler(0, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 3)
        {
            AutoMove_R(Four, Quaternion.Euler(0, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 4)
        {
            AutoMove_R(Three, Quaternion.Euler(-50, 0, 0));
            AutoMove_R(Two, Quaternion.Euler(50, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 5)
        {

            AutoMove_R(Seven, Quaternion.Euler(0, 0, 0));

            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 6)
        {
            AutoMove_P_R(Frame, One, new Vector3(2, 0, 0), Quaternion.Euler(0, -90, 0));
        }
        else if (MoveStatus == 7)
        {
            MoveStatus = 111;
            Debug.Log(MoveStatus);
        }
    }

    public void Car_1()
    {
        if (MoveStatus == 10)
        {
            AutoMove_P_R(Frame, One, new Vector3(-6.3f, 0, 0), Quaternion.Euler(0, 0, 0));
        }
        else if (MoveStatus == 11)
        {
            AutoMove_R(Three, Quaternion.Euler(-80, 0, 0));
            AutoMove_R(Two, Quaternion.Euler(70, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 12)
        {
            AutoMove_R(Four, Quaternion.Euler(0, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 13)
        {
            AutoMove_R(Five, Quaternion.Euler(0, 0, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 14)
        {
            AutoMove_R(Six, Quaternion.Euler(0, 180, 0));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 15)
        {
            AutoMove_P(Seven, new Vector3(0, 0, -5f));
            Debug.Log(MoveStatus);
        }
        else if (MoveStatus == 16)
        {
            MoveStatus = 111;
            Debug.Log(MoveStatus);
        }
    }

    public void AutoMove_P_R(GameObject Move_P, GameObject Move_R, Vector3 targetLocalPosition, Quaternion targetLocalRotation)
    {
        Vector3 currentLocalPosition = Move_P.transform.localPosition;
        Quaternion currentLocalRotation = Move_R.transform.localRotation;

            Move_P.transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPosition, positionSpeed * Time.deltaTime);
            Move_R.transform.localRotation = Quaternion.RotateTowards(currentLocalRotation, targetLocalRotation, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(currentLocalPosition, targetLocalPosition) <= 0.01f &&
                Quaternion.Angle(currentLocalRotation, targetLocalRotation) <= 0.1f)
            {
                MoveStatus += 1;
            }
    }

    public void AutoMove_P(GameObject Move_P, Vector3 targetLocalPosition)
    {
        Vector3 currentLocalPosition = Move_P.transform.localPosition;

            Move_P.transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPosition, positionSpeed * Time.deltaTime);

        if (Vector3.Distance(currentLocalPosition, targetLocalPosition) <= 0.01f)
        {
            MoveStatus += 1;
        }
    }

    public void AutoMove_R(GameObject Move_R, Quaternion targetLocalRotation)
    {
        Quaternion currentLocalRotation = Move_R.transform.localRotation;

            Move_R.transform.localRotation = Quaternion.RotateTowards(currentLocalRotation, targetLocalRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(currentLocalRotation, targetLocalRotation) <= 0.1f)
            {
                MoveStatus += 1;
            }
    }


    public void AutoMove_P_P(GameObject Move_P, Vector3 targetLocalPosition, GameObject Move_P2, Vector3 targetLocalPosition2)
    {
        Vector3 currentLocalPosition = Move_P.transform.localPosition;
        Vector3 currentLocalPosition2 = Move_P2.transform.localPosition;

        Move_P.transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPosition, positionSpeed * Time.deltaTime);
        Move_P2.transform.localPosition = Vector3.MoveTowards(currentLocalPosition2, targetLocalPosition2, positionSpeed * Time.deltaTime);

        if (Vector3.Distance(currentLocalPosition, targetLocalPosition) <= 0.01f && Vector3.Distance(currentLocalPosition2, targetLocalPosition2) <= 0.01f)
        {
            MoveStatus += 1;
        }
    }

    public void AutoMove_R_R(GameObject Move_R, Quaternion targetLocalRotation, GameObject Move_R2, Quaternion targetLocalRotation2)
    {
        Quaternion currentLocalRotation = Move_R.transform.localRotation;
        Quaternion currentLocalRotation2 = Move_R2.transform.localRotation;

        Move_R.transform.localRotation = Quaternion.RotateTowards(currentLocalRotation, targetLocalRotation, rotationSpeed * Time.deltaTime);
        Move_R2.transform.localRotation = Quaternion.RotateTowards(currentLocalRotation2, targetLocalRotation2, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(currentLocalRotation, targetLocalRotation) <= 0.1f && Quaternion.Angle(currentLocalRotation2, targetLocalRotation2) <= 0.1f)
        {
            MoveStatus += 1;
        }
    }
}