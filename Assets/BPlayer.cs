using UnityEngine;

public class BPlayer : MonoBehaviour
{
    public float speed = 2;
    public float proximityIndex;
    public AudioSource proximitySoundLoop;

    Rigidbody2D rb;
    BBow bow;
    Vector2 moveVelocity;

    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        bow = GetComponent<BBow>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed * (bow.isCharging ? 0.5f : 1);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        var closestEnemy = FindClosestEnemy();
        if (closestEnemy)
        {
            proximityIndex = 1 - Mathf.Max(0, Vector2.Distance(closestEnemy.transform.position, transform.position) / 3);
            proximitySoundLoop.volume = 1f * proximityIndex;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnTriggerEnter " + collision.collider.tag);
        if (collision.collider.CompareTag("Enemy"))
        {
            GameObject.FindObjectOfType<BController>().GameOver(this);
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        sound.PlayOneShot(clip);
    }
    public BEnemy FindClosestEnemy()
    {
        var gos = GameObject.FindObjectsOfType<BEnemy>();
        BEnemy closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (var go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
