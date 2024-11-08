using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anykey : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit(); //quitte quand le joueur appuis sur echap.
        }

        else if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Jeu");
        }
    }
}
