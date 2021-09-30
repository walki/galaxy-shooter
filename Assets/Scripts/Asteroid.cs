using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _rotateSpeed = 30.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _explosion;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager")?.GetComponent<SpawnManager>();
        _explosion = GameObject.Find("Audio_Manager/Explosion")?.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            ExplosionSequence();
        }
    }

    private void ExplosionSequence()
    {
        _spawnManager?.StartSpawning();

        var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        _explosion.Play();
        Destroy(this.gameObject, 0.1f);
    }
}
