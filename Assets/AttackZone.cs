using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public int damage;
    public float time;
    private int timecounter;
    public Vector2 offset;
    void Start()
    {
        transform.position = position + (Vector3)offset;
        timecounter = (int)(time/Time.deltaTime);
        if (rotation != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
        if (scale != Vector3.zero)
        {
            transform.localScale = scale;
        }
        else
        {
            transform.localScale = new Vector3(PrefabToSpawn.transform.localScale.x, 1f, 1f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timecounter==0)
        {
            GameObject newattack =Instantiate(PrefabToSpawn,position,Quaternion.identity);
            if(newattack.GetComponent<spikescript>())
            {
                newattack.GetComponent<spikescript>().damage = damage;
            }
            if(rotation!=Vector3.zero)
            {
                newattack.transform.rotation = Quaternion.Euler(rotation);
            }
            if(scale!=Vector3.zero)
            {
                newattack.transform.localScale = scale;
            }
            Destroy(gameObject);
        }
        else
        {
            timecounter--;
        }
    }

    public void InstantiateObject(GameObject Prefab,Vector3 pos, int dmg, float timebeforeattack, Vector2 newoffset)
    {
        PrefabToSpawn=Prefab;
        position = pos;
        damage = dmg;
        rotation = Vector3.zero;
        scale = Vector3.zero;
        time = timebeforeattack;
        offset = newoffset;
    }
    public void InstantiateObject(GameObject Prefab, Vector3 pos, int dmg, float timebeforeattack, Vector3 newscale, Vector3 rot, Vector2 newoffset)
    {
        PrefabToSpawn = Prefab;
        position = pos;
        damage = dmg;
        rotation = rot;
        scale = newscale;
        time = timebeforeattack;
        offset = newoffset;
    }
}
