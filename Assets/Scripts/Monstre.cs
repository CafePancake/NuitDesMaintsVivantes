using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstre : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _anim;
    private float _posY; //position en y
    private float _vitesse;
    private float _limiteX = 10f; //limite de la position en x
    private float _limiteY = 3f; //limite de position en y
    private float _randScale; //nombre aleatoire qui va determiner le scale du monstre plus tard
    private bool _estMort; //est ce que le monstre est mort
    
    void Start()
    {
        Recycler(); //recycle monstres au debut
    }

    void Update()
    {
        _posY = transform.position.y;
        _renderer.sortingOrder=-Mathf.RoundToInt(_posY); //change le order in layer en fonction de la position en y


         transform.Translate(Vector3.left*Time.deltaTime*_vitesse, Space.World); //déplace le monstre vers la gauche en fonction de la vitesse
         _posY = Mathf.Clamp(_posY, -_limiteY, -1f);    //limitation de position en y

        if(_estMort)
        {
           _vitesse = -2f; //si le monstre est mort il recule un peu a cause de l'impact du shotgun
        }

        if(transform.position.x < -10f)
        {
            Recycler(); //recycle le monstre si il se rend assez loin vers la gauche
        }
    }

    private void Recycler()
    {
        transform.position = new Vector3(Random.Range(_limiteX, _limiteX+40), Random.Range(-_limiteY, -0.5f),0); //position aleatoire entre parametres limités
        _randScale = Random.Range(0.2f, 0.5f); //scale aléatoire
        _vitesse = Random.Range(6f, 10f); //vitesse aleatoire
        transform.localScale = new Vector3(_randScale, _randScale, 0); //applique la transformation du scale aléatoire
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bang")){
            _anim.SetTrigger("dies"); //meurt quand entre en contact avec la zone bang
            _estMort=true;
        }
    }

    private void Detruire() //se détruit, s'exécute a la fin de l'animation de mourir comme event
    {
        Destroy(gameObject);
    }
    
}

