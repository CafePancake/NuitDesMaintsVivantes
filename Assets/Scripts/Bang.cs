using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bang : MonoBehaviour
{
    public bool _bangReady = true; //bool qui détermine si le personnage est prêt à tirer
    [SerializeField] private AudioClip _sonBang;
    [SerializeField] private AudioClip _sonVide;
    [SerializeField] private AudioClip _sonPump;
    [SerializeField] private GameManager _manager;
    public float _pumpSpeed = 1.2f; //délais en sec avant de pouvoir tirer a nouveau
    private Vector3 _posCam = new Vector3(0, 0, -10); //position de la camera
    private Collider2D _collider;
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;  //la zone bang commence avec collider disabled
    }

    void Update()
    {
        if(Input.GetKeyDown("space")&& _bangReady)  //quand le perso est pret et qu'on appuis sur espace
        {
            AudioSource.PlayClipAtPoint(_sonBang, _posCam); //joue le son de tirer a la position de la camera
            _collider.enabled = true;
            Invoke("DisCollider",0.1f); //le collider s'active durant 0.1sec
            _bangReady = false;
            Invoke("Pump", _pumpSpeed);
        }
        else if(Input.GetKeyDown("space"))
        {
            AudioSource.PlayClipAtPoint(_sonVide, _posCam); //sinon si le jouer n'est pas pret joue le son de chambre vide
        }
    }

    private void DisCollider()
    {
        _collider.enabled = false;
    }
    private void Pump() //invoked apres delais pumpspeed
    {
        AudioSource.PlayClipAtPoint(_sonPump, _posCam);
        _bangReady = true; //perso prêt a tirer
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monstre")){ //ajoute 5 points quand bangzone touche un monstre
            _manager.AjouterPoints(5);
        }
    }
}
