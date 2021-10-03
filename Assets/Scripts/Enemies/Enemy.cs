using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{

    Animator anim;
    Collider2D collider;

    //Will summon enemies or not
    public bool canSummon;
    public GameObject enemySummon;

    //Will shoot 
    public Vector3 bulletOffset;
    public bool canShoot;
    public GameObject bullet;
    public float shootTime = 10f;
    public float bulletSpeed;

    public Vector3 spawnOffset;
    public List<GameObject> summonedEnemies;
    public int maxSummons = 3;
    public float spawnTime = 10f;
    //Damage to player
    public int damageAmmount = 1;


    public int maxHealth = 1;
    public int curHealth;

    public float speed = 0.5f;
    bool dead = false;
    
    public Vector3 dir;
    public LayerMask collisionMask;
    float enemyScale;
    public float raycastLength = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        curHealth = maxHealth;
        enemyScale = transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (canSummon)
            {
                Summon();
            }
            else if (canShoot)
            {
                Vector3 shootingDirection = dir;
                Shoot(shootingDirection);
            }
            Patrol();
        }
        //Set facing direction
        transform.localScale = new Vector3(enemyScale * dir.x, transform.localScale.y, transform.localScale.z);

    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(ColorFlash());
        curHealth -= damage;
        if (curHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ColorFlash()
    {
        anim.SetBool("takingDamage", true);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        anim.SetBool("takingDamage", false);
    }

    public void Die()
    {
        //Add enemies killed to player stats
        PlayerStats.enemiesKilled++;
        dead = true;
        anim.SetBool("dead", dead);
        //Disable collider so character cannot touch enemy
        collider.enabled = false;
        GameObject.Destroy(this.gameObject,10);
    }

    bool isSummoning = false;
    bool canMove = true;

    public void Summon()
    {
        //Clear empty spots
        summonedEnemies = summonedEnemies.Where(item => item != null).ToList();
        if (!isSummoning && summonedEnemies.Count < maxSummons && !dead)
        {
            StartCoroutine(SpawnTimer());
            isSummoning = true;
        }
    }

    IEnumerator SpawnTimer()
    {

        yield return new WaitForSeconds(spawnTime);
        anim.SetBool("summoning", true);
        canMove = false;
        yield return new WaitForSeconds(.7f);
        canMove = true;
        //to fix the issues with translucency
        Vector3 zOffset = new Vector3(0, 0, 0.0001f + 0.0001f * summonedEnemies.Count);
        if (!dead)
        {
            GameObject temp = Instantiate(enemySummon, transform.position + spawnOffset + zOffset, Quaternion.identity);
            summonedEnemies.Add(temp);
        }
     
        anim.SetBool("summoning", false);
        isSummoning = false;
    }

    public void Patrol()
    {
        //Only move if its not summoning
        if (canMove)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dir.x, raycastLength, collisionMask);

        Debug.DrawRay(transform.position, Vector2.right * dir.x * raycastLength, Color.green);

        if (hit)
        {
            if (hit.transform.tag == "Platform")
            {
                //Change direciton on hit
                if (dir.x == 1)
                {
                    dir.x = -1;
                }
                else
                {
                    dir.x = 1;
                }
            }
        }

        //Floor collision
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.right/10 * dir.x, Vector2.down, raycastLength*1.75f, collisionMask);

        Debug.DrawRay(transform.position + Vector3.right/10 * dir.x, Vector2.down * raycastLength*1.75f, Color.green);

        
        if (hit1)
        {
            //Do nothing
        }
        else
        {         
            //Change direciton on hit
            if (dir.x == 1)
            {
                dir.x = -1;
            }
            else
            {
                dir.x = 1;
            }
        }
    }
    bool isShooting = false;

    public void Shoot(Vector3 shootingDirection)
    {
        if (!isShooting && !dead)
        {
            StartCoroutine(ShootTimer(shootingDirection));
            isShooting = true;
        }
    }

    IEnumerator ShootTimer(Vector3 shootingDirection)
    {
        yield return new WaitForSeconds(shootTime);
        anim.SetBool("shooting", true);
        canMove = false;
        yield return new WaitForSeconds(.7f);
        canMove = true;
      
        if (!dead)
        {
            GameObject bulletObject = Instantiate(bullet, new Vector3((transform.position.x) + -(bulletOffset.x * shootingDirection.x), transform.position.y + bulletOffset.y), Quaternion.identity);
            //Rotate bullet to enemies direction
            bulletObject.transform.localScale = new Vector3(bulletObject.transform.localScale.x * -shootingDirection.x, bulletObject.transform.localScale.y, bulletObject.transform.localScale.z);
            Rigidbody2D bulletClone = bulletObject.GetComponent<Rigidbody2D>();

            bulletClone.velocity = new Vector3(shootingDirection.x * -bulletSpeed, 0);
        }
        anim.SetBool("shooting", false);
        isShooting = false;
    }
}
