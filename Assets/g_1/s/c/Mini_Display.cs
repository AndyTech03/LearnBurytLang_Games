using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mini_Display : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    [SerializeField] private Material Hightlighted_Image;
    [SerializeField] private Material Default_Image;
    [SerializeField] private Material Border_Image;
    [SerializeField] private Material Base_Image;
    [SerializeField] private Renderer Self_Renderer;

    public bool Is_Hightlighted
    {
        set
        {
            if (value)
            {
                Self_Renderer.materials = new Material[] { Hightlighted_Image, Border_Image, Base_Image };
            }
            else
            {
                Self_Renderer.materials = new Material[] { Default_Image, Border_Image, Base_Image };
            }

        }
    }
    public void Init(string text)
    {
        Text.text = text;
        Is_Hightlighted = false;
    }
}
