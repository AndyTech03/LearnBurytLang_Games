using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Container_Buttons : MonoBehaviour
{
    [SerializeField] private GameObject NextButton_Pos;
    [SerializeField] private GameObject BackButton_Pos;
    [SerializeField] private GameObject OKButton_Pos;

    public Action NextButtonClick { get => NextButton.ButtonClicked; set => NextButton.ButtonClicked = value; }
    public Action OKButtonClick { get => OKButton.ButtonClicked; set => OKButton.ButtonClicked = value; }
    public Action BackButtonClick { get => BackButton.ButtonClicked; set => BackButton.ButtonClicked = value; }

    private Button NextButton;
    private Button BackButton;
    private Button OKButton;

    public void Init(GameObject buttons_prefab, Material next_logo, Material back_logo, Material ok_logo)
    {
        NextButton = Instantiate(buttons_prefab, NextButton_Pos.transform).GetComponent<Button>();
        NextButton.Init(next_logo);

        BackButton = Instantiate(buttons_prefab, BackButton_Pos.transform).GetComponent<Button>();
        BackButton.Init(back_logo);

        OKButton = Instantiate(buttons_prefab, OKButton_Pos.transform).GetComponent<Button>();
        OKButton.Init(ok_logo);
    }
}
