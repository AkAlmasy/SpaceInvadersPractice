using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 15;

    [Header("Enemy Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 0.3f;
    [SerializeField] GameObject laserShot;
    [SerializeField] float projectileSpeed = 5f;

    [Header("Enemy Effects")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float explosionLingerTime = 0.5f;
    [SerializeField] AudioClip enemyExplosionSound;
    [SerializeField] [Range(0,1)] float enemyExplosionSoundVolume = 0.5f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.2f;



    // Use this for initialization
    void Start () {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update () {
        CountDownAndShoot();
	}

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserShot, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DmgDealer dmgDealerAsset = other.gameObject.GetComponent<DmgDealer>();
        if (!dmgDealerAsset) { return; }
        ProcessHit(dmgDealerAsset);
    }

    private void ProcessHit(DmgDealer dmgDealerAsset)
    {
        health -= dmgDealerAsset.GetDamage();
        dmgDealerAsset.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, explosionLingerTime);
        AudioSource.PlayClipAtPoint(enemyExplosionSound, Camera.main.transform.position, enemyExplosionSoundVolume);
        FindObjectOfType<GameSession>().AddToScore(scoreValue);

    }
}
