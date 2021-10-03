using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 1;
    float destroyTimer = 5f;
    public bool enemyBullet;
    Rigidbody2D rigidbody;
    public float bulletSpeed = 5f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GameObject.Destroy(this.gameObject,destroyTimer);
    }
    void Update()
    {
        rigidbody.velocity = bulletSpeed * (rigidbody.velocity.normalized);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!enemyBullet)
        {
            if (col.transform.tag == "Platform" || col.transform.tag == "Enemy" || col.transform.tag == "Boss")
            {
                if (col.transform.tag == "Enemy" || col.transform.tag == "Boss")
                {
                    col.transform.GetComponent<Enemy>().TakeDamage(damage);
                }
                //Particle effects
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            if (col.transform.tag == "Platform" || col.transform.tag == "Player")
            {
                if (col.transform.tag == "Player")
                {
                    col.transform.GetComponent<PlayerStats>().TakeDamage(damage);
                }
                //Particle effects
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
