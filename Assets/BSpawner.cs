using System.Collections;
using UnityEngine;

public class BSpawner : MonoBehaviour
{

    public BEnemy enemy;
    public int enemyID = 0;

    public float spawnInterval = 3000;
    public float spawnIntervalMin = 5000;
    public float spawnIntervalMax = 7000;
    public bool canSpawn = true;

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
        var bull = Instantiate(enemy, transform.position, transform.rotation);

        canSpawn = false;

        //Debug.Log("Waiting 1 second...");
        yield return new WaitForSeconds(spawnInterval / 1000);
        if (gameObject == null)
        {
            yield break;
        }
        canSpawn = true;
    }
}
