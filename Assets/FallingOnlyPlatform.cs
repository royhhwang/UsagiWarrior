﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOnlyPlatform : MonoBehaviour
{

    public Transform movingPlatform;
    public Transform position1;
    public Transform position2;

    public Vector3 newPosition;
    public string currentState;
    float smooth;

    private void Start()
    {
        smooth = 1f;
        newPosition = position1.position;
    }

    private void FixedUpdate()
    {
        movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, Time.deltaTime * smooth);
    }

    public void ChangeTarget()
    {
        if (currentState == "Top")
        {
            newPosition = position2.position;
        }
        else if (currentState == "Bottom")
        {
            newPosition = position1.position;
        }
    }

    public void LateChangeTarget()
    {
        Invoke("ChangeTarget", 1.5f);
    }
}


