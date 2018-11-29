using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    
    public static Transform targetPoint;
    public Vector3  offset;
    
    public float    limitMax = -5;          // ограничение вращения
    public float    limitMin = -80;
    public float    sensitivity = 3;        // чувствительность мышки
    public float    zoom = 0.8f;            // чувствительность 
    public float    zoomMax = 40;           // макс. увеличение
    public float    zoomMin = 3;            // мин. увеличение
    private float   _mX, _mY = 0, _X, _Y;

    //private float   _startTime;             // Начальное время
    //private float   _stepTime = 0.00001f;   // Скорость движения в секундах

    public static Transform b1;
    public static Transform b2;
    public static Transform b3;
    public static Transform b4;

    private void Start()
    {
        targetPoint = GameObject.Find("Point").transform;
        b1 = GameObject.Find("Ball1").transform;
        b2 = GameObject.Find("Ball2").transform;
        b3 = GameObject.Find("Ball3").transform;
        b4 = GameObject.Find("Ball4").transform;

        targetPoint.parent = b1;
        targetPoint.localPosition = new Vector3(0f, 0f, 0f);

        offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax) / 3);
        transform.position = new Vector3(-7f, 5f, -6f);
        transform.localEulerAngles = new Vector3(26f, 47.5f, 0f);
        _Y = -5f;
    }

    public void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            offset.z += zoom;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            offset.z -= zoom;
        }

        offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

        _mX = 0;
        //mY = 0;
        if (Input.GetMouseButton(1))
        {
            _mX =  Input.GetAxis("Mouse X") * sensitivity;
            _mY += Input.GetAxis("Mouse Y") * sensitivity;
            _Y = _mY =  Mathf.Clamp(_mY, limitMin, limitMax);
        }
        
        _X = transform.localEulerAngles.y + _mX;
        
        transform.localEulerAngles = new Vector3(-_Y, _X, 0);
        transform.position = transform.localRotation * offset + targetPoint.position;
    }

    public static void ChangeSelect()
    {
        targetPoint.parent = null;
        switch (BallScript.numberBallSelect)
        {
            case 1:
                targetPoint.parent = b1;
                targetPoint.localPosition = new Vector3(0f, 0f, 0f);
                break;
            case 2:
                targetPoint.parent = b2;
                targetPoint.localPosition = new Vector3(0f, 0f, 0f);
                break;
            case 3:
                targetPoint.parent = b3;
                targetPoint.localPosition = new Vector3(0f, 0f, 0f);
                break;
            case 4:
                targetPoint.parent = b4;
                targetPoint.localPosition = new Vector3(0f, 0f, 0f);
                break;
        }
    }
}
