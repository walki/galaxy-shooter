using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _speedBoost = 3.5f;
    
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    [SerializeField]
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _powerUpTime = 5.0f;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private int _tripleShotCount = 0;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;



    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserShot;
    
    [SerializeField]
    private AudioSource _explosion;


    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;


    private bool _leftEngineFirst;


    [SerializeField]
    private PlayerType _playerType = PlayerType.Single;

    private enum PlayerType : int
    {
        Single = 0,
        Player1 = 1,
        Player2 = 2
    }

    private Dictionary<PlayerType, string> horizontalMap = new Dictionary<PlayerType, string>(){
                                                                    {PlayerType.Single, "Horizontal" },
                                                                    {PlayerType.Player1, "Horizontal_1" },
                                                                    {PlayerType.Player2, "Horizontal_2" }};
    private Dictionary<PlayerType, string> verticalMap = new Dictionary<PlayerType, string>(){
                                                                    {PlayerType.Single, "Vertical" },
                                                                    {PlayerType.Player1, "Vertical_1" },
                                                                    {PlayerType.Player2, "Vertical_2" }};
    private Dictionary<PlayerType, KeyCode> fireMap = new Dictionary<PlayerType, KeyCode>(){
                                                                    {PlayerType.Single, KeyCode.Space },
                                                                    {PlayerType.Player1, KeyCode.Space },
                                                                    {PlayerType.Player2, KeyCode.KeypadEnter }};


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas")?.GetComponent<UIManager>();

        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        _leftEngineFirst = (Random.Range(0, 2) == 0);


        _audioSource = GetComponent<AudioSource>();
        Vector3 startPosition = Vector3.zero;
        switch (_playerType)
        {
            case PlayerType.Player1:
                startPosition = new Vector3(-5f, -1.5f, 0f);
                break;
            case PlayerType.Player2:
                startPosition = new Vector3(5f, -1.5f, 0f);
                break;
            case PlayerType.Single:
                startPosition = new Vector3(0f, -1.5f, 0f);
                break;
        }
        transform.position = startPosition;

        
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();

        checkForLaserFire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && other.GetComponent<Laser>().Shooter == Laser.LaserType.Enemy)
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

    private void checkForLaserFire()
    {
        if (Input.GetKeyDown(fireMap[_playerType]) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _tripleShotCount++;
            }
            else
            {
                Vector3 laserPosition = transform.position + new Vector3(0, 0.7f, 0);
                Instantiate(_laserPrefab, laserPosition, Quaternion.identity);
            }

            _audioSource.clip = _laserShot;
            _audioSource.Play();

        }
    }

    private void calculateMovement()
    {
        float horizontalWrapDistance = 11.3f;
        float maxVerticalDistance = 0;
        float minVerticalDistance = -3.8f;

        float horizontalInput = Input.GetAxis(horizontalMap[_playerType]);
        float verticalInput = Input.GetAxis(verticalMap[_playerType]);

        Vector3 direction = (Vector3.right * horizontalInput + Vector3.up * verticalInput);

        // Move based on input
        transform.Translate(direction * _speed * Time.deltaTime);


        // Limit Vertical movement
        transform.position = new Vector3(transform.position.x,
                                        Mathf.Clamp(transform.position.y, minVerticalDistance, maxVerticalDistance),
                                        0);


        // Wrap the horizontal movement
        if (transform.position.x >= horizontalWrapDistance)
        {
            transform.position = new Vector3(-horizontalWrapDistance, transform.position.y, 0);
        }
        else if (transform.position.x < -horizontalWrapDistance)
        {
            transform.position = new Vector3(horizontalWrapDistance, transform.position.y, 0);
        }
    }

    
    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer?.SetActive(false);
            return;
        }
        
        
        _lives--;

        if ((_leftEngineFirst && _lives == 2) || (!_leftEngineFirst && _lives == 1))
        {
            _leftEngine.SetActive(true);
        }
        else if ((_leftEngineFirst && _lives == 1) || (!_leftEngineFirst && _lives == 2))
        {
            _rightEngine.SetActive(true);
        }



        _uiManager.SetLives(_lives);
        
        if(_lives == 0)
        {
            _spawnManager?.OnPlayerDeath();

            _explosion?.Play();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotPowerUp()
    {
        _isTripleShotActive = true;
        _tripleShotCount = 0;
        StartCoroutine(TripleShotPowerDown());

    }

    private IEnumerator TripleShotPowerDown()
    {
        // If the player hasn't shot 10 times, with triple shot, give them another 5 sec.
        while (_tripleShotCount < 10)
        {
            yield return new WaitForSeconds(_powerUpTime);
        }
        _isTripleShotActive = false;
        _tripleShotCount = 0;
    }
    public void SpeedPowerUp()
    {
        _speed += _speedBoost;
        StartCoroutine(SpeedPowerDown());
    }

    private IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(_powerUpTime);

        _speed -= _speedBoost;
    }

    public void ShieldPowerUp()
    {
        _isShieldActive = true;
        _shieldVisualizer?.SetActive(true);

    }

    public void KillEnemy()
    {
        _uiManager?.UpdateScore();
    }


}
