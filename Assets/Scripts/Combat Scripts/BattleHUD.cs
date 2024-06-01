using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    //get the name of the unit spawned
    public TextMeshProUGUI nameText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
    }
}
