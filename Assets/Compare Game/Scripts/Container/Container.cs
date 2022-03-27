using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private GameObject Button_Prefab;
    [SerializeField] private string NextButton_Text;
    [SerializeField] private string BackButton_Text;
    [SerializeField] private string OKButton_Text;
    [SerializeField] private Container_Buttons Buttons;
    [SerializeField] private GameObject Display_Prefab;
    [SerializeField] private GameObject MediumDisplay_Prefab;
    [SerializeField] private GameObject LargeDisplay_Prefab;
    [SerializeField] private Displays Displays;

    public void Start()
    {
        Buttons.Init(Button_Prefab, NextButton_Text, BackButton_Text, OKButton_Text);
        Displays.Init(Display_Prefab, MediumDisplay_Prefab, LargeDisplay_Prefab);
    }
}
