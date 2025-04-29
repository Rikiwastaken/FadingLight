using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    public List<GameObject> Eyelist;

    private List<Vector2> EyeposList;

    public List<float> EyeMovementRange;

    private Transform PlayerTransf;

    public bool activateeyes;

    // Start is called before the first frame update
    void Start()
    {
        EyeposList = new List<Vector2>();
        foreach (GameObject Eye in Eyelist)
        {
            EyeposList.Add(Eye.transform.localPosition);
        }

        PlayerTransf = FindAnyObjectByType<PlayerHP>().transform; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i< Eyelist.Count; i++)
        {
            Vector2 newpos = EyeposList[i] + ((Vector2)PlayerTransf.position - EyeposList[i] - (Vector2)transform.position).normalized * EyeMovementRange[i];
            if (GetComponentInParent<SpriteRenderer>().flipX )
            {
                newpos = new Vector2(-EyeposList[i].x, EyeposList[i].y) + ((Vector2)PlayerTransf.position - EyeposList[i] - (Vector2)transform.position).normalized * EyeMovementRange[i];
            }
            
            Eyelist[i].transform.localPosition = newpos;
            if (activateeyes)
            {
                Eyelist[i].GetComponent<SpriteRenderer>().color = new Color(0.64f, 0.35f, 0.33f, 1f);

            }
            else
            {
                Eyelist[i].GetComponent<SpriteRenderer>().color = new Color(0f, 0.84f, 1f, 1f);
            }
        }
    }
}
