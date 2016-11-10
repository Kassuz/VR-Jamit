//  By Kasperi K, audio bits by Samuli T 
//
//  Handles the enemy movement and death

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    // Audio clips and source set in the editor
    public AudioClip[] sounds;
    public AudioSource hitSource;

    public float health = 100f;
    public float forceMagnitude = 100f;

    // Used in the corutine which pushes the enemy
    private WaitForSeconds shotDuration = new WaitForSeconds(1f);

    private Transform player;
    private NavMeshAgent nav;
    private Rigidbody rb;
    private GameManager gM;

    void Awake()
    {
        // Get reference to the NavMeshAgent
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Get the references to components
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // If NavMeshAgent is aviable and active, move towards the player
        if (nav != null && nav.isActiveAndEnabled)
        {
            nav.SetDestination(player.position);
        }

        // If dead disable NavMeshAgent and destroy enemy
        if (health <= 0f)
        {
            nav.enabled = false;
            gM.AddPoints(1);
            Destroy(gameObject);
        }
    }


    // Called when enemy is damaged
    public void Damage(float lostHealth)
    {
        
        // Play random hit sound
        int audioIndex = Random.Range(0, 4);
        hitSource.clip = sounds[audioIndex];
        hitSource.Play();

        // Loose health
		this.health -= lostHealth;
    }


    // Corutine for pushing the enemy away to given direction
    public IEnumerator PushAway(Vector3 direction)
    {
        // NavMEshAgent has to be disabled before applying force
        nav.enabled = false;
        rb.AddForce(direction * forceMagnitude);

        // Wait for a while to enemy to fly away
        yield return shotDuration;

        // If enemy is still alive, stop it and enable NavMeshAgent
        if (rb != null && nav != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            nav.enabled = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Hit by the hammer
        if (collision.gameObject.tag == "Hammer")
        {
            // Calculate vector away from the position the hammer hit and then push enemy to that direction
            Vector3 direction = new Vector3(collision.transform.position.x - transform.position.x, 1 , collision.transform.position.z - transform.position.z);
            direction.Normalize();
            StartCoroutine(PushAway(direction));

            // Deal damage to the enemy
            Damage(50f);
        }
    }
}
