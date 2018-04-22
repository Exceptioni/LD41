using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : MonoBehaviour {

    public Enemy Enemy;
    private SpriteRenderer SpriteR;
    bool invulnerable = false;

    private Rigidbody2D rb;
    private void Awake()
    {
        SpriteR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
        transform.Translate(Vector2.down * Enemy.Speed * Time.deltaTime);

        float c = (float)Enemy.Health / (float)Enemy.MaxHealth;
        SpriteR.color = new Color(1, c, c, 1);

        if (transform.position.y < 12f)
        {
            SpriteR.color = new Color(0.1f, 0.1f, 0.1f, 1);

            if (!invulnerable)
            {
                GameManager.Instance.Player.TakeDamage(Enemy.Strength);
                invulnerable = true;
            }
            //Enemy.BonusSpeed = 2;

            if(transform.position.y < 9f)
            {
                Die(false);
            }
        }
    }

    public void Die(bool playerKilled)
    {
        GameManager.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void SetEnemy(Enemy e)
    {
        if (Enemy != null)
        {
            Enemy.GE = null;
        }
        Enemy = e;
        Enemy.GE = this;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Spell" && c.gameObject.GetComponent<GameCard>().IsInField)
        {
            if (invulnerable) return;
            c.gameObject.GetComponent<GameCard>().OnHitEnemy(Enemy);
        }
    }
}
