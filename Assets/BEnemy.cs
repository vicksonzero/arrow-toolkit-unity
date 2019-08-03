using System;
using System.Threading.Tasks;
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

    // Start is called before the first frame update
    void Start()
    {
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        followInterval = UnityEngine.Random.Range(followIntervalMin, followIntervalMax);
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>();
        UpdateTargetPos();
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
            UpdateTargetPos();
        }
    }

    async void UpdateTargetPos()
    {
        targetPosition = playerTransform.position;
        canFollowPlayer = false;
        await Task.Delay(TimeSpan.FromMilliseconds(followInterval));
        canFollowPlayer = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnTriggerEnter " + collision.collider.tag);
        if (collision.collider.CompareTag("Arrow"))
        {
            collision.collider.GetComponent<BArrow>().RestoreVelo();
            collision.collider.GetComponent<BArrow>().PickUpCoin(1);
            collision.collider.GetComponent<BArrow>().pickUpSpeed(0.5f);

            Destroy(gameObject);
        }
    }
}
