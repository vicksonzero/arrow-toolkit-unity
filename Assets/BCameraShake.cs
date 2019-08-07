using UnityEngine;

public class BCameraShake : MonoBehaviour
{
    public float shakeEnd = 0;
    public Camera myCamera;

    public float masterShakeIntensity = 0.5f;
    float shakeIntensity = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        if (!myCamera)
        {
            myCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (shakeEnd > Time.time)
        {
            if (shakeIntensity > 0)
            {
                myCamera.transform.localPosition = new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
            }
        }
        else
        {
            myCamera.transform.localPosition = Vector3.zero;
        }
    }
    public void Shake(float duration, float intensity)
    {
        if (duration == 0)
        {
            myCamera.transform.localPosition = new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
        }
        else
        {
            shakeIntensity = masterShakeIntensity * intensity;
            shakeEnd = Mathf.Max(Time.time + duration, shakeEnd);
        }
    }
    public void Nudge(float duration, float intensity)
    {
        shakeIntensity = masterShakeIntensity * intensity;
        myCamera.transform.localPosition = Random.insideUnitCircle.normalized * shakeIntensity + new Vector2(myCamera.transform.localPosition.x, myCamera.transform.localPosition.y);
        shakeIntensity = 0;
        shakeEnd = Mathf.Max(Time.time + duration, shakeEnd);
    }
}
