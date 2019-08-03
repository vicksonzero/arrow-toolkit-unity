using UnityEngine;
using UnityEngine.UI;

public class BArrow : MonoBehaviour
{

    public LayerMask collisionMask;
    public Vector2 currVelo;
    public float currSpeed;
    public float maxVelocity = 30;
    public int coin = 0;
    public int combo = 0;
    public int level = 0;
    public float[] penetrateSpeed = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };

    public Text coinTextLabel;
    public Text speedTextLabel;

    public BArrowItem arrowItemPrefab;
    public AudioClip swooshSound;
    public AudioClip ricochetSound;


    Rigidbody2D rb;
    BPlayer player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<BPlayer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currVelo = rb.velocity;
        currSpeed = currVelo.magnitude;
        speedTextLabel.text = currSpeed.ToString("F2");
        if (currSpeed < 0.3)
        {
            Die();
        }


        if (player)
        {
            var speed = rb.velocity.magnitude;
            var distToPlayer = Vector2.Distance(player.transform.position, transform.position);
            if (speed < 2 && distToPlayer < 0.4)
            {
                Die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {

            Vector2 v = Vector2.Reflect(-collision.contacts[0].relativeVelocity, collision.contacts[0].normal);
            var angle = Vector2.SignedAngle(Vector2.right, v);
            transform.eulerAngles = new Vector3(0, 0, angle);

            var player = FindObjectOfType<BPlayer>();

            if (player)
            {
                player.PlaySound(ricochetSound);
            }

            rb.velocity = v;
        }
    }

    public void pickUpSpeed()
    {
        var amount = penetrateSpeed[level];
        rb.velocity *= amount;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void RestoreVelo()
    {
        rb.velocity = currVelo;
    }

    public void PickUpCoin(int amount)
    {
        coin += (int)Mathf.Pow(Mathf.Max(1, coin), 0.5f) * amount;
        coinTextLabel.text = "" + coin;
    }

    public void AddCombo()
    {
        combo++;
    }

    public void Die()
    {
        var ui = GameObject.FindObjectOfType<BUI>();
        ui.MaxCombo = Mathf.Max(ui.MaxCombo, combo);

        var arrowItem = Instantiate(arrowItemPrefab, transform.position, Quaternion.identity);
        arrowItem.GetComponent<BArrowItem>().ApplyCoin(coin);
        Destroy(gameObject);
    }


    public void PlaySound(AudioClip clip)
    {
        var player = FindObjectOfType<BPlayer>();
        if (player)
        {
            player.PlaySound(clip);
        }
    }
}
