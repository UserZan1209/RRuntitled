using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotData : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI amount;

    public void SetData(string n, int a)
    {
        name.text = n;
        amount.text = a.ToString();
    }
}
