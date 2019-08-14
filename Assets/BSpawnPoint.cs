using System.Collections;
using UnityEngine;

public class BSpawnPoint : MonoBehaviour
{
    public float slowSpeed = 1;
    public float fastSpeed = 5;

    public float prepareTime = 2;
    public float restTime = 3;

    public bool canSpawn = false;

    public BAutoRotateSprite smallWhirlPool;
    public BAutoRotateSprite bigWhirlPool;

    float smallSpeedFactor = 0;
    float bigSpeedFactor = 0;
    // Start is called before the first frame update
    void Start()
    {
        smallSpeedFactor = smallWhirlPool.speed / slowSpeed;
        bigSpeedFactor = bigWhirlPool.speed / slowSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnWithEffect(BEnemy enemy)
    {
        StartCoroutine(SpawnWithEffectSequence(enemy));
    }

    IEnumerator SpawnWithEffectSequence(BEnemy enemy)
    {
        canSpawn = false;
        yield return new WaitForEndOfFrame();
        smallWhirlPool.speed = smallSpeedFactor * fastSpeed;
        bigWhirlPool.speed = bigSpeedFactor * fastSpeed;

        yield return new WaitForSeconds(prepareTime);
        if (gameObject == null)
        {
            yield break;
        }

        var bull = Instantiate(enemy, transform.position, transform.rotation);

        smallWhirlPool.speed = smallSpeedFactor * slowSpeed;
        bigWhirlPool.speed = bigSpeedFactor * slowSpeed;

        yield return new WaitForSeconds(restTime);
        canSpawn = true;
    }
}
