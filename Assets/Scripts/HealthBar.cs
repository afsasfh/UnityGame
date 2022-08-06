using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthPointImage;
    public Image healthPontEffect;

    private PlayerControlier player;

    private void Awake()
    {
        GameObject.FindGameObjectsWithTag("Player");
        player =GetComponent<PlayerControlier>();
        
    }
    private  void Update()
    {
        healthPointImage.fillAmount = player.currentHp / player.maxHp;
        if(healthPontEffect.fillAmount > healthPointImage.fillAmount)
        {
            healthPontEffect.fillAmount -= 0.003f;
        }else
        {
            healthPontEffect.fillAmount = healthPointImage.fillAmount;
        }
    }

}
