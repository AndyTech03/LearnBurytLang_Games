using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mini_Display : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    public void Init(string text)
    {
        Text.text = text;
    }
}
