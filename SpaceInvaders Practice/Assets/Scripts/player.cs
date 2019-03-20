using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {

    // config params
    [Header("Player")] 
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] float startHealth = 500;
    [SerializeField] private float currentHealth;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.5f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.2f;
    
    [Header("Projectile")]
    [SerializeField] GameObject laserShot;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float fireSpeed = 0.2f;

    [Header("Unity Stuff")]
    public Image healthBar;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;



    // Use this for initialization
    void Start () {
        SetUpMoveBoundaries();
        currentHealth = startHealth;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DmgDealer dmgDealerAsset = other.gameObject.GetComponent<DmgDealer>();
        if (!dmgDealerAsset) { return; }
        ProcessHit(dmgDealerAsset);
    }

    private void ProcessHit(DmgDealer dmgDealerAsset)
    {
        currentHealth -= dmgDealerAsset.GetDamage();
        dmgDealerAsset.Hit();
        healthBar.fillAmount = currentHealth / startHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<LvLControl>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);

    }



    private void Fire()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(AutoFire());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);
    }

    IEnumerator AutoFire()
    {
        while (true)
        { 
            GameObject laser = Instantiate(laserShot, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            yield return new WaitForSeconds(fireSpeed);
        }
    }
}
