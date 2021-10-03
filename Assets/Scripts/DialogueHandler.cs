using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    public string[] dialogue;
    public string[] dialogueQuestComplete;


    GameObject player;
    PlayerStats playerStats;

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI dialogueText;

    public GameObject speechBubble;
    public Sprite speechBubbleExclamation;
    public Sprite speechBubbleQuestionMark;


    AudioSource audioSource;
    public AudioClip[] voiceClips;

    //play on enter or if player must interact first
    public bool startOnEnter;

    //enable door
    public bool enableDoor;
    public GameObject doorToOpen;
    public GameObject doorTilemap;

    //Clearing path 
    public bool clearPath;
    public GameObject blockTilemap;

    //Spawn in boss
    public bool enableBoss;
    public GameObject boss;
    public GameObject bossHealthBar;
    //Enable doublejumping and shooting through dialogue
    public bool enableDoubleJump;
    public bool enableShooting;

    // Quests
    public bool killCountQuest;
    public int killAmmount;
    public bool keyQuest;
    public GameObject doorToOpenQuest;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
    }

    bool dialogueShowing;
    public bool inRange;
    bool runOnce = true;

    public void Update()
    {

        if (keyQuest && PlayerStats.keys >= 1)
        {
            speechBubble.GetComponent<SpriteRenderer>().sprite = speechBubbleQuestionMark;
        }
        else if (keyQuest)
        {
            speechBubble.GetComponent<SpriteRenderer>().sprite = speechBubbleExclamation;
        }
        if (killCountQuest && PlayerStats.enemiesKilled >= killAmmount && runOnce)
        {
            runOnce = false;
            speechBubble.GetComponent<SpriteRenderer>().sprite = speechBubbleQuestionMark;
            dialogueText.text = "Mission Complete. Total enemies killed: " + PlayerStats.enemiesKilled;
            if (clearPath)
            {
                blockTilemap.SetActive(false);
            }
            DisplayText(dialogueQuestComplete);
        }
        else if (killCountQuest)
        {
            speechBubble.GetComponent<SpriteRenderer>().sprite = speechBubbleExclamation;
        }

        if (inRange || dialogueShowing)
        {
            if (startOnEnter)
            {
                GetComponent<BoxCollider2D>().enabled = false;

                if(keyQuest && PlayerStats.keys >= 1)
                {
                    DisplayText(dialogueQuestComplete);
                    //Open door with key
                    doorToOpenQuest.GetComponent<ChangeScene>().isEnabled = true;
                }
                else if(killCountQuest &&  PlayerStats.enemiesKilled >= killAmmount)
                {
                    DisplayText(dialogueQuestComplete);
                }
                else
                {
                    DisplayText(dialogue);
                }
            }

            else if (Input.GetButtonDown("Enter"))
            {
                if (keyQuest && PlayerStats.keys >= 1)
                {
                    DisplayText(dialogueQuestComplete);
                    //Open door with key
                    doorToOpenQuest.GetComponent<ChangeScene>().isEnabled = true;
                }
                else if (killCountQuest && PlayerStats.enemiesKilled >= killAmmount)
                {
                    DisplayText(dialogueQuestComplete);
                }
                else
                {
                    DisplayText(dialogue);
                }
            }
        }

        if (dialogueShowing)
        {
            speechBubble.SetActive(false);
            interactText.enabled = false;
        }
        else if (inRange)
        {
            speechBubble.SetActive(true);
            interactText.enabled = true;
        }
        else
        {
            speechBubble.SetActive(true);
        }
    }

    bool cancelTyping;
    bool isTyping;
    int currentLine = -1;
    bool runOneTime = true;
    
    void DisplayText(string[] s)
    {
        dialogueShowing = true;
        dialogueText.enabled = true;
        playerStats.CanMove = false;

        if (Input.GetButtonDown("Enter") || startOnEnter && runOneTime)
        {
            runOneTime = false;
            if (!isTyping)
            {
                currentLine += 1;

                //End of dialogue
                if (currentLine > s.Length-1)
                {
                    currentLine = -1;                   
                    dialogueShowing = false;
                    //Open door when dialogue is finished
                    if (enableDoor)
                    {
                        doorTilemap.SetActive(true);
                        doorToOpen.GetComponent<ChangeScene>().isEnabled = true;
                    }                 
                    //Enable doublejump
                    if (enableDoubleJump)
                    {
                        player.GetComponent<Player>().canDoubleJump = true;
                    }
                    if (enableBoss && boss != null)
                    {
                        boss.SetActive(true);
                        bossHealthBar.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    //Enable shooting
                    if (enableShooting)
                    {
                        player.GetComponent<WeaponHandler>().canShoot = true;
                    }
                    dialogueText.enabled = false;
                    playerStats.CanMove = true;
                }
                else
                {
                    StartCoroutine(TextScroll(s[currentLine]));
                }
            }
            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
             
        }        
    }

    //Produces scrolling text effect and sounds
    IEnumerator TextScroll(string text)
    {
        int letter = 0;
        isTyping = true;
        cancelTyping = false;
        dialogueText.text = "";

        while (letter < text.Length-1 && !cancelTyping && isTyping)
        {
            dialogueText.text += text[letter];
            audioSource.PlayOneShot(voiceClips[Random.Range(0, voiceClips.Length)]);
            letter++;
            yield return new WaitForSeconds(.11f);

        }

        dialogueText.text = text;
        cancelTyping = false;
        isTyping = false;
    }
    
}
