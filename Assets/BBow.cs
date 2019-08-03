using UnityEngine;

public class BBow : MonoBehaviour
{
    public bool haveArrow = true;
    public BArrow arrowPrefab;

    public float arrowSpeed = 3;

    public int coin = 0;

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
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.right * arrowSpeed;

        haveArrow = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bArrowItem = collision.collider.GetComponent<BArrowItem>();
        if (bArrowItem)
        {
            coin += bArrowItem.coin;
            ui.Coins = coin;
            haveArrow = true;
            Destroy(bArrowItem.gameObject);
        }

    }
}
