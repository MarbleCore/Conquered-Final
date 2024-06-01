using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicScriptPlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI healthDisplayValue;
    public TextMeshProUGUI manaDisplayValue;
    public TextMeshProUGUI energyDisplayValue;

    public void changeHealthValue(int value)
    {
        healthDisplayValue.text = value.ToString() + "/" + "100";
    }

    public void changeManaValue(int value)
    {
        manaDisplayValue.text = value.ToString() + "/" + "100 (+10)";
    }

    public void changeEnergyValue(int value)
    {
        energyDisplayValue.text = value.ToString() + "/" + "10";
    }
}
