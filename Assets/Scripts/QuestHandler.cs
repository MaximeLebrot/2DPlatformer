using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class QuestHandler : MonoBehaviour
{

    public string[] startText;
    public string[] duringText;

    public string[] completedText;

    GameObject player;
    PlayerStats playerStats;
    bool questMenuOpen;

    bool questAccepted;

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI dialogueText;

 AudioSource audioSource;
    public AudioClip[] voiceClips;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(this.gameObject.transform.position,player.transform.position) <= 1f || questMenuOpen)
        {
            if (Input.GetButtonDown("Enter"))
            {
                if (!questAccepted)
                {
                    QuestMenuOpen(startText);

                }
                else if (questAccepted)
                {
                    QuestMenuOpen(duringText);

                }
            }           

            if (questMenuOpen)
            {
                interactText.enabled = false;
            }
            else
            {
                interactText.enabled = true;
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    void QuestMenuOpen(string[] text)
    {
        questMenuOpen = true;
        DisplayText(text);
    }

    bool cancelTyping;
    bool isTyping;
    int currentLine = -1;

    void DisplayText(string[] s)
    {
        questMenuOpen = true;
        dialogueText.enabled = true;
        playerStats.CanMove = false;

        if (Input.GetButtonDown("Enter"))
        {
            if (!isTyping)
            {
                currentLine += 1;

                //End of dialogue
                if (currentLine > s.Length-1)
                {
                    currentLine = -1;

                    if (!questAccepted)
                    {
                        questAccepted = true;
                        print("Quest Accepted");
                    }
                    questMenuOpen = false;
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

    //Give reward and set as complete
    void CompleteQuest()
    {

    }
    
}
