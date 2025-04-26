using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    public List<GameObject> Eyelist;

    private List<Vector2> EyeposList;

    public List<float> EyeMovementRange;

    private Transform PlayerTransf;


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
            Vector2 newpos = EyeposList[i] + ((Vector2)PlayerTransf.position - EyeposList[i] - (Vector2)transform.position).normalized*EyeMovementRange[i];
            Eyelist[i].transform.localPosition = newpos;
        }
    }
}
