//  By Kasperi K
//
//  Handles the player health and collisions with enemies

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public float playerHealth = 100f;
    public GameManager gM;

    private bool isAlive = true;

    void Update()
    {
        // If players health reaches zero, game ends
        if (playerHealth <= 0 && isAlive)
        { 
            isAlive = false;
            gM.GameOver();
        }
    }

    void OnCollisionEnter (Collision col)
    {

        // If player collides with spider
        if (col.gameObject.tag == "Enemy")
        {
            // Take damage
            playerHealth -= 10;

            // Calculate direction away from the palyer and push spider away
            Vector3 direction = new Vector3(col.transform.position.x - transform.position.x , 0 , col.transform.position.z - transform.position.z);
            direction.Normalize();
            StartCoroutine(col.gameObject.GetComponent<Enemy>().PushAway(direction));
        }
    }

}
