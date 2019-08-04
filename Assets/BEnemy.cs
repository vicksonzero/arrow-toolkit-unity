using System.Collections;
using UnityEngine;

public class BEnemy : MonoBehaviour
{

    public float speed = 1;
    public float minSpeed = 1;
    public float maxSpeed = 1;

    public float followInterval = 3000;
    public float followIntervalMin = 3000;
    public float followIntervalMax = 4000;
    public bool canFollowPlayer = true;
    public GameObject shield;

    protected RectTransform playerTransform;
    protected Vector3 targetPosition;

    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        followInterval = UnityEngine.Random.Range(followIntervalMin, followIntervalMax);
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>();
        StartCoroutine(UpdateTargetPos());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        var angle = Vector2.SignedAngle(Vector2.right, targetPosition - transform.position);
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1 && shield == null)
        {
            canFollowPlayer = true;
        }

        if (canFollowPlayer)
        {
            StartCoroutine(UpdateTargetPos());
        }
    }

    IEnumerator UpdateTargetPos()
    {
        Debug.Log("BEnemy UpdateTargetPos");
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
                Destroy(gameObject);
            }
        }
    }
}
