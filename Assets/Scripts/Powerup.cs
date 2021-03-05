using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    // 0 = Triple Shot
    // 1 = Speed
    // 2 = Shield
    [SerializeField]
    private int _powerupID;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.9f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player)
            {
                switch (_powerupID) 
                {
                    case 0: 
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        //player.ShieldActive();
                        break;
                    default:
                        Debug.LogError("_powerupID not found");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
