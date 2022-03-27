using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container_Buttons : MonoBehaviour
{
    [SerializeField] private GameObject NextButton_Pos;
    [SerializeField] private GameObject BackButton_Pos;
    [SerializeField] private GameObject OKButton_Pos;

    private Button NextButton;
    private Button BackButton;
    private Button OKButton;

    public void Init(GameObject buttons_prefab, string next_text, string back_text, string ok_text)
    {
        NextButton = Instantiate(buttons_prefab, NextButton_Pos.transform).GetComponent<Button>();
        NextButton.Init(next_text);
        BackButton = Instantiate(buttons_prefab, BackButton_Pos.transform).GetComponent<Button>();
        BackButton.Init(back_text);
        OKButton = Instantiate(buttons_prefab, OKButton_Pos.transform).GetComponent<Button>();
        OKButton.Init(ok_text);
    }
}
