using System.Collections;
using UnityEngine;

public class BEnemyHunter : BEnemy
{
    IEnumerator UpdateTargetPos()
    {
        Debug.Log("BEnemyHunter UpdateTargetPos");
        if (playerTransform == null)
        {
            canFollowPlayer = false;
        }
        else
        {
            targetPosition = playerTransform.position;
            canFollowPlayer = false;
            yield return new WaitForSeconds(followInterval / 1000);
            canFollowPlayer = true;
        }
    }
}
