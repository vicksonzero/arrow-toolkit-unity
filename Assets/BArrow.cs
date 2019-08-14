using UnityEngine;
using UnityEngine.UI;

public class BArrow : MonoBehaviour
{
    public bool isDark = false;
    public LayerMask collisionMask;
    public Vector2 currVelo;
    public float currSpeed;
    public float maxVelocity = 30;
    public int coin = 0;
    public int combo = 0;
    public int level = 0;
    public int bounceLevel = 0;
    public float[] chargeArrowSpeeds = new float[] { 3, 10, 20, 30 };
    public float[] penetrateSpeed = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };
    public float[] bounceBonus = new float[] { 0.8f, 0.8f, 0.8f, 0.8f };

    public Text coinTextLabel;
    public Text speedTextLabel;

    public BArrowItem arrowItemPrefab;
    public AudioClip swooshSound;
    public AudioClip ricochetSound;

    public SpriteRenderer flameSprite;
    public ParticleSystem darkParticles;


    Rigidbody2D rb;
    BPlayer player;
    TrailRenderer trail;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<BPlayer>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        coinTextLabel.text = ""; // "+1"
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

        if (isDark && currSpeed <= 2)
        {
            UndoDark();
        }


        if (player)
        {
            var distToPlayer = Vector2.Distance(player.transform.position, transform.position);
            if (distToPlayer < 0.4)
            {
                if (currSpeed < 2)
                {
                    Die();
                }
                else if (isDark)
                {
                    GameObject.FindObjectOfType<BController>().GameOver(player);
                    Destroy(player.gameObject);
                    Destroy(gameObject);
                }
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

            FindObjectOfType<BController>().PlaySound(ricochetSound);

            rb.velocity = v * bounceBonus[level];
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
            bounceLevel++;
            //coinTextLabel.text = "+" + GetCoinBonus1();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("CampFire"))
        {
            LightOnFire(collision.GetComponentInParent<BCampFire>());
        }
    }

    public void PickUpSpeed()
    {
        var amount = penetrateSpeed[level];
        rb.velocity *= amount;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void RestoreVelo()
    {
        rb.velocity = currVelo;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void PickUpCoin(int amount)
    {
        coin += GetCoinBonus1() * amount;
        coinTextLabel.text = "" + coin;
    }

    public int GetCoinBonus1()
    {
        return (int)Mathf.Pow(Mathf.Max(1, coin), 0.5f);
    }


    public int GetCoinBonus2()
    {
        return (int)Mathf.Pow(Mathf.Max(1, bounceLevel), 0.7f);
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
        arrowItem.GetComponent<BArrowItem>().ApplyStat(combo, level, bounceLevel);

        Destroy(gameObject);
    }


    public void PlaySound(AudioClip clip)
    {
        FindObjectOfType<BController>().PlaySound(clip);
    }

    public void LightOnFire(BCampFire campFire)
    {
        if (flameSprite && !flameSprite.gameObject.activeSelf)
        {
            flameSprite.gameObject.SetActive(true);
            if (level < 3)
            {
                level++;
                if (campFire)
                {
                    campFire.TakeDamage();
                }
            }
        }

        rb.velocity = rb.velocity.normalized * chargeArrowSpeeds[level];
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void SetToDark()
    {
        isDark = true;
        darkParticles.Play();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.startColor = Color.black;
    }

    public void UndoDark()
    {
        isDark = false;
        darkParticles.Stop();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.startColor = Color.white;
    }
}
