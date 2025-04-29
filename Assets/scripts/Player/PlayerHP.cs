using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerHP : MonoBehaviour
{
    public float Eldonhp;
    public float Eldonmaxhp;
    public float EldonNRG;
    public float EldonmaxNRG;
    [SerializeField] private Transform groundcheck;
    private bool inv = false;
    public int iframe;
    public float timeinvincible;
    public float hitjumpforce;
    private Rigidbody2D rb;
    public float damagereduction;

    public GameObject GameOverPrefab;


    public float radOcircle;
    private Healthbar healthbar;
    private EquipmentScript equipmentScript;

    private Global global;

    public int MetalScrap; //number of Metal Scraps;
    public int CorePieces; // number of Core Pieces
    public int ElectronicComponents; // number of Electronic Components

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        rb = GetComponent<Rigidbody2D>();
        equipmentScript = GetComponent<EquipmentScript>();
        
    }


    private float calculatedefense()
    {
        float defense = damagereduction;
        if (equipmentScript.equipedPlateIndex != -1)
        {
            defense += equipmentScript.Platelist[equipmentScript.equipedPlateIndex].Defense;
        }
        if (GetComponent<ChainSawMode>().chainsawmode)
        {
            defense *= GetComponent<ChainSawMode>().chainsawdefenseMultiplier;
        }
        if (GetComponent<AugmentsScript>().EquipedAugments[13] && EldonNRG>=EldonmaxNRG*0.8f)
        {
            defense *= GetComponent<AugmentsScript>().Augmentlist[13].valueincr;
        }
        return defense;
    }
    public void TakeDamage(float damage, Vector2 velchg, Vector2 ForceApplied)
    {
        if (!inv)
        {
            int damagetotake = 1;
            float totaldamagereduction = calculatedefense();

            if (damage - totaldamagereduction > 0f)
            {
                damagetotake = (int)(damage - totaldamagereduction);
            }
            Eldonhp -= damagetotake;
            inv = true;
            iframe = (int)(timeinvincible/Time.fixedDeltaTime);
            if (velchg.y != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, velchg.y);
            }
            if (velchg.x != 0)
            {
                rb.velocity = new Vector2(velchg.x, rb.velocity.y);
            }
            if (ForceApplied != Vector2.zero)
            {
                rb.AddForce(ForceApplied / Time.deltaTime);
            }
        }
    }

    public void TakeDamage(float damage, Vector2 velchg, Vector2 ForceApplied, float energydamage)
    {
        if (!inv)
        {
            int damagetotake = 1;
            int energydamagetotake = 1;
            float totaldamagereduction = calculatedefense();

            if (damage - totaldamagereduction > 0f)
            {
                damagetotake = (int)(damage - totaldamagereduction);
            }
            if(energydamage - totaldamagereduction > 0f)
            {
                energydamagetotake = (int)(energydamage - totaldamagereduction);
            }
            EldonNRG -= damagetotake;
            if(EldonNRG < 0)
            {
                EldonNRG = 0;
            }
            if (GetComponent<AugmentsScript>().EquipedAugments[17])
            {
                if(EldonNRG/2<=damagetotake)
                {
                    damagetotake -= (int)(EldonNRG/2);
                    EldonNRG = 0;
                }
                else
                {
                    damagetotake = 0;
                    EldonNRG -=damagetotake*2;
                }
            }
            Eldonhp -= damagetotake;
            inv = true;
            iframe = (int)(timeinvincible/Time.fixedDeltaTime);
            if (velchg.y != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, velchg.y);
            }
            if (velchg.x != 0)
            {
                rb.velocity = new Vector2(velchg.x, rb.velocity.y);
            }
            if (ForceApplied != Vector2.zero)
            {
                rb.AddForce(ForceApplied, ForceMode2D.Impulse);
            }
        }
    }

    public void TakeDamage(float damage, float energydamage)
    {
        if (!inv)
        {
            int damagetotake = 1;
            int energydamagetotake = 1;
            float totaldamagereduction = calculatedefense();

            if (damage - totaldamagereduction > 0f)
            {
                damagetotake = (int)(damage - totaldamagereduction);
            }
            if (energydamage - totaldamagereduction > 0f)
            {
                energydamagetotake = (int)(energydamage - totaldamagereduction);
            }
            EldonNRG -= damagetotake;
            if (EldonNRG < 0)
            {
                EldonNRG = 0;
            }
            if (GetComponent<AugmentsScript>().EquipedAugments[17])
            {
                if (EldonNRG / 2 <= damagetotake)
                {
                    damagetotake -= (int)(EldonNRG / 2);
                    EldonNRG = 0;
                }
                else
                {
                    damagetotake = 0;
                    EldonNRG -= damagetotake * 2;
                }
            }
            Eldonhp -= damagetotake;
            inv = true;
            iframe = (int)(timeinvincible/Time.fixedDeltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!inv)
        {
            int damagetotake = 1;
            float totaldamagereduction = calculatedefense();

            if (damage - totaldamagereduction > 0f)
            {
                damagetotake = (int)(damage - totaldamagereduction);
            }
            EldonNRG -= damagetotake;
            if (EldonNRG < 0)
            {
                EldonNRG = 0;
            }
            if (GetComponent<AugmentsScript>().EquipedAugments[17])
            {
                if (EldonNRG / 2 <= damagetotake)
                {
                    damagetotake -= (int)(EldonNRG / 2);
                    EldonNRG = 0;
                }
                else
                {
                    damagetotake = 0;
                    EldonNRG -= damagetotake * 2;
                }
            }
            Eldonhp -= damagetotake;
            inv = true;
            iframe = (int)(timeinvincible/Time.fixedDeltaTime);
        }
    }

    void FixedUpdate()
    {

        if(MetalScrap>99999)
        {
            MetalScrap = 99999;
        }
        if (ElectronicComponents > 99999)
        {
            ElectronicComponents = 99999;
        }
        if (CorePieces > 99999)
        {
            CorePieces = 99999;
        }

        if (global==null)
        {
            global = FindAnyObjectByType<Global>();
        }
        

        healthbar.SetHealth(Eldonhp);
        healthbar.SetEnergy(EldonNRG);

        if (Eldonhp <= 0)
        {
            global.inbossfight = false;
            Instantiate(GameOverPrefab, GameObject.Find("Canvas").transform);
        }


        if (Eldonhp >Eldonmaxhp)
        {
            Eldonhp = Eldonmaxhp;
        }
        if (inv == true)
        {
            iframe = iframe - 1;
            if (iframe == 0)
            {
                inv = false;
                Color col = GetComponent<SpriteRenderer>().color;
                col.a = 1f;
                GetComponent<SpriteRenderer>().color = col;
            }
            else if(iframe>0)
            {
                Color col = GetComponent<SpriteRenderer>().color;
                col.a = 0.8f;
                GetComponent<SpriteRenderer>().color = col;
            }
        }


        if(global.indialogue || global.atsavepoint)
        {
            inv = true;
            iframe = 2;
            GetComponent<PlayerMovement>().rolling = false;
        }
        

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll") && iframe == 0 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime<1 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1/14)
        {
            iframe += 1;
            inv = true;
            
        }
        if((GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll") && GetComponent<PlayerJumpV3>().grounded) || GetComponent<PlayerDodge>().airdodgelengthcnt>0 )
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("playerpassthroughenemy");
            gameObject.layer = LayerIgnoreRaycast;
            GetComponent<PlayerMovement>().rolling = true;
        }
        else
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("player");
            gameObject.layer = LayerIgnoreRaycast;
            GetComponent<PlayerMovement>().rolling = false;
        }

        if (GetComponent<Rigidbody2D>().position.y<-50)
        {
            Eldonhp -=0.05f* Eldonmaxhp;
        }
    }


    public void UpdateBars()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        healthbar.SetMaxhealth(Eldonmaxhp);
        healthbar.SetMaxEnergy(EldonmaxNRG);
    }
}