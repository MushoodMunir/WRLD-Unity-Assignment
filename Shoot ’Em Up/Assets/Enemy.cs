using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{



    public float startHealth = 100;

    public GameManager gameManager;

    private float health;
    private bool isDead = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        health = startHealth;
    }

    public void TakeDamage(float amount, GameObject enemy)
    {
        health -= amount;

        //healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            gameObject.SetActive(false);
            gameManager.enemyList.Remove(enemy);
        }
    }

}