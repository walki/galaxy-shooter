using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private GameManager _gameManager;
    private Player _player;
    private Player _player1;
    private Player _player2;

    private Animator _animator;
    private bool _isDying;

    [SerializeField]
    private AudioSource _explosion;

    [SerializeField]
    private GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager.isCoopMode)
        {
            _player1 = GameObject.Find("Player_1").GetComponent<Player>();
            _player2 = GameObject.Find("Player_2").GetComponent<Player>();

        }
        else
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }


        _explosion = GameObject.Find("Audio_Manager/Explosion")?.GetComponent<AudioSource>();

        _animator = GetComponent<Animator>();

        StartCoroutine(FireLasers());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        // Enemy moves down
        Vector3 direction = Vector3.down;
        transform.Translate(direction * _speed * Time.deltaTime);

        if (!_isDying)
        {
            if (transform.position.y < -5.5f)
            {
                // if the enemy drops off the edge, we are going to randomize a respawn at the top of the screen
                float randomX = Random.Range(-9.5f, 9.5f);
                transform.position = new Vector3(randomX, 7.5f, transform.position.z);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player_1")
        {
            _player1.Damage();

            DeathSequence();
        }
        else if (other.name == "Player_2")
        {
            _player2.Damage();

            DeathSequence();
        }
        else if (other.name == "Player")
        {
            _player.Damage();
            DeathSequence();
        }

        if (other.tag == "Laser" && other.GetComponent<Laser>().Shooter == Laser.LaserType.Player)
        {
            if (_gameManager.isCoopMode)
            {
                if (_player1 != null)
                { 
                    _player1?.KillEnemy();
                }
                else if (_player2 != null)
                {
                    _player2?.KillEnemy();
                }

            }
            else
            {
                _player?.KillEnemy();
            }
            Destroy(other.gameObject);

            DeathSequence();
        }

    }

    private void DeathSequence()
    {
        _isDying = true;

        Destroy(GetComponent<Collider2D>());
        _animator.SetTrigger("OnEnemyDeath");
        _explosion.Play();

        Destroy(this.gameObject, 2.8f);
    }

    private IEnumerator FireLasers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));

            if (!_isDying)
            {
                
                var enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                foreach(var laser in lasers)
                {
                    laser.Shooter = Laser.LaserType.Enemy;
                }

            }
        }
    }


}
