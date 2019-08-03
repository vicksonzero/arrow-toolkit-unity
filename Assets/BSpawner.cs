using System.Collections;
using UnityEngine;

public class BSpawner : MonoBehaviour
{

    public BEnemy enemy;

    public float spawnInterval = 3000;
    public float spawnIntervalMin = 5000;
    public float spawnIntervalMax = 7000;
    public bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnInterval = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnEnemy());
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
