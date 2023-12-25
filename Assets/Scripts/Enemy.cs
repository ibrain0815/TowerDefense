using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public float health = 100;
    public float speed ;
    public float startSpeed = 10f;

    public int worth = 50;
    public GameObject deathEffect;


    private void Start()
    {
        speed = startSpeed;
    }
    public void TakeDamege(float amount)
    {
        health -= amount;
        if(health < 0)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = speed *(1f - pct);
    }

    void Die()
    {
        PlayerStats.Money += worth;
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect,5f);
        Destroy(gameObject);
    }

}
