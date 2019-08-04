using UnityEngine;

public class BEnemyTorch : MonoBehaviour
{
    public BCampFire campFire;

    public bool willSpawnFire = true;
    private void Start()
    {
        if (FindObjectOfType<BCampFire>() != null || FindObjectsOfType<BEnemyTorch>().Length > 1)
        {
            willSpawnFire = false;
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        willSpawnFire = false;
    }

    private void OnDestroy()
    {
        if (willSpawnFire && campFire)
        {
            Instantiate(campFire, transform.position, Quaternion.identity);
        }
    }
}
