using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    GameObject boss;
    Vector2 originalSize;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        originalSize = GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        float healtBarSize = boss.GetComponent<Enemy>().curHealth / (float)boss.GetComponent<Enemy>().maxHealth;
        GetComponent<RectTransform>().sizeDelta = new Vector2(originalSize.x * healtBarSize, GetComponent<RectTransform>().sizeDelta.y);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(99 * healtBarSize , GetComponent<RectTransform>().anchoredPosition.y);
        if(healtBarSize == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
