using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displays : MonoBehaviour
{
    [SerializeField] private GameObject[] Displays_Pos;
    [SerializeField] private GameObject MediumDisplay_Pos;
    [SerializeField] private GameObject LargeDisplay_Pos;

    private Mini_Display[] MiniDisplays;
    private Medium_Display MediumDisplay;
    private Large_Display LargeDisplay;


    public void Init(GameObject display_prefab, GameObject medium_display_prefab, GameObject large_display_prefab)
    {
        MiniDisplays = new Mini_Display[Displays_Pos.Length];
        for (int i = 0; i < Displays_Pos.Length; i++)
        {
            MiniDisplays[i] = Instantiate(display_prefab, Displays_Pos[i].transform).GetComponent<Mini_Display>();
            MiniDisplays[i].Init(i.ToString());
        }
        MediumDisplay = Instantiate(medium_display_prefab, MediumDisplay_Pos.transform).GetComponent<Medium_Display>();
        MediumDisplay.Init("”ровень пока\nне пройден");
        LargeDisplay = Instantiate(large_display_prefab, LargeDisplay_Pos.transform).GetComponent<Large_Display>();
        LargeDisplay.Init("”ровень 1\n\t\"÷вета\"");
    }
}
