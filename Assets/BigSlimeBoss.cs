using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeBoss : MonoBehaviour
{

    public GameObject HorizontalSpike;
    public GameObject VerticalSpike;
    public float yforvertical;
    public int verticalspikedamage;
    public int cnt;
    public Vector2 WheretoSpawnLowHorizontal;
    public Vector2 WheretoSpawnHighHorizontal;

    private void Start()
    {
        SpawnLineOfSpikes();
    }

    private void FixedUpdate()
    {
        if(cnt == 240)
        {
            SpawnLineOfSpikesMoved();
        }
        if (cnt == 440)
        {
            SpawnLineOfSpikesMoved();
        }
        cnt++;
    }

    void SpawnLineOfSpikes()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f/3f - i*2f/3f, yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i* 2f / 3f, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLineOfSpikesMoved()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f / 3f - i * 2f / 3f -1f/3f, yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i * 2f / 3f + 1f / 3f, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLineOfSpikesMovedAgain()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f / 3f - i * 2f / 3f - 2f / 3f, yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i * 2f / 3f + 2f / 3f, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLowHorizontalSpike()
    {

    }

}
