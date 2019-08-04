using UnityEngine;

public class BLevel : MonoBehaviour
{
    public int level = -1;
    public BArrowItem startingArrow;
    public int[][] enemyCaps = new int[][]{
        new int[] { 10, 2, 0 },
        new int[] { 20, 10, 0 },
        new int[] { 25, 10, 1 },
        new int[] { 35, 15, 2 },
        new int[] { 50, 20, 3 },
        new int[] { 70, 25, 3 },
    };

    public int[] levelReq = new int[]
    {
        0,
        10,
        50,
        100,
        200,
    };

    public int[] enemyCount = new int[5] { 0, 0, 0, 0, 0 };


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
                if (level == 1)
                {
                    controller.Tutorial_2_3();
                }
            }
        }
    }
}
