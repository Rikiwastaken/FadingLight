using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeBar : MonoBehaviour
{
    private Global global;
    private int MaxHP;
    private int MaxNRG;
    public Image HPimage;
    public Image NRGImage;
    private EnemyHP bossenemyHP;
    void Start()
    {
        global = FindAnyObjectByType<Global>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (global == null)
        {
            global = FindAnyObjectByType<Global>();
        }
        if(global.inbossfight)
        {
            HPimage.fillAmount = bossenemyHP.enemyhp*1f / MaxHP;
            NRGImage.fillAmount = bossenemyHP.enemyNRG*1f / MaxNRG;
        }
    }


    public void EndCombat()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        bossenemyHP = null;
    }

    public void InitiateCombat(EnemyHP EnemyHP)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        bossenemyHP = EnemyHP;
        MaxHP = bossenemyHP.enemymaxhp;
        MaxNRG = bossenemyHP.enemymaxNRG;
    }

}
