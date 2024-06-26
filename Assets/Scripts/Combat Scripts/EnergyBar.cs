using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public LogicScriptPlayerInfo logic;

    public void SetMaxEnergy(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetEnergy(int energy)
    {
        slider.value = energy;
        logic.changeEnergyValue(energy);

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
