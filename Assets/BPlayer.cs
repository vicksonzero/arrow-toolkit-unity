using System.Collections;
using UnityEngine;

public class BPlayer : MonoBehaviour
{
    public float speed = 2;
    public float proximityIndex = 0;
    public AudioSource proximitySoundLoop;
    public bool isDashing = false;
    public float dashSpeed = 5;

    public float dashDuration = 1;

    public bool canDash = true;
    public float dashCooldown = 0.1f;

    public float dashCommandSensitivity = 0.5f;
    private Vector2 dashCommandLastRawInput = Vector2.zero;
    private Vector2 dashCommandLastCommand = Vector2.zero;
    public bool dashCommandWasReleasedBefore = false;

    public SpriteRenderer dashAfterImage;
    public bool canSpawnAfterImage = true;
    public float spawnAfterImageInterval = 0.2f;
    public float spawnAfterImageDuration = 0.5f;

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
        // handle command dash
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var inputIsReleased = (dashCommandLastRawInput != moveInput && moveInput == Vector2.zero);
        var inputIsPressed = (dashCommandLastRawInput == Vector2.zero && moveInput != Vector2.zero);
        if (!isDashing && canDash && !bow.haveArrow && inputIsReleased)
        {
            dashCommandLastCommand = dashCommandLastRawInput;
            StartCoroutine(KeepDashCommandState());
        }

        if (canDash && !bow.haveArrow && inputIsPressed && dashCommandLastCommand == moveInput)
        {
            // print("direction input released");
            if (isDashing)
            {
                if (canDash)
                {
                    StartCoroutine(DoDashCooldown());
                }
            }
            if (dashCommandWasReleasedBefore)
            {
                StartCoroutine(DashThenReset());
            }
            dashCommandLastCommand = dashCommandLastRawInput;
        }

        // cancel any dashing if have arrow
        if (bow.haveArrow)
        {
            isDashing = false;
        }
        if (isDashing && moveInput == Vector2.zero)
        {
            isDashing = false;
            if (canDash)
            {
                StartCoroutine(DoDashCooldown());
            }
        }

        // dash effect
        if (isDashing && canSpawnAfterImage)
        {
            var _afterImage = Instantiate(dashAfterImage, transform.position, transform.rotation);
            Destroy(_afterImage, spawnAfterImageDuration);
            StartCoroutine(ThrottleAfterImage());
        }

        // handle actual moving
        var _speed = (isDashing ? dashSpeed : speed);
        if (!isDashing && bow.isCharging)
        {
            _speed *= 0.7f;
        }
        moveVelocity = moveInput.normalized * _speed;
        dashCommandLastRawInput = moveInput;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        if (bow.isCharging)
        {
            var angle = Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (moveVelocity != Vector2.zero)
        {
            var angle = Vector2.SignedAngle(Vector2.right, moveVelocity);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

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

    IEnumerator DashThenReset()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        if (canDash)
        {
            StartCoroutine(DoDashCooldown());
        }
    }
    IEnumerator DoDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator KeepDashCommandState()
    {
        dashCommandWasReleasedBefore = true;
        yield return new WaitForSeconds(dashCommandSensitivity);
        dashCommandWasReleasedBefore = false;

    }

    IEnumerator ThrottleAfterImage()
    {
        canSpawnAfterImage = false;
        yield return new WaitForSeconds(spawnAfterImageInterval);
        canSpawnAfterImage = true;

    }
}
