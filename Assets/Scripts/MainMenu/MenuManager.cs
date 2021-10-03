using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class MenuManager : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public List<GameObject> buttons;
    public AudioSource audioSource;
    bool keyDown;
    public AudioClip buttonSound;
    public GameObject Credits;
    public GameObject Settings;

    public GameObject lastSelectedGameObject;
    private GameObject currentSelectedGameObject_Recent;

    void Start()
    {
        EventSystem.current.firstSelectedGameObject = buttons[0];
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        GetLastGameObjectSelected();

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        }
    }

    private void GetLastGameObjectSelected()
    {
        if (EventSystem.current.currentSelectedGameObject != currentSelectedGameObject_Recent)
        {

            lastSelectedGameObject = currentSelectedGameObject_Recent;

            currentSelectedGameObject_Recent = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        audioSource.PlayOneShot(buttonSound, 0.25f);

    }

    public void OnSelect(BaseEventData eventData)
    {
        audioSource.PlayOneShot(buttonSound, 0.25f);
    }

    //BUTTON FUNCTIONALITY

    //Play sound effect
    public void PlaySoundEffect(AudioClip audio)
    {
        audioSource.PlayOneShot(audio,0.25f);
    }

    //Change scene
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(1);
    }

    //Open up settings menu
    public void ToggleSettings()
    {
        if (Settings.activeSelf == true)
        {
            Settings.SetActive(false);
        }
        else
        {
            Settings.SetActive(true);
            Credits.SetActive(false);
        }
    }
    public void ToggleCredits()
    {
        if (Credits.activeSelf == true)
        {
            Credits.SetActive(false); 
        }
        else
        {
            Credits.SetActive(true);
            Settings.SetActive(false);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
