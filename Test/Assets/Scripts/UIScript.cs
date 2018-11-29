using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    public void ButtonLeftClickUI()
    {
        if (++BallScript.numberBallSelect > 4)
            BallScript.numberBallSelect = 1;

        CameraScript.ChangeSelect();
    }

    public void ButtonRightClickUI()
    {
        if (--BallScript.numberBallSelect < 1)
            BallScript.numberBallSelect = 4;

        CameraScript.ChangeSelect();
    }

    public void SliderChangedUI(float Value)
    {
        BallScript.sliderValue = Value;
    }
}
