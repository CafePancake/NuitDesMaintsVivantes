using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelCtrl : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Bang _bang; //communique avec zone bang
    [SerializeField] private AudioClip _sonOuch;
    [SerializeField] private AudioClip _sonAmmo;
    [SerializeField] private AudioClip _sonHealth;

    private Vector3 _posCam = new Vector3(0, 0, -10); //position de la camera
    private Collider2D _collider;

    private float _posX;
    private float _posY;
    private int _nBlink; //nombre de fois qu'un blink a ete executé
    private float _vitesse = 3f;
    private float _limiteY; //limitation de position en y



    void Start()
    {
        _collider=GetComponent<Collider2D>();
    }

    void Update()
    {
        _posX = transform.position.x;
        _posY = transform.position.y;
        _renderer.sortingOrder=-Mathf.RoundToInt(_posY); //le order in layer s'ajuste en fonction de la position en y, ce qui laisse passer des ennemis devant ou derrière le perso en fonction de la différence en y

        float dx=Input.GetAxisRaw("Vertical"); //direction du mouvement dx = -1 quand descent, 0 quand immobile et 1 quand monte

        if(dx>0)
        {
            _anim.SetTrigger("up"); //si perso monte, anim monte
        }

        else if(dx<0)
        {
            _anim.SetTrigger("down"); //si perso descend, anim descend
        }
        
        else if(dx==0){
            _anim.SetTrigger("idle"); //si immobile anim idle
        }

        if(Input.GetKeyDown("space"))
        {
            _anim.SetTrigger("bang"); //quand appuis sur espace anim tirer
        }

        transform.Translate(Vector3.up * dx*_vitesse * Time.deltaTime, Space.World); //déplace le perso en fonction de direction et vitesse

        _limiteY = Mathf.Clamp(transform.position.y, -3f, 0.5f);
        transform.position = new Vector3(transform.position.x, _limiteY, 0); //la position en y est limitée entre -3f et 0.5, pas idéal
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monstre")){ //quand collision avec un monstre
            AudioSource.PlayClipAtPoint(_sonOuch, _posCam); 
            _manager.AjusterVie(-1); //retire une vie
            Blink(); //blink est une fonction qui agit comme iframes
        }

         else if(other.CompareTag("Health")){ //quand collision avec bonue de vies
            _manager.AjusterVie(3); //ajoute 3 vies
            AudioSource.PlayClipAtPoint(_sonHealth, _posCam);
        }

        else if(other.CompareTag("Ammo")){ //quand collision avec bonus vitesse (ammo)
            _bang._pumpSpeed = 0.6f; //pumpspeed delais reduit a 0.6 sec
            _vitesse = 5f; //vitesse augmentée a 5
            Invoke("NormalizeSpeed", 5f); //vitesse normale apres 5sec
            AudioSource.PlayClipAtPoint(_sonAmmo, _posCam);
        }
    }


    private void Blink()
    {
        _collider.enabled = false; //collision disabled
        _nBlink=0; //reset du nombre de blink
        InvokeRepeating("DisRenderer", 0f, 0.2f); //chaque 0.2 sec
    }

    private void DisRenderer()
    {
        _renderer.enabled = false; //disable renderer du perso
        Invoke("EnRenderer", 0.1f); //puis apres 0.1sec
    }

    private void EnRenderer()
    {
        _renderer.enabled = true; //enable le renderer
        _nBlink++; //nb de blink +1
        if(_nBlink>=10) //si a blinké 10 fois ou plus
        {
            CancelInvoke(); //annule les invokes
            _collider.enabled = true; //collision enabled 
        }
    }


    private void NormalizeSpeed() //execute quand le bonus de vitesse prend fin
    {
        _bang._pumpSpeed = 1.2f;
        _vitesse = 3f;
    }
}
