using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{


    [System.Serializable]
    public class DialoguePart
    {
        public Sprite Face;
        public string name;
        public string text;
        public List<DialogueMovement> movementtotrigger;
    }

    [System.Serializable]
    public class DialogueMovement
    {
        public GameObject Actor;
        public Vector2 wheretogo;
        public float speed;
    }

    [System.Serializable]
    public class Dialogue
    {
        public int worldflagindex;
        public List<DialoguePart> parts;
        public int worldflagtotriggerattheend;
        public Vector2 startpos;
    }

    public List<Dialogue> DialogueList; 

    public GameObject DialogueWindowPrefab;

    private Global Global;

    private bool indialogue;
    public int pageindex;

    private GameObject currentwindow;
    private Dialogue currentdialogue;

    private bool launchfadetoblack;
    private Image fadetoblack;

    public float fadeduration;

    private int fadecounter;

    // Start is called before the first frame update
    void Start()
    {
        Global = FindAnyObjectByType<Global>();
        fadetoblack = GameObject.Find("FadeToBlackImageForcutscene").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (launchfadetoblack)
        {
            if (fadetoblack.color.a < 1)
            {
                fadecounter += 1;
                if (fadecounter / (fadeduration / Time.deltaTime) > 1f)
                {
                    fadetoblack.color = new Color(1f, 1f, 1f, 1f);
                    launchfadetoblack = false;

                }
                else
                {
                    fadetoblack.color = new Color(1f, 1f, 1f, fadecounter / (fadeduration / Time.deltaTime));
                }
            }
            if (fadetoblack.color.a == 1)
            {
                launchdialogue();
                fadecounter = -(int)(fadeduration / Time.deltaTime);
                fadetoblack.color = new Color(1f, 1f, 1f, ((fadeduration / Time.deltaTime) - fadecounter) / (fadeduration / Time.deltaTime));
            }
        }
        else
        {
            if (fadetoblack.color.a > 0)
            {
                fadecounter += 1;
                fadetoblack.color = new Color(1f, 1f, 1f, ((fadeduration / Time.deltaTime) - fadecounter) / (fadeduration / Time.deltaTime));
            }
            else
            {
                fadecounter = 0;
                fadetoblack.color = new Color(1f, 1f, 1f, 0);
            }

        }

        if (currentdialogue != null)
        {
            if(currentdialogue.parts[pageindex]!=null)
            {
                foreach (DialogueMovement Movement in currentdialogue.parts[pageindex].movementtotrigger)
                {
                    if (Movement.Actor != null)
                    {
                        Vector2 pos = Movement.Actor.transform.position;
                        Vector2 dest = Movement.wheretogo;
                        if (Vector2.Distance(pos, dest) >= 0.05)
                        {
                            float speed = Movement.speed;
                            Movement.Actor.GetComponent<Rigidbody2D>().velocity = (dest - pos).normalized * speed;
                        }
                        else
                        {
                            Movement.Actor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        }
                    }
                }
            }
            
        }
    }

    public void flagactivated(int flagindex)
    {
        foreach (Dialogue dialogue in DialogueList)
        {
            if (dialogue.worldflagindex ==flagindex)
            {

                launchfade(dialogue);
                break;
            }
        }
    }

    void launchfade (Dialogue dialogue)
    {
        currentdialogue = dialogue;
        launchfadetoblack = true;
    }

    void launchdialogue()
    {
        FindAnyObjectByType<PlayerHP>().GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        Global.indialogue = true;
        pageindex = -1;
        OpenNextPage();
    }

    public void OpenNextPage()
    {
        if(currentdialogue != null)
        {
            Destroy(currentwindow);
        }
        if (currentdialogue.parts.Count>pageindex+1)
        {
            pageindex += 1;
            if(pageindex==0 && currentdialogue.startpos!=Vector2.zero)
            {
                FindAnyObjectByType<PlayerMovement>().transform.position = currentdialogue.startpos;
            }
            if(pageindex>0)
            {
                foreach (DialogueMovement Movement in currentdialogue.parts[pageindex - 1].movementtotrigger)
                {
                    if (Movement.Actor != null)
                    {
                        Movement.Actor.transform.position = Movement.wheretogo;
                    }
                }
            }
            
            currentwindow = Instantiate(DialogueWindowPrefab, GameObject.Find("Canvas").transform);
            currentwindow.GetComponent<CutsceneDialogueWindow>().InitiateDialogue(currentdialogue.parts[pageindex]);
        }
        else
        {
            Global.indialogue = false;
            if(currentdialogue.worldflagtotriggerattheend!=0)
            {
                Global.worldflags[currentdialogue.worldflagtotriggerattheend] = true;
            }
            currentdialogue = null;
        }
        
    }

    public void AccelerateOrClose()
    {
        if(Global.indialogue)
        {
            if (currentwindow.GetComponent<CutsceneDialogueWindow>().numberofcharacterstoshow < currentdialogue.parts[pageindex].text.Length)
            {
                currentwindow.GetComponent<CutsceneDialogueWindow>().numberofcharacterstoshow = currentdialogue.parts[pageindex].text.Length;
            }
            else
            {
                OpenNextPage();
            }
        }
    }

    public void TrytoTrigger(int worldflagindex)
    {
        foreach(Dialogue dialogue in DialogueList)
        {
            if(dialogue.worldflagindex==worldflagindex)
            {
                launchfade(dialogue);
                return;
            }
        }
    }
}
