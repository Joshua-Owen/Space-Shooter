using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnRate = 5.0f;
    
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject _powerUpContainer;

    [SerializeField]
    private GameObject _TripleShotPowerUpPrefab;
    [SerializeField]
    private List<GameObject> _powerUp;
    void Start()
    {
       StartCoroutine("SpawnEnemyRoutine");
       StartCoroutine("SpawnPowerUpRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        
        while (!_stopSpawning)
        {print("f2");
            yield return new WaitForSeconds(_spawnRate);
            float spawnPoint = Random.Range(-9.0f, 9.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            //links enemys to enemy container child
            newEnemy.transform.parent = _enemyContainer.transform;

            
        }

      
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        while (!_stopSpawning)
        {
            float randomSpawnTime = Random.Range(3.0f, 7.0f);
            yield return new WaitForSeconds(randomSpawnTime);
            float spawnPoint = Random.Range(-9.0f, 9.0f);
            Vector3 posToSpawn = new Vector3(spawnPoint, 7, 0);
            int randomPowerUp = Random.Range(0, _powerUp.Count);
            GameObject newEnemy = Instantiate(_powerUp[randomPowerUp], posToSpawn, Quaternion.identity);
           
            //links enemys to enemy container child
            newEnemy.transform.parent = _powerUpContainer.transform;
           
        }
        

    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
