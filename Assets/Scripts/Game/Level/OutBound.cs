using System;
using UnityEngine;

public class OutBound : MonoBehaviour
{
    public Action OnBallOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnBallOut?.Invoke();
    }
}
