using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallUnstickingScript : MonoBehaviour
{
    public LayerMask wallLayer; // Assigne "wall" et "ground" ici via l'inspecteur
    public List<Vector2> notinthemur = new List<Vector2>();

    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        for(int i = 0;i<100;i++)
        {
            notinthemur.Add(Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        TryUnstuck();
    }

    void TryUnstuck()
    {
        Bounds originalBounds = boxCollider.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(originalBounds.center, originalBounds.size * 0.95f, 0f, wallLayer);

        if (hits.Length == 0)
        {
            for(int i=1;i<notinthemur.Count;i++)
            {
                notinthemur[i]=notinthemur[i-1];
            }
            notinthemur[0] = originalBounds.center;
        }
        else
        {
            for (int i = 30; i < notinthemur.Count; i++)
            {
                if (Physics2D.OverlapBoxAll(notinthemur[i], originalBounds.size * 0.95f, 0f, wallLayer).Length==0)
                {
                    Debug.Log("joueur sauvé");
                    transform.position = notinthemur[i];
                }
            }
            
        }
    }
}
