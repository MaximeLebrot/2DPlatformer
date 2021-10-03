using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField]
    int maxHealth = 3;
    [SerializeField]
    int curHealth;
    [SerializeField]
    public static int coins;
    [SerializeField]
    public static int keys;
    [SerializeField]
    public static int enemiesKilled;

    Vector3 spawnPos;
    bool dead;
    bool canMove = true;

    //UI
    public Image deathScreen;
    public Image fadeInScreen;

    public TextMeshProUGUI interactText;

    //Hearts
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    //Collectibles
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI keyText;

    int baseDamage = 1;
    private int damageMultiplier = 1;

    bool invulnerable;
    //to set animator state
    bool takingDamage;

    Player player;
    AnimationController anim;

    //Properties
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }

    public bool TakingDamage { get => takingDamage; set => takingDamage = value; }
    public bool Dead { get => dead; set => dead = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public Vector3 SpawnPos { get => spawnPos; set => spawnPos = value; }

    void Start()
    {
        deathScreen.enabled = true;
        deathScreen.canvasRenderer.SetAlpha(0.0f);

        fadeInScreen.enabled = true;
        StartCoroutine(FadeIn());

        player = GetComponent<Player>();
        anim = GetComponent<AnimationController>();

        SpawnPos = transform.position;
        curHealth = maxHealth;
    }

    void LateUpdate()
    {
        UpdateUI();

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
    }

    public void UpdateUI()
    {
        coinText.text = "" + coins;
        keyText.text = "" + keys;

        //Display hearts on screen
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < curHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
       
    }

    public void TakeDamage(int damage)
    {    
        if (!invulnerable)
        {
            //Start timer
            StartCoroutine(TakeDamageCooldown());
            curHealth -= damage;
            if (curHealth <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator TakeDamageCooldown()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        invulnerable = true;
        TakingDamage = true;
        yield return new WaitForSeconds(.3f);
        invulnerable = false;
        TakingDamage = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    bool hasRespawned = false;
    public void Die()
    {
        //Make sure it only runs once
        if (!hasRespawned)
        {
            dead = true;
            canMove = false;
            hasRespawned = true;
            StartCoroutine(Respawn());
        }
    }

    bool fadingIn;

    public void FadeInScreen()
    {
        if (!fadingIn)
        {
            StartCoroutine(FadeIn());
        }
    }

    //Fades in screen
    IEnumerator FadeIn()
    {
        fadingIn = true;
        fadeInScreen.enabled = true;
        fadeInScreen.CrossFadeAlpha(0, 3, false);
        yield return new WaitForSeconds(3f);
        fadeInScreen.enabled = false;
        //Reset screen to black
        fadeInScreen.CrossFadeAlpha(1, 0, false);
        fadingIn = false;

    }

    IEnumerator Respawn()
    {
        deathScreen.CrossFadeAlpha(1, 2, true);
        yield return new WaitForSeconds(2f);
        transform.position = SpawnPos;
        player.ResetVelocity();
        curHealth = maxHealth;
        dead = false;
        canMove = true;
        deathScreen.CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(3f);
        hasRespawned = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "PickUp")
        {
            PickUpItem(col.transform.gameObject.GetComponent<PickUp>().pickupType,col);
        }

        if (col.transform.tag == "NPC")
        {
            interactText.enabled = true;
            col.transform.gameObject.GetComponent<DialogueHandler>().inRange = true;
        }

        //Handle changing scenes through portals
        //Only show interact text if door is enabled
        if (col.transform.tag == "Portal" && col.GetComponent<ChangeScene>().isEnabled == true)
        {
            interactText.enabled = true;
            col.transform.gameObject.GetComponent<ChangeScene>().inRange = true;        
        }

        //Collapse bridge for cutscene
        if (col.transform.tag == "Bridge")
        {         
            col.transform.gameObject.GetComponent<BridgeCollapse>().CollapseBridge();
        }

        //Trigger Cat run
        if (col.transform.tag == "Cat")
        {
            col.transform.gameObject.GetComponent<CatRunTrigger>().CatTriggered();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Portal")
        {
            interactText.enabled = false;
            col.transform.gameObject.GetComponent<ChangeScene>().inRange = false;
        }
        if (col.transform.tag == "NPC")
        {
            interactText.enabled = false;
            col.transform.gameObject.GetComponent<DialogueHandler>().inRange = false;
        }
    }

    void PickUpItem(string pickUpType, Collider2D col)
    {
        switch (pickUpType)
        {
            case "Health":
                //Only pickup if health isnt full
                if (curHealth < maxHealth && !dead)
                {
                    curHealth++;
                    Destroy(col.transform.gameObject);
                }            
                break;
            case "Key":
                keys++;
                Destroy(col.transform.gameObject);
                break;
            case "Coin":
                coins++;
                Destroy(col.transform.gameObject);
                break;
        }
    }  
}
