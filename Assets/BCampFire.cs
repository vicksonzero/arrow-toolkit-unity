using System.Collections;
using UnityEngine;

public class BCampFire : MonoBehaviour
{
    public int hp;
    public int maxHP = 3;

    public BDancingFlame flame;
    public float[] flameScales = new float[] { 0, 0.3f, 0.7f, 1 };
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        StartCoroutine(StartFlameLater());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartFlameLater()
    {
        flame.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        if (flame && flame.gameObject)
        {
            flame.gameObject.SetActive(true);
        }
    }

    public void TakeDamage()
    {
        hp--;
        if (hp == 0)
        {
            Destroy(gameObject);
        }
        flame.UpdateBaseScale(Vector2.one * flameScales[hp]);
    }

}
