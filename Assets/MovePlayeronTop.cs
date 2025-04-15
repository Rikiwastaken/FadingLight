using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayeronTop : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerHP>() != null && transform.position.y <= collision.transform.position.y)
        {
            int direction = (int)((collision.transform.position.x - transform.position.x) / Mathf.Abs(collision.transform.position.x - transform.position.x));
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 10, 15), ForceMode2D.Impulse);
        }
    }
}
