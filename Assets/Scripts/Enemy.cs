using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;

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

        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
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
