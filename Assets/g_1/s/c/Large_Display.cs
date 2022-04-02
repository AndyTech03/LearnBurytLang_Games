using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Large_Display : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    public void SetText(string text)
    {
        Text.text = text;
    }
}
