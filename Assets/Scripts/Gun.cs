//  By Kasperi K, audio by Samuli T
//  (and tutorials)
//
//  Handles the shooting and the gun breaking

using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    // References set in the editor
    public Transform barrelEnd;
    public ParticleSystem smoker;
    public ParticleSystem muzzleFlash;
    public Renderer rend_barrel;
    public float gunRange = 100f;
    public float gunDamage = 50f;
    public float minRepairTime = 0.5f;

    // Reference to the right hand controller
    public SteamVR_TrackedObject rightHand;

    // Audioclips and sources set in the editor
    public AudioClip[] audioList;
    public AudioSource audioSource;
    public AudioSource hammeringExtra;
    public AudioSource audioGunBreak;

    // Line renderer for drawing the laser
    private LineRenderer laserLine;

    // VAriables for gun breaking
    private bool isBroken = false;
    private float damageToGun = 0;
    private float nextRepair;

    // The time the laser is shown
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
    }


    void Update()
    {
        // If gun had been repaired, fix it
        if (isBroken && damageToGun <= 0)
        {
            FixGun();
        }

        // If right hand controllers trigger has been pressed and gun isn't broken, then shoot
        // Controls can be switched to "Fire1" for easier testing
        if( SteamVR_Controller.Input((int)rightHand.index).GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) /* Input.GetButtonDown("Fire1")*/ && !isBroken)
        {
            // Play the shooting effects
            StartCoroutine(ShotEffect());

            // Set the laser starting position to the end of the barrel
            laserLine.SetPosition(0, barrelEnd.position);
            
            // Create variable for storing hit data
            RaycastHit hit;

            // Sound for shooting
            float pitchAmount = Random.Range(-0.3f, 0.3f);
            audioSource.pitch = 1;
            audioSource.pitch += pitchAmount;
            int audioIndex = Random.Range(5, 6);
            audioSource.clip = audioList[audioIndex];
            audioSource.Play();

            // Make raycast from the barrel end
            if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out hit, gunRange))
            { 
                // If we hit something set the end point for the laser
                laserLine.SetPosition(1, hit.point);

                // If we hit an enemy, applay damage
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.Damage(gunDamage);

                }

                // If we hit start button, start the game
                if (hit.collider.CompareTag("Start"))
                {
                    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                    if (gm != null)
                        gm.StartGame();
                }
                // If we hit quit button, quit
                if (hit.collider.CompareTag("Quit"))
                {
                    Debug.Log("Quit game");
                    Application.Quit();
                }

            }
            else
            {
                // If we hit nothing, laser end point is at max gun range
                laserLine.SetPosition(1, barrelEnd.position + (barrelEnd.forward * gunRange));
            }

            // There is a 10% chance that the gun breaks after every shot
            if (Random.value < 0.1)
            {
                BreakGun();
            }
        }


    }

    // Gun breaks
    private void BreakGun()
    {
        Debug.Log("Gun broke :(");

        // Play sound
        audioSource.pitch = 1.4f;
        audioSource.clip = audioList[7];
        audioSource.Play();
        audioGunBreak.Play();


        // Gun starts to smoke and changes color
        smoker.Play();
        isBroken = true;
		rend_barrel.material.SetColor("_Color", Color.red);
        damageToGun = 100;
    }

    private void FixGun()
    {
        Debug.Log("Gun fixed :)");

        // Stop the smoke and return to the original color
        smoker.Stop();
        isBroken = false;
		rend_barrel.material.SetColor("_Color", Color.white);
        damageToGun = 0;
    }


    void OnTriggerEnter(Collider other)
    {   
        // If hammer hits the gun
        if (other.gameObject.tag == "Hammer")
        {
            //Hammering sounds
            int audioIndex = Random.Range(0, 4);
            audioSource.clip = audioList[audioIndex];
            audioSource.Play();

            float pitchAmount = Random.Range(-3f, 3f);
            hammeringExtra.pitch = 1;
            hammeringExtra.pitch += pitchAmount;
            hammeringExtra.Play();

            // If gun is broken and enough time has passed repair the gun a bit
            if (isBroken && Time.time > nextRepair)
            {
                nextRepair = Time.time + minRepairTime;
                damageToGun -= 50f;
            }
        }

    }


    // Draws the laser for a short duration and plays muzzle flash effect
    private IEnumerator ShotEffect()
    {

        laserLine.enabled = true;
        muzzleFlash.Stop();
        muzzleFlash.Play();

        yield return shotDuration;

        laserLine.enabled = false;
    }
}
