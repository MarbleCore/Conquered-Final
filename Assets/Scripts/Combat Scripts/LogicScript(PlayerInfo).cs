using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicScriptPlayerInfo : MonoBehaviour
{
    //import game objects as TMPro
    public TextMeshProUGUI healthDisplayValue;
    public TextMeshProUGUI manaDisplayValue;
    public TextMeshProUGUI energyDisplayValue;

    //change the text based on current health
    public void changeHealthValue(int value)
    {
        healthDisplayValue.text = value.ToString() + "/" + "100";
    }

    //change the text based on current mana
    public void changeManaValue(int value)
    {
        manaDisplayValue.text = value.ToString() + "/" + "100 (+10)";
    }

    //change the text based on current energy
    public void changeEnergyValue(int value)
    {
        energyDisplayValue.text = value.ToString() + "/" + "10";
    }
}
