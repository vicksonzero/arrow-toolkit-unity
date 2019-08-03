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

    public Text coinTextLabel;

    public BArrowItem arrowItemPrefab;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currVelo = rb.velocity;
        currSpeed = currVelo.magnitude;
        if (currSpeed < 0.2)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {

            Vector2 v = Vector2.Reflect(-collision.contacts[0].relativeVelocity, collision.contacts[0].normal);
            var angle = Vector2.SignedAngle(Vector2.right, v);
            transform.eulerAngles = new Vector3(0, 0, angle);


            rb.velocity = v;
        }
    }

    public void pickUpSpeed(float amount)
    {
        rb.velocity *= amount;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void RestoreVelo()
    {
        rb.velocity = currVelo;
    }

    public void PickUpCoin(int amount)
    {
        coin += (int)Mathf.Pow(1.2f, coin) * amount;
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
}
