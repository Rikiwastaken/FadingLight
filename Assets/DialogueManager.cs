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
        public DialogueMovement movementtotrigger;
    }

    [System.Serializable]
    public class DialogueMovement
    {
        public GameObject Actor;
        public Vector2 wheretogo;
        public float inwhattime;
    }

    [System.Serializable]
    public class Dialogue
    {
        public int worldflagindex;
        public List<DialoguePart> parts;
    }

    public List<bool> flagstomonitor = new List<bool>();

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
        foreach (bool flag in Global.worldflags)
        {
            flagstomonitor.Add(flag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Dialogue part in DialogueList)
        {
            if (flagstomonitor[part.worldflagindex] != Global.worldflags[part.worldflagindex])
            {

                launchdialogue(part);
                flagstomonitor = Global.worldflags;
                break;
            }
        }
        
    }

    void launchdialogue(Dialogue dialogue)
    {
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
            currentwindow = Instantiate(DialogueWindowPrefab, GameObject.Find("Canvas").transform);
            currentwindow.GetComponent<CutsceneDialogueWindow>().InitiateDialogue(currentdialogue.parts[pageindex]);
        }
        else
        {
            Global.indialogue = false;
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
}
