using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechDroneScript : MonoBehaviour
{
    public Transform target;
    public float detectionrange;
    public Transform launcher;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            transform.position = launcher.transform.position + (target.transform.position - launcher.transform.position)/2f;
            Vector3 offset = target.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the target.
                                 );
            transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
            transform.localScale = new Vector3(Vector2.Distance((Vector2)launcher.transform.position, (Vector2)target.transform.position), 0.3f, 1f);
            if(Vector2.Distance((Vector2)launcher.transform.position, (Vector2)target.transform.position)>detectionrange)
            {
                target=null;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        

    }
}
