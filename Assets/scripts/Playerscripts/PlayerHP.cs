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
    private int iframe;
    public int invicibilityframes;
    public float hitjumpforce;
    private Rigidbody2D rb;
    public float damagereduction;


    public float radOcircle;
    private Healthbar healthbar;
    private EquipmentScript equipmentScript;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        rb = GetComponent<Rigidbody2D>();
        equipmentScript = GetComponent<EquipmentScript>();
        
    }

    // Update is called once per frame

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag =="enemy" & !inv)
        {
            TakeDamage(other.gameObject.GetComponent<EnemyHP>().enemydamage, new Vector2(rb.velocity.x, hitjumpforce),Vector2.zero);
        }
    }

    public void TakeDamage(float damage, Vector2 velchg, Vector2 ForceApplied)
    {
        int damagetotake = 1;
        float totaldamagereduction = damagereduction;
        if (equipmentScript.equipedPlateIndex!=-1)
        {
            totaldamagereduction += equipmentScript.Platelist[equipmentScript.equipedPlateIndex].Defense;
        }

        if (damage- totaldamagereduction > 0f)
        {
            damagetotake = (int)(damage - totaldamagereduction);
        }
        Eldonhp -= damagetotake;
        inv = true;
        iframe = invicibilityframes;
        if(velchg!=Vector2.zero)
        {
            rb.velocity = new Vector2(rb.velocity.x, hitjumpforce);
        }
        if(ForceApplied!=Vector2.zero)
        {
            rb.AddForce(ForceApplied/Time.deltaTime);
        }
        
    }
    void FixedUpdate()
    {

        healthbar.SetHealth(Eldonhp);
        healthbar.SetEnergy(EldonNRG);

        if (Eldonhp <= 0)
        {
            SceneManager.LoadScene("BreedingGrounds");
        }
        if (inv == true)
        {
            iframe = iframe - 1;
            if (iframe == 0)
            {
                inv = false;
            }
        }

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll") && iframe == 0 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime<1 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1/14)
        {
            iframe += 1;
            inv = true;
            
        }
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("roll"))
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

        if (GetComponent<Rigidbody2D>().position.y<-3)
        {
            SceneManager.LoadScene("BreedingGrounds");
        }
    }


    public void UpdateBars()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        healthbar.SetMaxhealth(Eldonmaxhp);
        healthbar.SetMaxEnergy(EldonmaxNRG);
    }
}