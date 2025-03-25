using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetaTestScript : MonoBehaviour
{

    private bool Npressed;

    public GameObject Bobby;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N) && !Npressed)
        {
            Npressed = true;
            Instantiate(Bobby, transform.position + new Vector3(1f,0f,0f)*transform.localScale.x,Quaternion.identity);
        }

        if(!Input.GetKeyDown(KeyCode.N))
        {
            Npressed=false;
        }
    }
}
