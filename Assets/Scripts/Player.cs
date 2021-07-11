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
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = 0f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    

    private GameObject _shield;
    
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _clipExplosion;
    [SerializeField]
    private AudioClip _clipLaser;

    [SerializeField]
    public int _score;

    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (!_spawnManager)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _shield = gameObject.transform.GetChild(0).gameObject;
        if (!_shield)
        {
            Debug.LogError("The Shield is NULL");
        }

        _rightEngine = gameObject.transform.GetChild(2).gameObject;
        if (!_rightEngine)
        {
            Debug.LogError("The Right Engine is NULL");
        }

        _leftEngine = gameObject.transform.GetChild(3).gameObject;
        if (!_leftEngine)
        {
            Debug.LogError("The Left Engine is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (!_uiManager)
        {
            Debug.LogError("UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    public void Firing()
    {
        if (Time.time >= _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;
       
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

        AudioSource.PlayClipAtPoint(_clipLaser, transform.position);
        
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
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        } 

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        
        if (_lives >= 0)
        {
            _uiManager.UpdateLives(_lives);
        }
        else if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverSequence();
            AudioSource.PlayClipAtPoint(_clipExplosion, transform.position);
            Destroy(gameObject);
        }  
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine()); 
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed *= _speedPowerupMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed /= _speedPowerupMultiplier;
        _isSpeedActive = false;
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScoreText(_score);
    }
}
