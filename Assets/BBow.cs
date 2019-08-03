using UnityEngine;

public class BBow : MonoBehaviour
{
    public bool haveArrow = true;
    public BArrow arrowPrefab;

    public float arrowSpeed = 3;

    public int level = 0;

    public AudioClip pickUpSound;

    BUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindObjectOfType<BUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (haveArrow)
            {
                ShootArrow();
            }
        }
    }


    public void ShootArrow()
    {
        var arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        var angle = Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrow.transform.position);
        arrow.transform.eulerAngles = new Vector3(0, 0, angle);
        arrow.GetComponent<BArrow>().level = level;
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.right * arrowSpeed;

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

    public void UpgradeArrow()
    {
        if (level < 3)
        {
            level++;
        }

        switch (level)
        {
            case 1:
                arrowSpeed = 15;
                break;
            case 2:
                arrowSpeed = 20;
                break;
            case 3:
                arrowSpeed = 30;
                break;
            default:
                arrowSpeed = 10;
                break;
        }
    }
}
