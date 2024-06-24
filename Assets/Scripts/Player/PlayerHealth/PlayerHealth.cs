using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    public int currentHealth;
    public Healthbar healthbar;
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //animator.SetTrigger("Hurt");
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public float delayTime = 3f;
    void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(LoadSceneAfterDelay("BossScene"));
    }
    
    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayTime); // Wait for 3 seconds
        SceneManager.LoadScene(sceneName);
    }
    
}
