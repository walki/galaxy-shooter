using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;


    const float SCREEN_X = 9.5f;
    const float SCREEN_Y = 7.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    // Spawn Game Objects every 5 seconds using Coroutines
    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1.5f);

        while (!_stopSpawning)
        {
            float randomX = Random.Range(-SCREEN_X, SCREEN_X);
            Vector3 enemyPosition = new Vector3(randomX, SCREEN_Y, 0);
            GameObject spawn = Instantiate(_enemyPrefab, enemyPosition, Quaternion.identity);
            spawn.transform.parent = _enemyContainer.transform;

            float randomInt = Random.Range(1.0f, 6.0f);
            yield return new WaitForSeconds(randomInt);

        }
    }

    private IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(1.5f);

        while (!_stopSpawning)
        {
            float timeTilPowerup = Random.Range(6.0f, 12.0f);
            yield return new WaitForSeconds(timeTilPowerup);

            float randomX = Random.Range(-SCREEN_X, SCREEN_X);
            Vector3 powerUpPosition = new Vector3(randomX, SCREEN_Y, 0);

            int powerupType = Random.Range(0, 3);

            Instantiate(_powerups[powerupType], powerUpPosition, Quaternion.identity);
        }
    }
}
