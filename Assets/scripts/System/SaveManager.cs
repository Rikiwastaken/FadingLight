using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AugmentsScript;
using static EquipmentScript;
using static SupportDrone;
using static GadgetScript;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Save
    {
        public int slotID; //slot of the save
        public int elapsedseconds; //number of seconds elapsed since creation of the save
        public string zoneName; //Name of the zone where last saved
        public Vector2 coordinates; //Coordinates of the player when last saved
        public List<bool> unlockedAugments; //list of unlocked Augments (true if unlocked, false else)
        public List<bool> unlockedChains; //list of unlocked Chains (true if unlocked, false else)
        public List<bool> unlockedPlates; //list of unlocked Plates (true if unlocked, false else)
        public List<bool> unlockedDrones; //list of unlocked Drones (true if unlocked, false else)
        public List<bool> unlockedGadgets; //list of unlocked Gadgets (true if unlocked, false else)
        public List<int> loadout; //list of the IDs of equiped Items, 0 is chain, 1 is plate, 2 and 3 are drones, 4 is gadgets and the rest are augments;
        public List<bool> WorldFlags; //flags for progession
        public int Shards; //Number of Crystal Shards picked up
    }

    public Save save;
    public int CurrentSlot;
    private int FrameCounter;
    public bool playing;

    public bool deleteallsaves;

    private void FixedUpdate()
    {
        if(playing && save!=null)
        {
            FrameCounter++;
            if(FrameCounter*Time.deltaTime>1)
            {
                FrameCounter = 0;
                save.elapsedseconds += 1;
            }
        }

        if(deleteallsaves)
        {
            deleteallsaves = false;
            DeleteAllSlots();
        }
        
    }

    public void InitializeSaveObject(int slotID,int elapsedseconds)
    {
        InitializeSaveObject();
        save.elapsedseconds = elapsedseconds;
        save.slotID = slotID;
    }

    public void InitializeSaveObject(int slotID)
    {
        InitializeSaveObject();
        save.slotID = slotID;
    }

    public void CreateEmptySaveFile(int slotID)
    {
        EquipmentScript equipmentScript = FindAnyObjectByType<EquipmentScript>();
        AugmentsScript augmentsScript = FindAnyObjectByType<AugmentsScript>();
        save = new Save();
        save.elapsedseconds = 0;
        save.slotID = CurrentSlot;
        save.zoneName = SceneManager.GetActiveScene().name;
        save.coordinates = FindAnyObjectByType<PlayerMovement>().transform.position;
        List<Augment> augmentlist = augmentsScript.Augmentlist;
        List<bool> unlockedaugments = new List<bool>();
        foreach (Augment augment in augmentlist)
        {
            unlockedaugments.Add(false);
        }
        save.unlockedAugments = unlockedaugments;

        List<Chain> Chainlist = equipmentScript.Chainslist;
        List<bool> unlockedchains = new List<bool>(Chainlist.Count);
        foreach (Chain chain in Chainlist)
        {
            unlockedchains.Add(false);
        }
        unlockedchains[0]=true;
        save.unlockedChains = unlockedchains;

        List<Plate> Platelist = equipmentScript.Platelist;
        List<bool> unlockedplates = new List<bool>(Platelist.Count);
        foreach (Plate plate in Platelist)
        {
            unlockedplates.Add(false);
        }
        unlockedplates[0] = true;
        save.unlockedPlates = unlockedplates;

        List<HealerDrone> Dronelist = FindAnyObjectByType<SupportDrone>().drones;
        List<bool> unlockeddrones = new List<bool>(Dronelist.Count);
        foreach (HealerDrone drone in Dronelist)
        {
            unlockeddrones.Add(false);
        }
        save.unlockedDrones = unlockeddrones;

        List<Gadget> GadgetList = FindAnyObjectByType<GadgetScript>().GadgetList;
        List<bool> unlockedgadgets = new List<bool>(GadgetList.Count);
        foreach (Gadget gadget in GadgetList)
        {
            unlockedgadgets.Add(false);
        }
        unlockedgadgets[0] = true;
        save.unlockedGadgets = unlockedgadgets;

        List<int> equipedItemsID = new List<int>();
        equipedItemsID.Add(0);
        equipedItemsID.Add(0);
        equipedItemsID.Add(-1);
        equipedItemsID.Add(-1);
        equipedItemsID.Add(0);
        save.loadout = equipedItemsID;

        save.WorldFlags = new List<bool>();
        for(int i=0; i<FindAnyObjectByType<Global>().worldflags.Count;i++)
        {
            save.WorldFlags.Add(false);
        }
        save.Shards = 0;


    }

    public void InitializeSaveObject()
    {
        EquipmentScript equipmentScript = FindAnyObjectByType<EquipmentScript>();
        GadgetScript gadgetscript = FindAnyObjectByType<GadgetScript>();
        AugmentsScript augmentsScript = FindAnyObjectByType<AugmentsScript>();
        save = new Save();
        save.elapsedseconds = 0;
        save.slotID = CurrentSlot;
        save.zoneName = SceneManager.GetActiveScene().name;
        save.coordinates = FindAnyObjectByType<PlayerMovement>().transform.position;
        List<Augment> augmentlist = augmentsScript.Augmentlist;
        List<bool> unlockedaugments = new List<bool>();
        foreach(Augment augment in augmentlist)
        {
            unlockedaugments.Add(!augment.locked);
        }
        save.unlockedAugments = unlockedaugments;

        List<Chain> Chainlist = equipmentScript.Chainslist;
        List<bool> unlockedchains = new List<bool>(Chainlist.Count);
        foreach (Chain chain in Chainlist)
        {
            unlockedchains.Add(!chain.locked);
        }
        save.unlockedChains = unlockedchains;

        List<Plate> Platelist = equipmentScript.Platelist;
        List<bool> unlockedplates = new List<bool>(Platelist.Count);
        foreach(Plate plate in Platelist)
        {
            unlockedplates.Add(!plate.locked);
        }
        save.unlockedPlates = unlockedplates;

        List<HealerDrone> Dronelist = FindAnyObjectByType<SupportDrone>().drones;
        List<bool> unlockeddrones = new List<bool>(Dronelist.Count);
        foreach(HealerDrone drone in Dronelist)
        {
            unlockeddrones.Add(!drone.locked);
        }
        save.unlockedDrones = unlockeddrones;

        List<Gadget> GadgetList = gadgetscript.GadgetList;
        List<bool> unlockedgadgets = new List<bool>(GadgetList.Count);
        foreach (Gadget gadget in GadgetList)
        {
            unlockedgadgets.Add(!gadget.locked);
        }
        save.unlockedGadgets = unlockedgadgets;

        List<int> equipedItemsID = new List<int>();
        equipedItemsID.Add(equipmentScript.equipedChainIndex);
        equipedItemsID.Add(equipmentScript.equipedPlateIndex);
        equipedItemsID.Add(equipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID);
        equipedItemsID.Add(equipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID);
        equipedItemsID.Add(gadgetscript.ActiveGadgetID);
        List<bool> equipedAugments = FindAnyObjectByType<AugmentsScript>().EquipedAugments;
        for (int i = 0; i < equipedAugments.Count; i++)
        {
            if (equipedAugments[i])
            {
                equipedItemsID.Add(i);
            }
        }
        save.loadout = equipedItemsID;

        save.WorldFlags = new List<bool>();
        foreach(bool flag in FindAnyObjectByType<Global>().worldflags)
        {
            save.WorldFlags.Add(flag);
        }
        save.Shards = augmentsScript.numberofShardsPickedUp;

    }

    public void LoadSave()
    {
        string savetoload = GetLastSave(CurrentSlot)[0];
        if (savetoload != null)
        {
            save = JsonUtility.FromJson<Save>(savetoload);
            SceneManager.LoadScene(save.zoneName);
        }
        else
        {
            SceneManager.LoadScene("BreedingGrounds");
        }
    }


    public void ApplySave()
    {
        EquipmentScript equipmentScript = FindAnyObjectByType<EquipmentScript>();
        AugmentsScript augmentsScript = FindAnyObjectByType<AugmentsScript>();
        GadgetScript gadgetScript = FindAnyObjectByType<GadgetScript>();

        //    public int slotID; //slot of the save
        //public int elapsedseconds; //number of seconds elapsed since creation of the save
        //public int zoneID; //ID of the zone where last saved
        //public Vector2 coordinates; //Coordinates of the player when last saved
        //public List<bool> unlockedAugments; //list of unlocked Augments (true if unlocked, false else)
        //public List<bool> unlockedChains; //list of unlocked Chains (true if unlocked, false else)
        //public List<bool> unlockedPlates; //list of unlocked Plates (true if unlocked, false else)
        //public List<bool> unlockedDrones; //list of unlocked Drones (true if unlocked, false else)
        //list of unlocked Gadgets (true if unlocked, false else)
        //public List<int> loadout; //list of the IDs of equiped Items, 0 is chain, 1 is plate, 2 and 3 are drones, 4 is gadget and the rest are augments;
        //public List<bool> WorldFlags; //flags for progession
        //public int Shards; //Number of Crystal Shards picked up


        equipmentScript.transform.position = save.coordinates;
        for(int i = 0;i<augmentsScript.Augmentlist.Count;i++)
        {
            augmentsScript.Augmentlist[i].locked = !save.unlockedAugments[i];
        }

        for (int i = 0; i < equipmentScript.Chainslist.Count; i++)
        {
            equipmentScript.Chainslist[i].locked = !save.unlockedChains[i];
        }

        for (int i = 0; i < equipmentScript.Platelist.Count; i++)
        {
            equipmentScript.Platelist[i].locked = !save.unlockedPlates[i];
        }

        for (int i = 0; i < equipmentScript.drone1.GetComponent<SupportDrone>().drones.Count; i++)
        {
            equipmentScript.drone1.GetComponent<SupportDrone>().drones[i].locked = !save.unlockedDrones[i];
            equipmentScript.drone2.GetComponent<SupportDrone>().drones[i].locked = !save.unlockedDrones[i];
        }

        equipmentScript.equipedChainIndex = save.loadout[0];
        equipmentScript.equipedPlateIndex = save.loadout[1];
        equipmentScript.drone1.GetComponent<SupportDrone>().ActiveDroneID = save.loadout[2];
        equipmentScript.drone2.GetComponent<SupportDrone>().ActiveDroneID = save.loadout[3];
        gadgetScript.ActiveGadgetID = save.loadout[4];
        for(int i = 5;i<save.loadout.Count;i++)
        {
            augmentsScript.EquipedAugments[save.loadout[i]] = true;
        }
        augmentsScript.manuallyapplyaugmentboosts = true;

        for (int i = 0; i < gadgetScript.GadgetList.Count; i++)
        {
            gadgetScript.GadgetList[i].locked = !save.unlockedGadgets[i];
        }

        FindAnyObjectByType<Global>().worldflags=save.WorldFlags;

        augmentsScript.numberofShardsPickedUp = save.Shards;
    }

    public List<string> GetLastSave(int slotID)
    {
        string res = null;
        string nameres = null;
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles/"))
        {
            string[] listpaths = System.IO.Directory.GetFiles(Application.persistentDataPath + "/SaveFiles/");
            List<string> listnames = new List<string>();
            foreach (string path in listpaths)
            {
                string newname = path;
                string abstractpath = Application.persistentDataPath + "/SaveFiles/";
                newname = newname.Substring((int)(abstractpath.Length), newname.Length - abstractpath.Length);
                if (int.Parse("" + newname[0]) == slotID)
                    listnames.Add(newname);
            }
            if (listnames.Count > 0)
            {
                string lastname = listnames[0];
                foreach (string name in listnames)
                {
                    if (int.Parse(name.Split('_')[1]) > int.Parse(lastname.Split('_')[1]))
                    {
                        lastname = name;
                    }
                }
                res = System.IO.File.ReadAllText(Application.persistentDataPath + "/SaveFiles/" + lastname);
                nameres = lastname;
            }
        }
        List<string> finalres= new List<string>() { res,nameres};
        return finalres;
    }

    void Delete6thSave(int slotID)
    {
        if(System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles/"))
        {
            string[] listpaths = System.IO.Directory.GetFiles(Application.persistentDataPath + "/SaveFiles/");
            List<string> listnames = new List<string>();
            foreach (string path in listpaths)
            {
                string newname = path;
                string abstractpath = Application.persistentDataPath + "/SaveFiles/";
                newname = newname.Substring((int)(abstractpath.Length), newname.Length - abstractpath.Length);
                if (int.Parse("" + newname[0])==save.slotID)
                listnames.Add(newname);
            }
            if (listnames.Count>5)
            {
                string lastname = listnames[0];
                foreach (string name in listnames)
                {
                    if (int.Parse(name.Split('_')[1])< int.Parse(lastname.Split('_')[1]))
                    {
                        lastname = name;
                    }
                }
                System.IO.File.Delete(Application.persistentDataPath + "/SaveFiles/" + lastname);
            }
        }
        
    }

    public void DeleteSlot(int slotID)
    {
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles/"))
        {
            string[] listpaths = System.IO.Directory.GetFiles(Application.persistentDataPath + "/SaveFiles/");
            List<string> listnames = new List<string>();
            foreach (string path in listpaths)
            {
                string newname = path;
                string abstractpath = Application.persistentDataPath + "/SaveFiles/";
                newname = newname.Substring((int)(abstractpath.Length), newname.Length - abstractpath.Length);
                if (int.Parse("" + newname[0]) == slotID)
                {
                    System.IO.File.Delete(Application.persistentDataPath + "/SaveFiles/" + newname);
                }    
            }
        }
    }

    public void DeleteAllSlots()
    {
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles/"))
        {
            string[] listpaths = System.IO.Directory.GetFiles(Application.persistentDataPath + "/SaveFiles/");
            List<string> listnames = new List<string>();
            foreach (string path in listpaths)
            {
                System.IO.File.Delete(path);
            }
        }
    }

    public void SaveToFile()
    {
        InitializeSaveObject(CurrentSlot,save.elapsedseconds);
        string json = JsonUtility.ToJson(save);
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/SaveFiles");
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + save.slotID+"_"+save.elapsedseconds, json);
        Delete6thSave(save.slotID);
    }

}
