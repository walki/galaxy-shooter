using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    private AudioSource _powerUpSound;

    public enum PowerupType : int
    {
        TripleShot = 0,
        Speed = 1,
        Shields = 2,
    }

    [SerializeField]
    private PowerupType _powerupType;

    // Start is called before the first frame update
    void Start()
    {
        _powerUpSound = GameObject.Find("Audio_Manager/Powerup")?.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.down;
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _powerUpSound?.Play();

            Player player = other.transform.GetComponent<Player>();


            switch (_powerupType)
            {
                case PowerupType.TripleShot:
                    player?.TripleShotPowerUp();
                    break;
                case PowerupType.Speed:
                    player?.SpeedPowerUp();
                    break;
                case PowerupType.Shields:
                    player?.ShieldPowerUp();
                    break;
                default:
                    break;
            }

            Destroy(this.gameObject);
        }
    }
}
