using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeBoss : MonoBehaviour
{

    public GameObject HorizontalSpike;
    public GameObject VerticalSpike;
    public GameObject Crystal;
    public float yforvertical;
    public int verticalspikedamage;
    public int HorizontalSpikeDamage;
    public int CrystalDamage;
    public Vector3 WheretoSpawnLowHorizontal;
    public Vector3 WheretoSpawnHighHorizontal;
    public Vector3 WheretoSpawnCloseRangeSpikes;
    public Vector3 WheretoSpawnCrystal;
    private Global Global;

    private int attackcooldowncounter;
    public float attackcooldown;

    private Transform player;
    public float distancextotriggermelee;

    private void FixedUpdate()
    {
        if (GetComponent<EnemyHP>().activated)
        {
            if(player == null)
            {
                player = FindAnyObjectByType<PlayerMovement>().transform;
            }


            if(attackcooldowncounter <= 0 && !GetComponent<EnemyHP>().execution && !GetComponent<EnemyHP>().bossdying)
            {
                if(Mathf.Abs(player.position.x-transform.position.x)<=distancextotriggermelee)
                {
                    RandomAttackClose();
                }
                else
                {
                    RandomAttackFar();
                }
            }
            else
            {
                attackcooldowncounter--;
            }
        }
    }



    void RandomAttackClose()
    {
        int rd = Random.Range(0, 120);
        if (rd <= 5)
        {
            SpawnLineOfSpikes();
        }
        else if (rd <= 10)
        {
            SpawnLineOfSpikesMoved();
        }
        else if (rd <= 15)
        {
            SpawnLineOfSpikesMovedAgain();
        }
        else if (rd <= 20)
        {
            SpawnLowHorizontalSpike();
        }
        else if (rd <= 45)
        {
            SpawnHighHorizontalSpike();
        }
        else if (rd <= 105)
        {
            SpawnCrystal();
        }
        else
        {
            SpawnCloseRangeSpikes();
        }
        attackcooldowncounter = (int)(attackcooldown/Time.deltaTime) + Random.Range(-(int)((attackcooldown / Time.deltaTime)/3), (int)((attackcooldown / Time.deltaTime) / 3));
    }

    void RandomAttackFar()
    {
        int rd = Random.Range(0, 120);
        if(rd<=20)
        {
            SpawnLineOfSpikes();
        }
        else if(rd<=40)
        {
            SpawnLineOfSpikesMoved();
        }
        else if (rd <= 60)
        {
            SpawnLineOfSpikesMovedAgain();
        }
        else if(rd <= 105)
        {
            SpawnLowHorizontalSpike();
        }
        else
        {
            SpawnHighHorizontalSpike();
        }
        attackcooldowncounter = (int)(attackcooldown / Time.deltaTime) + Random.Range(-(int)((attackcooldown / Time.deltaTime) / 3), (int)((attackcooldown / Time.deltaTime) / 3));
    }

    void SpawnLineOfSpikes()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f/3f - i , yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLineOfSpikesMoved()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f / 3f - i  -2f/3f, yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i  + 1f / 3f, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLineOfSpikesMovedAgain()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject VerSpike = Instantiate(VerticalSpike, new Vector3(transform.position.x - 2f / 3f - i*0.75f, yforvertical, -1), Quaternion.identity);
            VerSpike.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            GameObject VerSpike2 = Instantiate(VerticalSpike, new Vector3(transform.position.x + 2f / 3f + i * 0.75f, yforvertical, -1), Quaternion.identity);
            VerSpike2.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
        }
    }

    void SpawnLowHorizontalSpike()
    {
        GameObject HorSpike = Instantiate(HorizontalSpike, transform.position+WheretoSpawnLowHorizontal, Quaternion.identity);
        HorSpike.GetComponent<SlimeBossSpike>().damage = HorizontalSpikeDamage;
        HorSpike.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void SpawnCrystal()
    {
        GameObject newCrystal = Instantiate(Crystal, WheretoSpawnCrystal, Quaternion.identity);
        newCrystal.GetComponent<SlimeBossSpike>().damage = CrystalDamage;
    }

    void SpawnHighHorizontalSpike()
    {
        GameObject HorSpike = Instantiate(HorizontalSpike, transform.position + WheretoSpawnHighHorizontal, Quaternion.identity);
        HorSpike.GetComponent<SlimeBossSpike>().damage = HorizontalSpikeDamage;
        HorSpike.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void SpawnCloseRangeSpikes()
    {
        for (int i = 0;i<5;i++)
        {
            GameObject SpikeLeft = Instantiate(HorizontalSpike, transform.position + WheretoSpawnCloseRangeSpikes + new Vector3(0,0.2f*i,0), Quaternion.identity);
            SpikeLeft.transform.localScale = new Vector3(0.2f, 0.5f, 1);
            SpikeLeft.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            SpikeLeft.transform.rotation = Quaternion.Euler(0, 0, 90);

            Vector3 wheretospawnright = new Vector3(-WheretoSpawnCloseRangeSpikes.x, WheretoSpawnCloseRangeSpikes.y, WheretoSpawnCloseRangeSpikes.z);
            GameObject SpikeRight = Instantiate(HorizontalSpike, transform.position + wheretospawnright + new Vector3(0, 0.2f * i, 0), Quaternion.identity);
            SpikeRight.GetComponent<SlimeBossSpike>().damage = verticalspikedamage;
            SpikeRight.transform.rotation = Quaternion.Euler(0, 0, 90);
            SpikeRight.transform.localScale = new Vector3(0.2f, -0.5f, 1);
        }
    }

}
