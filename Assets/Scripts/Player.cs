using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _speedPowerupMultiplier = 1.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = 0f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == false)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float boundTop = 0f;
        float boundBottom = -3.75f;
        float boundRight = 11f;
        float boundLeft = -11f;

        // Control Player movement using user input
        transform.Translate(direction * _speed * Time.deltaTime);

        // Restrict Player to vertical bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, boundBottom, boundTop), 0);

        // Teleport Player when horizontal bounds are exceeded
        if (transform.position.x >= boundRight)
        {
            transform.position = new Vector3(boundLeft, transform.position.y, 0);
        }
        else if (transform.position.x <= boundLeft)
        {
            transform.position = new Vector3(boundRight, transform.position.y, 0);
        }
    }

    void FireLaser()
    {        
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        } 
        else
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-0.13f, 0.17f, 0), Quaternion.identity);
        }

    }

    public void Damage()
    {
        _lives--;
        if (_lives < 1)
        {
            //tell spawn manager to stop cuz we ded
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine()); 
    }

    public void SpeedActive()
    {
        _speed = _speed * _speedPowerupMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed = _speed / _speedPowerupMultiplier;
    }

}
