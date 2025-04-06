using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeBoss : MonoBehaviour
{

    public GameObject HorizontalSpike;
    public GameObject VerticalSpike;
    public GameObject Crystal;
    public GameObject AttackPreviewPrefab;
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
    public bool phasetransition;

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
        if(GetComponent<EnemyHP>().enemyhp<= GetComponent<EnemyHP>().enemymaxhp/2 && !phasetransition)
        {
            phasetransition = true;
            attackcooldown = attackcooldown * 0.6666f;
        }
    }



    void RandomAttackClose()
    {
        int rd = Random.Range(0, 120);
        if (rd <= 10)
        {
            SpawnLineOfSpikes();
        }
        else if (rd <= 20)
        {
            SpawnLineOfSpikesMoved();
        }
        else if (rd <= 30)
        {
            SpawnLineOfSpikesMovedAgain();
        }
        else if (rd <= 40)
        {
            SpawnLowHorizontalSpike();
        }
        else if (rd <= 50)
        {
            SpawnHighHorizontalSpike();
        }
        else if (rd <= 90)
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
            Vector3 position = new Vector3(transform.position.x - 10f / 3f - i*5f, yforvertical, -1);
            GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));

            position = new Vector3(transform.position.x + 10f / 3f + i * 5f, yforvertical, -1);
            AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));
        }
    }

    void SpawnLineOfSpikesMoved()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 position = new Vector3(transform.position.x - 10f / 3f - i * 5f - 10f / 3f, yforvertical, -1);
            GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));

            position = new Vector3(transform.position.x + 10f / 3f + i * 5f + 5f / 3f, yforvertical, -1);
            AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));

        }
    }

    void SpawnLineOfSpikesMovedAgain()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = new Vector3(transform.position.x - 10f / 3f - i * 5f * 0.75f, yforvertical, -1);
            GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));

            position = new Vector3(transform.position.x + 10f / 3f + i * 5f * 0.75f, yforvertical, -1);
            AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(VerticalSpike, position, verticalspikedamage, 0.5f, new Vector2(0, -2.5f));
        }
    }

    void SpawnLowHorizontalSpike()
    {
        Vector3 position = transform.position + WheretoSpawnLowHorizontal;
        GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
        AttackPreview.GetComponent<AttackZone>().InstantiateObject(HorizontalSpike, position, HorizontalSpikeDamage, 0.5f,Vector3.zero,new Vector3(0,0,90), new Vector2(7.5f,0));
    }

    void SpawnCrystal()
    {

        Vector3 position = WheretoSpawnCrystal;
        GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
        AttackPreview.GetComponent<AttackZone>().InstantiateObject(Crystal, position, CrystalDamage, 1f, new Vector2(0, -7.5f));
    }

    void SpawnHighHorizontalSpike()
    {
        Vector3 position = transform.position + WheretoSpawnHighHorizontal;
        GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
        AttackPreview.GetComponent<AttackZone>().InstantiateObject(HorizontalSpike, position, HorizontalSpikeDamage, 0.5f, Vector3.zero, new Vector3(0, 0, 90), new Vector2(7.5f, 0));
    }

    void SpawnCloseRangeSpikes()
    {
        for (int i = 0;i<5;i++)
        {
            Vector3 position = transform.position + WheretoSpawnCloseRangeSpikes + new Vector3(0, i, 0);
            GameObject AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(HorizontalSpike, position, verticalspikedamage, 0.5f, new Vector3(1f, 2.5f, 1), new Vector3(0, 0, 90), new Vector2(1.25f, 0));


            Vector3 wheretospawnright = new Vector3(-WheretoSpawnCloseRangeSpikes.x, WheretoSpawnCloseRangeSpikes.y, WheretoSpawnCloseRangeSpikes.z);
            position = transform.position + wheretospawnright + new Vector3(0, i, 0);
            AttackPreview = Instantiate(AttackPreviewPrefab, position, Quaternion.identity);
            AttackPreview.GetComponent<AttackZone>().InstantiateObject(HorizontalSpike, position, verticalspikedamage, 0.5f, new Vector3(1f, -2.5f, 1), new Vector3(0, 0, 90), new Vector2(-1.25f, 0));
        }
    }

}
