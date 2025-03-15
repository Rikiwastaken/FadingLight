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
    public int numberofseparators;
    public GameObject SeparatorGO;
    public bool setupseparatorsbool;
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
        if (setupseparatorsbool)
        {
            SetupSeparators();
        }
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

    void SetupSeparators()
    {
        float minposx=transform.GetChild(0).GetComponent<RectTransform>().position.x- transform.GetChild(0).GetComponent<RectTransform>().rect.width/2;
        float maxposx = transform.GetChild(0).GetComponent<RectTransform>().position.x + transform.GetChild(0).GetComponent<RectTransform>().rect.width / 2;
        if (numberofseparators <=0)
        {
            SeparatorGO.SetActive(false);
        }
        else if(numberofseparators >1)
        {
            List<GameObject> separators = new List<GameObject>();
            for(int i = 1;i <= numberofseparators; i++)
            {
                GameObject newSeparator = Instantiate(SeparatorGO,SeparatorGO.transform.position,Quaternion.identity);
                float newx =minposx+(maxposx-minposx)*i/(numberofseparators+1);
                newSeparator.transform.position = new Vector2(newx, SeparatorGO.transform.position.y);
                newSeparator.transform.localScale = new Vector3(0.25f, 1f,1f);
                separators.Add(newSeparator);
            }
            foreach(GameObject separator in separators)
            {
                separator.transform.SetParent(SeparatorGO.transform,true);
            }
            SeparatorGO.GetComponent<Image>().enabled = false; ;
        }
        setupseparatorsbool = false;
    }

    public void EndCombat()
    {
        foreach (Transform child in SeparatorGO.transform)
        {
            Destroy(child.gameObject);
        }
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
