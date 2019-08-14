using System.Collections;
using UnityEngine;

public class BEnemyBow : MonoBehaviour
{
    public bool haveArrow = true;
    public GameObject haveArrowIndicator;
    public ParticleSystem haveArrowIndicatorParticle;
    public bool isCharging = false;
    public GameObject chargingIndicator;

    public int chargeLevel = 0;
    public BArrow arrowPrefab;
    public float chargeStartTime;
    float[] chargeArrowSpeeds = new float[] { 3, 6, 10, 20 };

    public ParticleSystem chargeParticles;
    public ParticleSystem chargeLevelParticles;

    public int level = 0;

    public AudioClip pickUpSound;
    public AudioClip[] chargingSounds;
    public AudioClip electricSound;

    public SpriteRenderer exclamationMarkSprite;


    public int cachedCoin;
    public int cachedCombo;
    public int cachedBounceLevel;

    BUI ui;
    BPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindObjectOfType<BUI>();
        player = FindObjectOfType<BPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (haveArrow)
        {
            if (!isCharging)
            {
                StartCoroutine(ChargeAndShoot());
            }
        }
        else
        {
            var arrow = FindObjectOfType<BArrow>();
            var arrowItem = FindObjectOfType<BArrowItem>();

            if (!arrow && !arrowItem)
            {
                exclamationMarkSprite.enabled = false;
            }
            else
            {
                var speed = ((!arrow) ? 100 : arrow.GetComponent<Rigidbody2D>().velocity.magnitude);

                exclamationMarkSprite.enabled = (speed < 2 || arrowItem != null);
            }

        }
        if (isCharging)
        {
            if (chargeLevel == 0 && Time.time - chargeStartTime >= 0.5)
            {
                chargeLevel++;

                FindObjectOfType<BPlayer>().PlaySound(chargingSounds[0]);
                chargeLevelParticles.Play();
            }

            if (chargeLevel == 1 && Time.time - chargeStartTime >= 1.5)
            {
                chargeLevel++;
                FindObjectOfType<BPlayer>().PlaySound(chargingSounds[1]);
                chargeLevelParticles.Play();
            }
            if (chargeLevel == 2 && Time.time - chargeStartTime >= 2.5)
            {
                chargeLevel++;
                FindObjectOfType<BPlayer>().PlaySound(chargingSounds[2]);
                chargeLevelParticles.Play();
            }
        }
    }

    IEnumerator ChargeAndShoot()
    {
        isCharging = true;
        UpdateArrowIndicator();
        chargeParticles.Play();
        chargeStartTime = Time.time;

        yield return new WaitForSeconds(3);
        if (isCharging)
        {
            ShootArrow();
            isCharging = false;
            UpdateArrowIndicator();
            chargeParticles.Stop();
            chargeLevel = 0;
        }
    }


    public void ShootArrow()
    {
        if (!player)
        {
            return;
        }

        var arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        var angle = Vector2.SignedAngle(Vector2.right, player.transform.position - arrow.transform.position);
        arrow.transform.eulerAngles = new Vector3(0, 0, angle);
        arrow.GetComponent<BArrow>().level = chargeLevel;
        arrow.GetComponent<BArrow>().coin = cachedCoin;
        arrow.GetComponent<BArrow>().combo = cachedCombo;
        arrow.GetComponent<BArrow>().bounceLevel = cachedBounceLevel;

        arrow.GetComponent<BArrow>().SetToDark();

        var _arrowSpeed = chargeArrowSpeeds[chargeLevel];
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.right * _arrowSpeed;

        haveArrow = false;
        UpdateArrowIndicator();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bArrowItem = collision.collider.GetComponent<BArrowItem>();
        if (bArrowItem)
        {
            cachedCoin = bArrowItem.coin;
            cachedCombo = bArrowItem.combo;
            cachedBounceLevel = bArrowItem.bounceLevel;

            haveArrow = true;
            UpdateArrowIndicator();
            haveArrowIndicatorParticle.Play();
            FindObjectOfType<BPlayer>().PlaySound(pickUpSound);
            Destroy(bArrowItem.gameObject);
        }

    }

    public void UpdateArrowIndicator()
    {
        haveArrowIndicator.SetActive(haveArrow && !isCharging);
        chargingIndicator.SetActive(haveArrow && isCharging);
    }

}
