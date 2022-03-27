using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject Text_Pos;
    [SerializeField] private GameObject Start_Pos;
    [SerializeField] private GameObject Pushed_Pos;
    [SerializeField] private TMP_Text Text;

    public void Init(string text)
    {
        Text.text = text;
    }
}
