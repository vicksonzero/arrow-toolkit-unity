using System.Collections;
using System.Linq;
using UnityEngine;

public class BSpawner : MonoBehaviour
{

    public BEnemy enemy;
    public int enemyID = 0;

    public float spawnInterval = 3000;
    public float spawnIntervalMin = 5000;
    public float spawnIntervalMax = 7000;
    public bool canSpawn = true;
    public bool spawnWhenActivated = true;

    public BSpawnPoint[] spawnPoints;

    BLevel bLevel;

    // Start is called before the first frame update
    void Start()
    {
        spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
        bLevel = FindObjectOfType<BLevel>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn)
        {
            // Debug.Log("Try spawn: " + name);
            if (bLevel.isEnemyCountBelowCap(enemyID))
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    public IEnumerator SpawnEnemy()
    {
        if (spawnWhenActivated)
        {
            var availableSpawnPoints = (spawnPoints.ToList()
                .Where((sp) => sp && sp.gameObject.activeSelf)
                .ToArray()
                );
            if (availableSpawnPoints.Length <= 0)
            {
                availableSpawnPoints = spawnPoints;
            }

            availableSpawnPoints = (availableSpawnPoints.ToList()
                .Where((sp) => sp && sp.canSpawn)
                .ToArray()
                );
            if (availableSpawnPoints.Length <= 0)
            {
                print("insufficient spawn points(name=" + name + ")");
                yield break;
            }
            var spawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Length)];
            spawnPoint.gameObject.SetActive(true);
            spawnPoint.SpawnWithEffect(enemy);
        }

        canSpawn = false;

        //Debug.Log("Waiting 1 second...");
        yield return new WaitForSeconds(spawnInterval / 1000);
        if (gameObject == null)
        {
            yield break;
        }
        canSpawn = true;
        spawnWhenActivated = true;
    }
}
