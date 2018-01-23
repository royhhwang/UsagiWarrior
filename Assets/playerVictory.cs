using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerVictory : MonoBehaviour
{

    public Victory victory;
    Animator anim;
    private int restartGame = 0;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (victory.playerWins == true)
        {
            anim.SetTrigger("playerWins");
            restartGame ++;
            if (restartGame >= 399)
            {
                SceneManager.LoadScene("Platforms");
                restartGame = 0;
            }
        }
    }
}
