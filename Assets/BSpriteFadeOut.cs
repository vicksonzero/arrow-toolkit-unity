using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpriteFadeOut : MonoBehaviour
{
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sprite)
        {
            var color = sprite.color;
            color.a *= 0.95f;
            sprite.color = color;
        }
    }
}
