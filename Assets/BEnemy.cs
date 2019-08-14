using System.Collections;
using UnityEngine;

public class BEnemy : MonoBehaviour
{
    public int enemyID = -1;
    public float speed = 1;
    public float minSpeed = 1;
    public float maxSpeed = 1;

    public float followInterval = 3000;
    public float followIntervalMin = 3000;
    public float followIntervalMax = 4000;
    public bool canFollowPlayer = true;
    public bool allowStop = false;

    protected RectTransform playerTransform;
    protected Vector3 targetPosition;

    public AudioClip hitSound;

    BLevel bLevel;

    // Start is called before the first frame update
    public virtual void Start()
    {
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        followInterval = UnityEngine.Random.Range(followIntervalMin, followIntervalMax);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            playerTransform = player.GetComponent<RectTransform>();
        }
        bLevel = FindObjectOfType<BLevel>();

        bLevel.JoinLevel(enemyID);
        StartCoroutine(UpdateTargetPos());
    }

    private void OnDestroy()
    {
        if (bLevel)
        {
            bLevel.LeaveLevel(enemyID);
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
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

        if (canFollowPlayer)
        {
            StartCoroutine(UpdateTargetPos());
        }
    }

    public virtual float GetSpeed()
    {
        return speed;
    }

    public virtual IEnumerator UpdateTargetPos()
    {
        // Debug.Log("BEnemy UpdateTargetPos");
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnTriggerEnter " + collision.collider.tag);
        if (collision.collider.CompareTag("Arrow"))
        {
            if (collision.otherCollider.CompareTag("Enemy"))
            {
                var arrow = collision.collider.GetComponent<BArrow>();
                arrow.RestoreVelo();
                arrow.PickUpCoin(1);
                arrow.AddCombo();
                arrow.PickUpSpeed();
                if (hitSound)
                {
                    arrow.PlaySound(hitSound);
                }

                var shakeIntensity = arrow.currSpeed / 30;

                FindObjectOfType<BCameraShake>().Nudge(0.1f, shakeIntensity * 2f);
                var torch = GetComponent<BEnemyTorch>();
                if (torch)
                {
                    torch.willSpawnFire = true;
                }
                Destroy(gameObject);
            }
        }
    }
}
