using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image oxygenBar;
    //public float healthAmount = 100f;
    //public float damagePerSecond = 0.0833f;
    public GameManager oxygenSystem;

    public Image staminaBar;
    public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*while(healthAmount > 0)
        {
            TakeDamage(1);
        }
        
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        /if (healthAmount > 0)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
        } */

        UpdateOxygenBar();
    }

    /*public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthBar.fillAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healingAmount / 100f;
    }*/

    private void UpdateOxygenBar()
    {
        oxygenBar.fillAmount = 
            oxygenSystem.GetOxygen() / oxygenSystem.GetTotalOxy();
    }

}
