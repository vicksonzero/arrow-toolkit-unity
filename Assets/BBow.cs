﻿using UnityEngine;

public class BBow : MonoBehaviour
{
    public bool haveArrow = true;
    public bool isCharging = false;
    public int chargeLevel = 0;
    public BArrow arrowPrefab;
    public float chargeStartTime;
    public float[] chargeArrowSpeeds;
    public ParticleSystem chargeParticles;

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
                chargeLevel = 0;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (haveArrow)
            {
                isCharging = true;
                chargeStartTime = Time.time;
            }
        }
        if (isCharging)
        {
            if (chargeLevel == 0 && Time.time - chargeStartTime >= 0.5)
            {
                chargeLevel++;

                GetComponent<BPlayer>().PlaySound(chargingSounds[0]);
                chargeParticles.Play();
            }
            if (chargeLevel == 1 && Time.time - chargeStartTime >= 1.5)
            {
                chargeLevel++;
                GetComponent<BPlayer>().PlaySound(chargingSounds[1]);
                chargeParticles.Play();
            }
            if (chargeLevel == 2 && Time.time - chargeStartTime >= 2.5)
            {
                chargeLevel++;
                GetComponent<BPlayer>().PlaySound(chargingSounds[2]);
                chargeParticles.Play();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bArrowItem = collision.collider.GetComponent<BArrowItem>();
        if (bArrowItem)
        {
            ui.Coins += bArrowItem.coin;
            haveArrow = true;
            GetComponent<BPlayer>().PlaySound(pickUpSound);
            Destroy(bArrowItem.gameObject);
        }

    }
    
}
