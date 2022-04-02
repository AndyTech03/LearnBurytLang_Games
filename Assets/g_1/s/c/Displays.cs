using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Displays : MonoBehaviour
    {
        [SerializeField] private GameObject[] Displays_Pos;
        [SerializeField] private GameObject MediumDisplay_Pos;
        [SerializeField] private GameObject LargeDisplay_Pos;

        private Mini_Display[] MiniDisplays;
        private Medium_Display MediumDisplay;
        private Large_Display LargeDisplay;

        private int _curent_level;


        public void Init(GameObject display_prefab, GameObject medium_display_prefab, GameObject large_display_prefab, int levels_count)
        {
            MiniDisplays = new Mini_Display[Displays_Pos.Length];
            for (int i = 0; i < Displays_Pos.Length; i++)
            {
                MiniDisplays[i] = Instantiate(display_prefab, Displays_Pos[i].transform).GetComponent<Mini_Display>();
                if (i < levels_count)
                {
                    MiniDisplays[i].Init((i + 1).ToString());
                }
                else
                {
                    MiniDisplays[i].Init("");
                }
            }
            MediumDisplay = Instantiate(medium_display_prefab, MediumDisplay_Pos.transform).GetComponent<Medium_Display>();
            LargeDisplay = Instantiate(large_display_prefab, LargeDisplay_Pos.transform).GetComponent<Large_Display>();
            _curent_level = -1;
        }

        public void SetLevel(ImageCartridge cartridge)
        {
            if (_curent_level != -1)
            {
                MiniDisplays[_curent_level].Is_Hightlighted = false;
            }

            _curent_level = cartridge.Level_Number;
            MiniDisplays[_curent_level].Is_Hightlighted = true;
            LargeDisplay.SetText(cartridge.Level_Title + '\n' + cartridge.Level_Description);
        }
    }
}