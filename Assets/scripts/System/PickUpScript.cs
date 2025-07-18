using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PickUpScript : MonoBehaviour
{
    [System.Serializable]
    public class Pickup
    {
        public int Type; // 0 chain, 1 Plate, 2 drone, 3 Augment, 4 Gadget, 5 Crystal Shard, 6 Metal Scraps, 7 Core Pieces, 8 Electronic Components
        public int ID;
        public int WorldFlagID;
        public int Quantity;
    }

    public Pickup pickup;

    private Global global;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EquipmentScript>() != null)
        {
            collision.GetComponent<EquipmentScript>().ReceiveItem(pickup.Type, pickup.ID, pickup.Quantity);
            global.worldflags[pickup.WorldFlagID] = true;
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(global == null)
        {
            global = FindAnyObjectByType<Global>();
        }
        else
        {
            if (global.worldflags[pickup.WorldFlagID]==true)
            {
                Destroy(gameObject);
            }
        }
    }
}
