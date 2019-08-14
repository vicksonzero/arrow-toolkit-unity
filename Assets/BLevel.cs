using System.Linq;
using UnityEngine;

public class BLevel : MonoBehaviour
{
    [Tooltip("Default -1")]
    public int level = -1;
    public BArrowItem startingArrow;
    public int[][] enemyCaps = new int[][]{
        new int[] { 6, 2, 0, 0 },
        new int[] { 10, 5, 0, 0 },
        new int[] { 15, 7, 1, 0 },
        new int[] { 20, 10, 2, 1 },
        new int[] { 30, 13, 3, 2 },
        new int[] { 50, 20, 3, 2 },
    };

    public BSpawnPoint[] spawnPoints;
    public int[][] unlockSpawnPointList = new int[][]{
        new int[] { 1, 4, 8 },
        new int[] { 9, 7 },
        new int[] { 3, 5 },
        new int[] { 2, 6 },
        new int[] {  },
        new int[] {  },
    };

    public BSpawner[] spawners;
    public int[][] unlockSpawnerList = new int[][]{
        new int[] { 0, 1, 2, 3, 4 ,5 },
        new int[] { 6, 7 },
        new int[] { 8, 9 },
        new int[] { 10, 11 },
        new int[] {  },
        new int[] {  },
    };

    public int[] levelReq = new int[]
    {
        0,
        10,
        50,
        90,
        150,
    };

    public int[] enemyCount = new int[] { 0, 0, 0, 0, 0 };


    BUI ui;
    BController controller;

    public void JoinLevel(int enemyID)
    {
        enemyCount[enemyID]++;
    }

    public void LeaveLevel(int enemyID)
    {
        enemyCount[enemyID]--;
    }

    public int GetCapByEnemyID(int enemyID)
    {
        return (level < 0) ? 0 : enemyCaps[level][enemyID];
    }

    public bool isEnemyCountBelowCap(int enemyID)
    {
        return (level >= 0) && (enemyCount[enemyID] < GetCapByEnemyID(enemyID));
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<BUI>();
        controller = FindObjectOfType<BController>();
        if (level >= 2)
        {
            controller.Tutorial_1_2();
            controller.Tutorial_2_3();
        }

        spawnPoints.ToList().ForEach((sp) =>
        {
            sp.gameObject.SetActive(false);
        });
        ActivateSpawnPointsOfLevel(0);
        for (int i = 0; i < level; i++)
        {
            ActivateSpawnPointsOfLevel(i);
        }

        spawners.ToList().ForEach((sp) =>
        {
            sp.gameObject.SetActive(false);
        });
        ActivateSpawnersOfLevel(0);
        for (int i = 0; i < level; i++)
        {
            ActivateSpawnersOfLevel(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (level == -1 && startingArrow == null)
        {
            level = 0;
            controller.Tutorial_1_2();
        }

        // if can upgrade...
        if (level >= 0 && level <= levelReq.Length - 2)
        {
            if (ui.Coins >= levelReq[level + 1])
            {
                level++;
                ActivateSpawnPointsOfLevel(level);
                ActivateSpawnersOfLevel(level);
                if (level == 1)
                {
                    controller.Tutorial_2_3();
                }
            }
        }
    }

    void ActivateSpawnPointsOfLevel(int level)
    {

        unlockSpawnPointList[level].ToList().ForEach((sp) =>
        {
            spawnPoints[sp].gameObject.SetActive(true);
        });
    }

    void ActivateSpawnersOfLevel(int level)
    {

        unlockSpawnerList[level].ToList().ForEach((sp) =>
        {
            spawners[sp].gameObject.SetActive(true);
        });
    }
}
