using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadphonesScript : MonoBehaviour
{
    private optionvalues options;
    private float basex;
    private float basescalex;
    void Start()
    {
        options = FindAnyObjectByType<optionvalues>();
        basex = transform.localPosition.x;
        basescalex = transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(options != null)
        {
            if(options.options.headphones)
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        if(transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            transform.localPosition = new Vector2(-basex, transform.localPosition.y);
            transform.localScale = new Vector2(-basescalex, transform.localScale.y);
        }
        else
        {
            transform.localPosition = new Vector2(basex, transform.localPosition.y);
            transform.localScale = new Vector2(basescalex, transform.localScale.y);
        }
    }
}
