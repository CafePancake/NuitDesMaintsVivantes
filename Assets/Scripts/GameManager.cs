using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _monstre; //prefab des monstres du jeu
    [SerializeField] private GameObject _helthPack; //prefab du bonus de vies
    [SerializeField] private GameObject _ammoKrate; //prefab du bonus de vitesse
    
    [SerializeField] private TextMeshProUGUI _txtVies; //champ affichage de vies
    [SerializeField] private TextMeshProUGUI _txtVague; //champ affichage du numéro de la vague actuelle
    [SerializeField] private TextMeshProUGUI _txtPointage; //cham affichage du pointage
    [SerializeField] private AudioClip _sonBonus;

    private Vector3 _camPos = new Vector3(0, 0, -10); //position de la camera
    private Vector3 _healthPos = new Vector3(-6f, -0.5f, 0); //position du bonus de vies
    private Vector3 _AmmoPos = new Vector3(-6f, -3.9f, 0); //position du bonus de vitesse

    const int _viesDepart = 3; //maximum de vie
    public int  _vies; //nb de vies
    private int _nbMains = 1; //nombre qui determine le nombre d'ennemis ajoutés a chaque vague
    private int _vague; //nombre de la vague actualle
    private int _temps; //nombre de sec
    private int _points; //nb de points



    void Start()
    {
        Invoke("DebuterPartie", 3f);
        InvokeRepeating("UpdateTimer", 1f, 1f); //commence un timer qui compte les secondes
        _vague = 1; //reset le numéro de vague, nb de points, et nb de vies
        _points = 0;
        _vies = _viesDepart;
    }

    void Update()
    {
        _txtPointage.text=""+_points; //update le nb de points affiché
        if(_vies==0)
        {
            SceneManager.LoadScene("GameOver"); //écran de fin de partie si je joueur est a court de vies
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit(); //quitte quand on appuis sur echap
        }
    }
    void DebuterPartie()
    {
        for (int i = 0; i < 6; i++)
        {
            Instantiate(_monstre); //crée 6 montres au début
        }
        InvokeRepeating("TriggerNextWave",5f,5f); //chaque 5sec une nouvelle vague commence
    }

    private void TriggerNextWave()
    {
        _vague++; //nombre de la vague +1
        _txtVague.text=""+_vague; //update le champ d'affichage de numéro de vague

        for (int i = 0; i < _nbMains; i++)
        {
            Instantiate(_monstre); //crée un nombre de monstre égal a _nbmain
        }
        _nbMains+=1; //nbmain augmente de 1 chaque vague, il y a donc de plus en plus de monstres créés par vague
    }

    public void AjusterVie(int vies)//recois un parametre qui modifie la valeur de vie
    {
        _vies = Mathf.Clamp(_vies+=vies, 0, 5); //nb vies est un minimum de 0 et max de 5
        _txtVies.text = ""; //efface le nb de vie dans le champ texte de vies
        for (int i = 0; i < _vies; i++)
        {
           _txtVies.text+="I"; //crée un trait I pour chaque vie du joueur
        }
    }

    private void UpdateTimer()
    {
        _temps++; //la variable temps (sec ecoulés depuis debut de partie) n'est pas utilisé mais pourrait être utile pour de futures modifications
        _points+=1; //points augmente de 1 chaque sec
        RollLootery(); //roule le dé de bonus chaque sec
    }

    private void RollLootery()
    {
        int LootNumber = (Random.Range(1, 60)); //chiffre aléatoire entre 1 et 59 (chiffres arbitraires)
        if(LootNumber==1)
        {
            Instantiate(_helthPack, _healthPos, Quaternion.identity); //si LootNumber est 1 crée un bonus de vies a la position prédéfinie
            AudioSource.PlayClipAtPoint(_sonBonus, _camPos);
        }

        else if(LootNumber==2)
        {
            Instantiate(_ammoKrate, _AmmoPos, Quaternion.identity); //si LootNumber est 2, crée un bonus de vitesse a position predefinie
            AudioSource.PlayClipAtPoint(_sonBonus, _camPos);
        }
    }

     public void AjouterPoints(int points)
    {
        _points+=points; //rajoute le nb de points spécifiés par le parametre
    }
}