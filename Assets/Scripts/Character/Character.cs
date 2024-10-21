using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    public float mHealth;
    public float Health;
    public float Stamina;
    public float staminaDelay;
    public float mStamina;
    public float StaminaUseage;
    public float Level;
    public float attackDelay;
    public float IFrameTime;
    public float IFrameTimer;

    public float experiencePoints;
    public float requiredExperiencePoints;

    public float baseAttack;
    public float moveSpeed;

    public bool canHeal;
    public bool isInvincible;
    public bool isRunning;

    /*
        The 'Stats' class is responsable for creating and defining the stats of a character and manage any modifactions to the stats.
     */

    public Stats(int level)
    {
        //Uses the level to generate stats that scale 
        StaminaUseage = 2f;
        Level = level;
        moveSpeed = 3.0f;
        experiencePoints = 0.0f;

        CalculateStats(level);

        mHealth = Health;
        mStamina = Stamina;
        staminaDelay = 0.0f;
        canHeal = false;
        isRunning = false;
        isInvincible = false;
        IFrameTime = 1.5f;
        IFrameTimer = 0.0f;
    }

    public void CalculateStats(float level)
    {
        Health = 15 * level;
        Stamina = 6 * level;
        baseAttack = 1;

        requiredExperiencePoints = level*100;
        experiencePoints = 0;
        level++;
    }

    public void AddExp(float xp)
    {
        experiencePoints += xp;
    }

    public float CalculateRequiredExperience()
    {
        float exp = requiredExperiencePoints - experiencePoints;
        return exp;
    }

    public void LogStats()
    {
        Debug.Log("HP: "+Health.ToString());
        Debug.Log("STM: "+Stamina.ToString());
        Debug.Log("lV: "+Level.ToString());
        Debug.Log("XP: "+experiencePoints.ToString());
        Debug.Log("REQ XP: "+requiredExperiencePoints.ToString());
        Debug.Log("Attack: "+baseAttack.ToString());
    }
}

public enum AliveState 
{
    Alive,
    Dead
}

/*
    The 'Character' class is created and defined on the controller of an entity
 */
[System.Serializable]
public class Character
{
    public GameObject Sprite;
    public SpriteRenderer Renderer;
    public Animator anim;
    public Stats myStats;
    
    public AliveState aliveState = AliveState.Alive;
    public bool canMove;


    public Rigidbody2D myRB;

    public PolygonCollider2D PC2Dcollider;
    public CircleCollider2D C2Dcollider;

    public Character(int level)
    {
        myStats = new Stats(level);
        myStats.baseAttack = 2;
        myStats.isInvincible = false;
        canMove = false;
    }

    public void ChangeHealthValue(float h)
    {
        if ((myStats.Health += h) <= 0)
        {
            myStats.Health = 0;
            aliveState = AliveState.Dead;
            KillCharacter();
        }
        else
        {
            myStats.Health += h;
            if(myStats.Health < 0) { myStats.Health = 0; }
        }
    }

    public void RegenerateHealth() // ONLY PLAYER
    {
        /*        if(myStats.Health < myStats.mHealth)
                {
                    myStats.Health += Time.deltaTime * 1.5f;
                    PlayerUserInterFace.instance.UpdateHealthUI(myStats.Health);
                }*/

        myStats.Health = myStats.mHealth;
    }

    public void KillCharacter()
    {
        Renderer.color = Color.grey;
        anim.SetTrigger("death");
        aliveState = AliveState.Dead;
    }

    public void UseStamina(float usage)
    {
        myStats.Stamina -= usage;
        myStats.staminaDelay += 1.5f;
    }

    public void RegenerateStamina()
    {
        myStats.Stamina += Time.deltaTime*2.5f;
    }

    public void StaminaDelay()
    {
        myStats.staminaDelay -= Time.deltaTime;
    }

    public void GainExp(int xp)
    {
        myStats.AddExp(xp);
    }

    public void ToggleHealthGeneration()
    {
        myStats.canHeal = !myStats.canHeal;
    }

    public void Attack(Character chr)
    {
        if(myStats.attackDelay <= 0)
        {
            myStats.attackDelay += 3.5f;

            Debug.Log("at: " + myStats.baseAttack.ToString());

            if(chr.aliveState != AliveState.Dead) { chr.TakeDamage(myStats.baseAttack); }
            
        }
    }

    public void AttackDelay()
    {
        myStats.attackDelay -= Time.deltaTime; 
    }

    public void TakeDamage(float damage)
    {
        if (!myStats.isInvincible) { ChangeHealthValue(-damage); }
    }

    public void UseIFrames()
    {
        myStats.IFrameTimer = myStats.IFrameTime;
        myStats.isInvincible = true;
        Debug.Log("IFrames");
    }

    public void IFrameCountdown()
    {
        myStats.IFrameTimer -= Time.deltaTime;
        if(myStats.IFrameTimer <= 0) { myStats.isInvincible = false;
            Debug.Log("end");
        }
    }

    public void DisableColliders()
    {
        PC2Dcollider.enabled = false;
        C2Dcollider.enabled = false;
    }

    public void Revive() // ONLY PLAYER
    {
        anim.SetTrigger("revive");
        aliveState = AliveState.Alive;
        myStats.Health = myStats.mHealth;
        PlayerUserInterFace.instance.UpdateHealthUI(myStats.mHealth);
        myStats.Stamina = myStats.mStamina;
        PlayerUserInterFace.instance.UpdateStaminaUI(myStats.mHealth);
        Renderer.color = Color.white;
    }

    public void ToggleRun()
    {
        myStats.isRunning = !myStats.isRunning;
    }

}
