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

    RectTransform playerTransform;
    Vector3 targetPosition;

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

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1)
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
            var arrow = collision.collider.GetComponent<BArrow>();
            arrow.RestoreVelo();
            arrow.PickUpCoin(1);
            arrow.AddCombo();
            arrow.pickUpSpeed(0.5f);
            if (hitSound)
            {
                arrow.PlaySound(hitSound);
            }

            Destroy(gameObject);
        }
    }
}
