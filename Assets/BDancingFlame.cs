using System.Collections;
using UnityEngine;

public class BDancingFlame : MonoBehaviour
{
    float startingScaleX;
    bool canUpdateScale = true;
    // Start is called before the first frame update
    void Start()
    {
        startingScaleX = transform.localScale.x;

    }
    // Update is called once per frame
    void Update()
    {
        if (canUpdateScale)
        {
            StartCoroutine(UpdateScaleX());
        }
    }

    IEnumerator UpdateScaleX()
    {
        var sx = transform.localScale.x == startingScaleX ? startingScaleX * 0.9f : startingScaleX;
        transform.localScale = new Vector2(sx, transform.localScale.y);

        canUpdateScale = false;
        yield return new WaitForSeconds(0.1f);
        canUpdateScale = true;
    }
    public void UpdateBaseScale(Vector2 newScale)
    {
        transform.parent.localScale = newScale;
    }
}
