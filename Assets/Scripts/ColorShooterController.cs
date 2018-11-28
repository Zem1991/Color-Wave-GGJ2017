using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShooterController : MonoBehaviour
{

    public float fireCooldown = 0.4f;
    public float bulletSpeed = 25f;
    public float reverseGravityTime = 3f;
    public float gravityFlipSmoothing = 0.1f;
    public float highJump = 20f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public Animator gunAnimator;
    public float maxAmmo = 100f, shotAmmo = 30f, ammoRecovery = 0.025f;
    public float blueAmmo, redAmmo, greenAmmo, yellowAmmo;
    public bool blueCooldown = false, redCooldown = false, greenCooldown = false, yellowCooldown = false;
    public MeshRenderer ammoSphere;
    public GameObject weapon;
    public AudioSource audioSource;
    public AudioClip highJumpSound;
    public AudioClip waveSound;
    public float waveSoundDistance = 5f;
    public AudioClip blueSound;
    public AudioClip redSound;
    public AudioClip shotSound;
    public AudioClip checkpointSound;
    public AudioClip[] music;
    public AudioSource musicPlayer;

    private int nextMusic = 0;

    public LayerMask groundLayers;
    public float groundMargin = 0.05f;
    public float groundSmoothing = 0.5f;

    public int reactionStrength = 5;
    [Header("Ammo Clips")]
    public GameObject blueClip;
    public GameObject greenClip, redClip, yellowClip;

    public ColorCube.CubeColor ammoType = ColorCube.CubeColor.blue;

    private float lastFire = -999f, timeTime = 0f;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Rigidbody rb;
    private Material sphereMaterial;
    private float lastReverseGravity = -999f;
    private Vector3 startGravity;

    public bool hasWeapon = false;
    public bool canShootBlue = true;
    public bool canShootGreen = false;
    public bool canShootRed = false;
    public bool canShootYellow = false;

    private Transform groundCheck;
    private CapsuleCollider capsuleCollider;
    private float capsuleHeight;
    private static int animatorParameterFire = Animator.StringToHash("Fire");

    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleHeight = capsuleCollider.height;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        blueAmmo = maxAmmo; redAmmo = maxAmmo; greenAmmo = maxAmmo; yellowAmmo = maxAmmo;

        sphereMaterial = new Material(ammoSphere.material);
        ammoSphere.material = sphereMaterial;
        UpdateAmmoSphere();
        UpdateWeapon();
        startGravity = Physics.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        timeTime = Time.time;

        if (Input.GetMouseButtonDown(0))
        {
            FireBullet(ammoType);
        }

        if (Input.GetMouseButtonDown(1) || (ammoType == ColorCube.CubeColor.blue && !canShootBlue) || (ammoType == ColorCube.CubeColor.green && !canShootGreen) || (ammoType == ColorCube.CubeColor.red && !canShootRed) || (ammoType == ColorCube.CubeColor.yellow && !canShootYellow))
        {
            if (ammoType == ColorCube.CubeColor.blue) ammoType = ColorCube.CubeColor.green;
            else if (ammoType == ColorCube.CubeColor.green) ammoType = ColorCube.CubeColor.red;
            else if (ammoType == ColorCube.CubeColor.red) ammoType = ColorCube.CubeColor.yellow;
            else if (ammoType == ColorCube.CubeColor.yellow) ammoType = ColorCube.CubeColor.blue;
            UpdateAmmoSphere();
        }

        if (Input.GetKey(KeyCode.Alpha1)) { ammoType = ColorCube.CubeColor.blue; UpdateAmmoSphere(); }
        if (Input.GetKey(KeyCode.Alpha2)) { ammoType = ColorCube.CubeColor.green; UpdateAmmoSphere(); }
        if (Input.GetKey(KeyCode.Alpha3)) { ammoType = ColorCube.CubeColor.red; UpdateAmmoSphere(); }
        if (Input.GetKey(KeyCode.Alpha4)) { ammoType = ColorCube.CubeColor.yellow; UpdateAmmoSphere(); }

        if (Time.time > lastReverseGravity + reverseGravityTime)
        {
            Physics.gravity = startGravity;
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f)),gravityFlipSmoothing);//Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, new Vector3(0f, -1f, 0f)), gravityFlipSmoothing);
        }
        else
        {
            Physics.gravity = -startGravity;
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180f)),gravityFlipSmoothing);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, new Vector3(0f, 1f, 0f)), gravityFlipSmoothing);
        }
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.clip = music[nextMusic];
            musicPlayer.Play();
            nextMusic++;
            if (nextMusic >= music.Length)
                nextMusic = 0;
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, new Vector3(0f, -1f, 0f));
        RaycastHit rayHit;
        bool hit = Physics.Raycast(ray, out rayHit, -groundCheck.localPosition.y, groundLayers, QueryTriggerInteraction.Ignore);
        if (hit && rayHit.distance < capsuleHeight / 2f - groundMargin)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0f, ((capsuleHeight / 2f) - rayHit.distance + groundMargin * 2f), 0f), groundSmoothing);
        }

        //AMMO
        blueAmmo += ammoRecovery; redAmmo += ammoRecovery; greenAmmo += ammoRecovery; yellowAmmo += ammoRecovery;
        if (blueAmmo > maxAmmo) { blueAmmo = maxAmmo; blueCooldown = false; }
        if (redAmmo > maxAmmo) { redAmmo = maxAmmo; redCooldown = false; }
        if (greenAmmo > maxAmmo) { greenAmmo = maxAmmo; greenCooldown = false; }
        if (yellowAmmo > maxAmmo) { yellowAmmo = maxAmmo; yellowCooldown = false; }
        if (blueAmmo < 0f && !blueCooldown) { blueAmmo = 0f; blueCooldown = true; }
        if (redAmmo < 0f && !redCooldown) { redAmmo = 0f; redCooldown = true; }
        if (greenAmmo < 0f && !greenCooldown) { greenAmmo = 0f; greenCooldown = true; }
        if (yellowAmmo < 0f && !yellowCooldown) { yellowAmmo = 0f; yellowCooldown = true; }
    }

    void FireBullet(ColorCube.CubeColor color)
    {
        canShootYellow = false;
        if ((color == ColorCube.CubeColor.blue && (blueCooldown || !canShootBlue)) ||
            (color == ColorCube.CubeColor.green && (greenCooldown || !canShootGreen)) ||
            (color == ColorCube.CubeColor.red && (redCooldown || !canShootRed)) ||
            (color == ColorCube.CubeColor.yellow && (yellowCooldown || !canShootYellow)))
            return; // Not able to fire, abort

        if (timeTime > lastFire + fireCooldown)
        {
            audioSource.PlayOneShot(shotSound);
            gunAnimator.SetTrigger(animatorParameterFire);
            lastFire = timeTime;
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);

            HitReaction bulletScript = bullet.GetComponent<HitReaction>();
            if (bulletScript)
            {
                bulletScript.color = color;
                bulletScript.UpdateColor(color);
            }

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb)
            {
                bulletRb.velocity = rb.velocity + bulletRb.transform.forward * bulletSpeed;
                bulletRb.useGravity = false;
            }

            switch (color)
            {
                case ColorCube.CubeColor.blue: blueAmmo -= shotAmmo; break;
                case ColorCube.CubeColor.red: redAmmo -= shotAmmo; break;
                case ColorCube.CubeColor.green: greenAmmo -= shotAmmo; break;
                case ColorCube.CubeColor.yellow: yellowAmmo -= shotAmmo; break;
            }
        }
    }

    public void RespawnPlayer()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        RigidbodyFirstPersonController fpsController = GetComponentInChildren<RigidbodyFirstPersonController>();
        fpsController.SetRotation(spawnRotation);
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    private void UpdateAmmoSphere()
    {
        Color color = Color.white;
        switch (ammoType)
        {
            case ColorCube.CubeColor.green: color = Color.yellow; break;
            case ColorCube.CubeColor.blue: color = Color.blue; break;
            case ColorCube.CubeColor.yellow: color = Color.yellow; break;
            case ColorCube.CubeColor.red: color = Color.red; break;
        }
        sphereMaterial.color = color;
    }

    public void UpdateWeapon()
    {
        weapon.SetActive(hasWeapon);
        blueClip.SetActive(canShootBlue);
        greenClip.SetActive(canShootGreen);
        redClip.SetActive(canShootRed);
        yellowClip.SetActive(canShootYellow);
        UpdateAmmoSphere();
    }

    public void Checkpoint(Vector3 newSpawnPosition, Quaternion newSpawnRotation)
    {
        spawnPosition = newSpawnPosition;
        spawnRotation = newSpawnRotation;
    }

    public Vector3 GetCheckpoint()
    {
        return spawnPosition;
    }

    public void ReverseGravity()
    {
        lastReverseGravity = timeTime;
    }
}
