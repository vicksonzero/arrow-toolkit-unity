using UnityEngine;

public class BEmote : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = target.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
