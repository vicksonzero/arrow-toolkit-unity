using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BAimAssist : MonoBehaviour
{
    public float[] scanDistances = new float[] { 5, 10, 20, 30 };
    public LayerMask raycastFilterWallShield;
    public LayerMask raycastFilterWall;
    public string debugString;


    LineRenderer lr;
    BBow bow;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        bow = GetComponentInParent<BBow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bow.isCharging)
        {
            lr.positionCount = 0;
            return;
        }
        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);

        float remainingDistance = scanDistances[bow.chargeLevel];

        Vector3 lastHitPoint = transform.position;
        lastHitPoint.z = 0;
        Vector3 reflectAngle = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        reflectAngle.z = 0;

        debugString = "";
        //out hit, remainingDistance
        RaycastHit2D hit;
        var remainingTry = 20;
        while (remainingTry > 0 && remainingDistance > 0)
        {
            hit = Physics2D.Raycast(lastHitPoint, reflectAngle, remainingDistance, raycastFilterWallShield);

            if (!hit)
            {
                break;
            }

            if (!hit.collider.CompareTag("Wall"))
            {
                hit = Physics2D.Raycast(lastHitPoint, reflectAngle, remainingDistance, raycastFilterWall);
            }

            reflectAngle = Vector3.Reflect(reflectAngle, hit.normal);
            reflectAngle.z = 0;
            lastHitPoint = hit.point + hit.normal * 0.001f;
            lastHitPoint.z = 0;
            debugString += remainingTry + ": " + hit.normal + "," + reflectAngle + "," + remainingDistance;

            points.Add(hit.point);
            remainingDistance -= hit.distance;
            remainingTry--;
            debugString += remainingDistance + "; ";
        }

        if (remainingDistance > 0)
        {
            points.Add(lastHitPoint + reflectAngle * remainingDistance);
        }
        lr.SetPositions(points.ToArray());
        lr.positionCount = points.Count;
    }
}
