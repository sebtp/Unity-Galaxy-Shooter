using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;
    private Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
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
            Destroy(this.gameObject);    
        }
   
        if (other.CompareTag("Laser"))
        {
            if (_player)
            {
                _player.AddScore(); 
            }
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }
    }
}
