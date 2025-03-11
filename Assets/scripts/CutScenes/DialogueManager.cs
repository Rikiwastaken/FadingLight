using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public List<Dialogue> DialogueList; 

    public GameObject DialogueWindowPrefab;

    private Global Global;

    private bool indialogue;
    public int pageindex;

    private GameObject currentwindow;
    private Dialogue currentdialogue;

    // Start is called before the first frame update
    void Start()
    {
        Global = FindAnyObjectByType<Global>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentdialogue != null)
        {
            foreach(DialogueMovement Movement in currentdialogue.parts[pageindex].movementtotrigger)
            {
                if (Movement.Actor != null)
                {
                    Vector2 pos = Movement.Actor.transform.position;
                    Vector2 dest = Movement.wheretogo;
                    if(Vector2.Distance(pos, dest) >= 0.05)
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

    public void flagactivated(int flagindex)
    {
        foreach (Dialogue part in DialogueList)
        {
            if (part.worldflagindex ==flagindex)
            {

                launchdialogue(part);
                break;
            }
        }
    }

    void launchdialogue(Dialogue dialogue)
    {
        FindAnyObjectByType<PlayerHP>().GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        Global.indialogue = true;
        pageindex = -1;
        currentdialogue = dialogue;
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
                launchdialogue(dialogue);
                return;
            }
        }
    }
}
