using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool isEnable = true;

    public Action OnBallPush;
    public Action OnClickPause;
    public Action<float> OnMovement;

    void Update()
    {
        if (isEnable)
        {
            if (Input.GetAxis("Horizontal") != 0)
                OnMovement?.Invoke(Input.GetAxis("Horizontal"));
            if (Input.GetKeyUp(KeyCode.Space))
                OnBallPush?.Invoke();
            if (Input.GetKeyUp(KeyCode.Escape))
                OnClickPause?.Invoke();
        }
    }
}
