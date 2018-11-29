using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


//[Serializable]
public class Points
{
    public float[] x;
    public float[] y;
    public float[] z;
}

public class BallScript : MonoBehaviour
{

    private TrailRenderer _trailRenderer;

    public string   filePath;   // Json file name
    private Points  _jsonPoints = new Points();

    // Mouse control
    private float   _delayClick = 0.3f;
    private float   _currentTimerClick = 0.0f;
    private int     _clickCount = 0;
    private bool    _isOneClick = false;
    private bool    _isDoubleClick = false;

    // Move entity
    private enum Status
    {
        Standing,
        Movement,
        Holding
    }
    private Status  _state = Status.Standing;

    // Move
    private Vector3 _startPosition;         // Стартовая позиция
    private Vector3 _startBasePosition;
    private Vector3 _endPosition;           // Конечная позиция
    private bool    _firstMove = true;
    private int     _currentPoint;
    private float   _startTime;             // Начальное время
    private float   _currentTime;           // Текушее
    private float   _minStepTime = 0.001f;
    private float   _maxStepTime = 10f;

    // Slider
    public static float sliderValue = 0.5f;
    private float _stepTime = 5f;

    // Hookup
    private int _thisNumberBall;
    public static int numberBallSelect = 1;

    private void Start()
    {
        switch (gameObject.name)
        {
            case "Ball1":
                _thisNumberBall = 1;
                break;
            case "Ball2":
                _thisNumberBall = 2;
                break;
            case "Ball3":
                _thisNumberBall = 3;
                break;
            case "Ball4":
                _thisNumberBall = 4;
                break;
        }

        _trailRenderer = GetComponent<TrailRenderer>();

        filePath = Application.streamingAssetsPath + "/" + filePath;
        if (File.Exists(filePath))
        {
            _jsonPoints = JsonUtility.FromJson<Points>(File.ReadAllText(filePath));
        }
        else
        {
            Debug.LogError("Ошибка загрузки файла Json: " + filePath);
        }

        StartPosition();
    }

    private void Update()
    {
        if (ThisBallCheck())
        {
            MouseUpdate();
            EntityUpdate();
        }
    }

    // Entity Update
    private void EntityUpdate()
    {
        switch (_state)
        {
            case Status.Standing:
                // Варианты: сброшен, запущен, переключен
                if (_isOneClick)
                {
                    _state = Status.Movement;
                    NextTargetPoint();
                }
                else if (_isDoubleClick)
                {
                    StartPosition();
                }

                break;
            case Status.Movement:
                // Варианты: сброшен, переключен
                if (_isDoubleClick)
                {
                    StartPosition();
                    break;
                }

                MovementUpdate();

                break;
            case Status.Holding:
                // Варианты: продолжать движение, сброшен, переключен
                if (_isDoubleClick)
                {
                    StartPosition();
                }

                break;
        }

        _isOneClick = false;
        _isDoubleClick = false;
    }

    // Start place
    private void StartPosition()
    {
        _firstMove = true;
        _trailRenderer.Clear();
        _currentPoint = 0;
        _state = Status.Standing;

        Vector3 position = new Vector3(_jsonPoints.x[0], _jsonPoints.y[0], _jsonPoints.z[0]);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.SetPositionAndRotation(position, rotation);
    }

    // Next Target Point
    private void NextTargetPoint()
    {

        if (++_currentPoint < _jsonPoints.x.Length)
        {
            _startPosition = transform.position;

            if (_firstMove)
            {
                _endPosition = new Vector3(_jsonPoints.x[_currentPoint], _jsonPoints.y[_currentPoint], _jsonPoints.z[_currentPoint]);
            }
            else // Расчет для повторных движений по намеченной траектории (*.[0] - *.[N] = loc[N])
            {
                _endPosition = new Vector3(_startBasePosition.x - (_jsonPoints.x[0] - _jsonPoints.x[_currentPoint]),
                            _startBasePosition.y - (_jsonPoints.y[0] - _jsonPoints.y[_currentPoint]),
                            _startBasePosition.z - (_jsonPoints.z[0] - _jsonPoints.z[_currentPoint])
                          );
            }

            _startTime = Time.time;
        }
        else
        {
            _firstMove = false;
            _startBasePosition = transform.position;
            _state = Status.Standing;
            _currentPoint = 0;
        }
    }

    // Movement Update
    private void MovementUpdate()
    {
        if ( Vector3.Distance(transform.position, _endPosition) <= 0.001f )
        {
            NextTargetPoint();
        }
        else
        {
            float sVal = Mathf.Abs(sliderValue - 1F);
            if (Mathf.Approximately(sVal, 0.0f))
            {
                sVal = 0.0f;
                _stepTime = _minStepTime;
            }
            else if ((Mathf.Approximately(sVal, 1.0f)))
            {
                sVal = 1.0f;
                _stepTime = -0.01f;
            }
            else
            {
                _stepTime = Mathf.Clamp(sVal, _minStepTime, _maxStepTime);
            }
            
            transform.position = Vector3.Lerp(_startPosition, _endPosition, (Time.time - _startTime) / _stepTime);
        }
    }

    // Mouse Update
    private void MouseUpdate()
    {
        if (_clickCount == 2)
        {
            RefreshClick();
            _isDoubleClick = true;
        }

        if ((_clickCount != 0) && (_currentTimerClick <= Time.time))
        {
            RefreshClick();
            _isOneClick = true;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1") && ThisBallCheck())
        {
            _clickCount++;
            _currentTimerClick = Time.time + _delayClick;

        }
    }

    private void RefreshClick()
    {
        _currentTimerClick = 0.0f;
        _clickCount = 0;
    }

    private bool ThisBallCheck()
    {
        return _thisNumberBall == numberBallSelect ? true : false;
    }

}
