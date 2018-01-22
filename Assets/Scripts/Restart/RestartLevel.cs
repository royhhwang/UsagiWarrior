using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

    void OnTriggerEnter()
    {
        SceneManager.LoadScene("Platforms");
    }
}
