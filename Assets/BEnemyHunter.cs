using System.Collections;
using UnityEngine;

public class BEnemyHunter : BEnemy
{
    public float runningSpeed = 1;
    public BEmote exclamationMarkPrefab;

    public override void Start()
    {
        base.Start();
        var _exclamationMark = Instantiate(exclamationMarkPrefab);
        _exclamationMark.target = transform;
        GetComponent<BEnemyBow>().exclamationMarkSprite = _exclamationMark.GetComponentInChildren<SpriteRenderer>();
    }
    public override void FixedUpdate()
    {
        var _speed = GetSpeed();
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.fixedDeltaTime);

        var displacement = targetPosition - transform.position;
        if (displacement.magnitude > 0.1f)
        {
            var angle = Vector2.SignedAngle(Vector2.right, displacement);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1 && !allowStop)
        {
            canFollowPlayer = true;
        }

        var arrowItem = FindObjectOfType<BArrowItem>();

        if (arrowItem || canFollowPlayer)
        {
            StartCoroutine(UpdateTargetPos());
        }

        var arrow = FindObjectOfType<BArrow>();

        if (arrow)
        {
            var arrowRb = arrow.GetComponent<Rigidbody2D>();
            var speed = arrowRb.velocity.magnitude;

            if (speed < 2)
            {
                var angle = Vector2.SignedAngle(Vector2.right, arrow.transform.position - transform.position);
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
        }
        var bow = GetComponent<BEnemyBow>();
        if (bow.isCharging && playerTransform)
        {
            var angle = Vector2.SignedAngle(Vector2.right, playerTransform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    public override IEnumerator UpdateTargetPos()
    {
        Debug.Log("BEnemyHunter UpdateTargetPos");

        var arrowItem = FindObjectOfType<BArrowItem>();

        if (arrowItem)
        {
            targetPosition = arrowItem.transform.position;
        }
        else
        {
            targetPosition = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 0);
        }
        canFollowPlayer = false;
        yield return new WaitForSeconds(followInterval / 1000);
        canFollowPlayer = true;
    }

    public override float GetSpeed()
    {
        var arrowItem = FindObjectOfType<BArrowItem>();

        if (arrowItem)
        {
            return runningSpeed;
        }
        return speed;
    }
}
