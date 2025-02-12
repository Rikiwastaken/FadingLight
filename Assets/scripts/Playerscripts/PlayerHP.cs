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
    [SerializeField] private LayerMask whatisspike;
    [SerializeField] private Transform groundcheck;
    private bool inv = false;
    private int iframe;
    public int invicibilityframes;
    public float hitjumpforce;
    private Rigidbody2D rb;


    public float radOcircle;
    private Healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GameObject.Find("PlayerLifeBars").GetComponent<Healthbar>();
        Eldonhp = Playervalues.EldonHP;
        Eldonmaxhp = Playervalues.EldonmaxHP;
        EldonNRG = 0;
        EldonmaxNRG = Playervalues.EldonmaxNRG;
        healthbar.SetMaxhealth(Eldonmaxhp);
        healthbar.SetMaxEnergy(EldonmaxNRG);
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag =="enemy" & !inv)
        {
            Eldonhp -= other.gameObject.GetComponent<EnemyHP>().enemydamage;
            inv = true;
            iframe = invicibilityframes;
            rb.velocity = new Vector2(rb.velocity.x, hitjumpforce);
        }
    }
    void FixedUpdate()
    {

        


        if (Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisspike) & inv == false)
        {
            Eldonhp = Eldonhp - 5;
            inv = true;
            iframe = invicibilityframes;
            rb.velocity=new Vector2(rb.velocity.x, hitjumpforce);
        }

        healthbar.SetHealth(Eldonhp);
        healthbar.SetEnergy(EldonNRG);

        if (Eldonhp <= 0)
        {
            SceneManager.LoadScene("MainMenu");
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
            SceneManager.LoadScene("MainMenu");
        }
    }
}