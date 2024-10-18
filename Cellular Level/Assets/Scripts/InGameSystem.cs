using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSystem : MonoBehaviour
{
    public float oxygenAmount = 100f;
    public float damagePerSecond = 0.0833f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oxygenAmount > 0)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage(float damage)
    {
        oxygenAmount -= damage;
        oxygenAmount = Mathf.Clamp(oxygenAmount, 0, 100);
    }

    public void Heal(float healingAmount)
    {
        oxygenAmount -= healingAmount;
        oxygenAmount = Mathf.Clamp(oxygenAmount, 0, 100);
    }
}
