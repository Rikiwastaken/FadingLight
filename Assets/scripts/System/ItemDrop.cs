using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static PickUpScript;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ItemDrop : MonoBehaviour
{
    public int type; //6 Metal Scraps, 7 Core Pieces, 8 Electronic Components

    public int quantity;

    private Transform player;

    public float speed;

    private int timealive;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerHP>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timealive++;
        Vector2 offset = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f))*speed;
        if(timealive>  5 / Time.fixedDeltaTime)
        {
            offset = Vector2.zero;
        }
        GetComponent<Rigidbody2D>().AddForce(((Vector2)(player.transform.position - transform.position).normalized * speed) + offset, ForceMode2D.Impulse);

        if(Vector2.Distance(player.transform.position, transform.position)<=0.5f && timealive>=1/Time.fixedDeltaTime)
        {
            player.GetComponent<EquipmentScript>().ReceiveItem(type, 0, quantity);
            Destroy(gameObject);
        }

    }
}
