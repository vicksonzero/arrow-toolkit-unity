using UnityEngine;

public class BZombieExplode : MonoBehaviour
{
    public SpriteRenderer zombieBodySprite;
    public float proximityIndex;
    private float shakeIntensity = 0.03f;
    public AudioSource fuseSound;
    public float fuse = 0;
    public float fuseLength = 1.5f;
    public BExplosion explosion;

    BPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<BPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            proximityIndex = 1 - Mathf.Max(0, Vector2.Distance(player.transform.position, transform.position) / 3);
            proximityIndex = Mathf.Pow(proximityIndex, 3);
            zombieBodySprite.color = Color.HSVToRGB(0, proximityIndex, 1);
            if (proximityIndex > 0.35)
            {
                zombieBodySprite.transform.localPosition = new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));

                var angle = Vector2.SignedAngle(Vector2.right, player.transform.position - transform.position);
                transform.eulerAngles = new Vector3(0, 0, angle);

                fuse += Time.deltaTime;

                if (!fuseSound.isPlaying)
                {
                    fuseSound.Play();
                }

                if (fuse >= fuseLength)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            else
            {
                fuse = 0;
                fuseSound.Stop();
            }
        }
    }
}
