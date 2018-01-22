using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinalStageTrigger : MonoBehaviour {

    public FinalPlatformChange finalPlatformChange;
    public LeftWall leftWall;
    public RightWall rightWall;
    public BackWall backWall;
    public FrontWall frontWall;

    public RestartLevel restartLevel;
    
   

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            finalPlatformChange.StartAnimation();
            leftWall.StartAnimation();
            rightWall.StartAnimation();
            backWall.StartAnimation();
            frontWall.StartAnimation();

            restartLevel.enabled = false;
        }
    }
}
