using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    //public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public int maxMana;
    public int currentMana;

    public int maxEnergy;
    public int currentEnergy;

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
