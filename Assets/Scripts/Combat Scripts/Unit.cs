using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //create variables
    public string unitName;

    public int damage;

    public int maxHP;
    public int currentHP;

    public int maxMana;
    public int currentMana;

    public int maxEnergy;
    public int currentEnergy;

    //make the unit take damage, bool for if the unit is still alive or not
    public bool TakeDamage(int damage, int multiplier)
    {
        currentHP -= damage * multiplier;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return false;
        } else
        {
            return true;
        }
    }

    //change the unit's mana, bool for if the unit has mana
    public bool changeMana(int amount)
    {
        currentMana -= amount;

        if (currentMana < 0)
        {
            return false;
        } else
        {
            return true;
        }
    }

    //change the unit's energy, bool for if the unit has energy
    public bool changeEnergy(int amount)
    {
        currentEnergy -= amount;

        if (currentEnergy < 0)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
