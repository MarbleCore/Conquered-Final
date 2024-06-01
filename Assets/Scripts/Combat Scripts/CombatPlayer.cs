using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int maxMana = 100;
    public int currentMana;

    public int maxEnergy = 10;
    public int currentEnergy;

    public HealthBar healthBar;
    public ManaBar manaBar;
    public EnergyBar energyBar;
    public LogicScriptPlayerInfo logic;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            changeHealth(-5);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            changeMana(-5);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            changeEnergy(-1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth = 100;
            changeHealth(0);
            currentMana = 100;
            changeMana(0);
            currentEnergy = 10;
            changeEnergy(0);
        }
    }

    public void changeHealth(int change)
    {
        currentHealth += change;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        } 
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
        logic.changeHealthValue(currentHealth);
    }

    public void changeMana(int change)
    {
        currentMana += change;
        if (currentMana < 0)
        {
            currentMana = 0;
        }
        else if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        manaBar.SetMana(currentMana);
        logic.changeManaValue(currentMana);
    }

    public void changeEnergy(int change)
    {
        currentEnergy += change;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        else if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        energyBar.SetEnergy(currentEnergy);
        logic.changeEnergyValue(currentEnergy);
    }
}
