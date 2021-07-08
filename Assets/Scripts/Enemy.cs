using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;

    private float _boundTop = 7.2f;
    //private float _boundBottom = -5f;
    private float _boundRight = 9f;
    private float _boundLeft = -9f;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (!_anim)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f && GetComponent<BoxCollider2D>())
        {
            transform.position = new Vector3(Random.Range(_boundLeft, _boundRight), _boundTop, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player)
            {
                _player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 2.35f);
        }
   
        if (other.CompareTag("Laser"))
        {
            if (_player)
            {
                _player.AddScore(); 
            }
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 2.35f);
        }
    }
}
