using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonsFin : MonoBehaviour
{
    void Start()
    {
        Invoke("Quitter", 48f); //quitte le jeu après 48 sec (c'est le temps ou l'audio coupe/ le joueur se fait attraper par la main)
    }
    public void Rejouer()
    {
        SceneManager.LoadScene("MainMenu"); //quand on appuis sur rejouer (repousser la main) retourne au menu principal
    }

    public void Quitter()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
