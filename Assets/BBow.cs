using UnityEngine;

public class BBow : MonoBehaviour
{
    public bool haveArrow = true;
    public GameObject haveArrowIndicator;
    public ParticleSystem haveArrowIndicatorParticle;
    public bool isCharging = false;
    public GameObject chargingIndicator;
    
    public int chargeLevel = 0;
    public BArrow arrowPrefab;
    public float chargeStartTime;
    float[] chargeArrowSpeeds = new float[] { 3, 10, 20, 30 };

    public ParticleSystem chargeParticles;
    public ParticleSystem chargeLevelParticles;

    public int level = 0;

    public AudioClip pickUpSound;
    public AudioClip[] chargingSounds;

    BUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindObjectOfType<BUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isCharging)
            {
                ShootArrow();
                isCharging = false;
                UpdateArrowIndicator();
                chargeParticles.Stop();
                chargeLevel = 0;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (haveArrow)
            {
                isCharging = true;
                UpdateArrowIndicator();
                chargeParticles.Play();
                chargeStartTime = Time.time;
            }
        }
        if (isCharging)
        {
            if (chargeLevel == 0 && Time.time - chargeStartTime >= 0.5)
            {
                chargeLevel++;

                FindObjectOfType<BController>().PlaySound(chargingSounds[0]);
                chargeLevelParticles.Play();
            }
            if (chargeLevel == 1 && Time.time - chargeStartTime >= 1.5)
            {
                chargeLevel++;
                FindObjectOfType<BController>().PlaySound(chargingSounds[1]);
                chargeLevelParticles.Play();
            }
            if (chargeLevel == 2 && Time.time - chargeStartTime >= 2.5)
            {
                chargeLevel++;
                FindObjectOfType<BController>().PlaySound(chargingSounds[2]);
                chargeLevelParticles.Play();
            }
        }
    }


    public void ShootArrow()
    {
        var arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        var angle = Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrow.transform.position);
        arrow.transform.eulerAngles = new Vector3(0, 0, angle);
        arrow.GetComponent<BArrow>().level = chargeLevel;
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
            ui.Coins += bArrowItem.coin;
            haveArrow = true;
            UpdateArrowIndicator();
            haveArrowIndicatorParticle.Play();
            FindObjectOfType<BController>().PlaySound(pickUpSound);
            Destroy(bArrowItem.gameObject);
        }

    }

    public void UpdateArrowIndicator()
    {
        haveArrowIndicator.SetActive(haveArrow && !isCharging);
        chargingIndicator.SetActive(haveArrow && isCharging);
    }

}
