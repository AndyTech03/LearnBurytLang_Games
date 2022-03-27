using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Compare_Game : MonoBehaviour
    {
        [SerializeField] private Word_By_Reels[] Words;
        [SerializeField] private GameObject Reel_Prefab;
        [SerializeField] private GameObject Сover_Prefab;

        [SerializeField] private Cartridge_Slot Cartridge_Slot;
        [SerializeField] private GameObject Cartrige_Prefab;
        [SerializeField] private GameObject ImagePlate_Prefab;
        [SerializeField] private Material Back;

        [SerializeField] private Material[] Images;

        [SerializeField] private ImageSlot[] Image_Slots;
        [SerializeField] private GameObject Luke_Prefab;

        [SerializeField] private Dispenser_Slot Dispenser_Slot;
        [SerializeField] private GameObject Dispenser_Prefab;



        public void Start()
        {
            int count;
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].Init(Reel_Prefab, Сover_Prefab);
            }

            count = Image_Slots.Length;

            for (int i = 0; i < count; i++)
            {
                Image_Slots[i].Init(Luke_Prefab);
            }

            Dispenser_Slot.Init(Dispenser_Prefab, count);
        }

        public void Start_Game()
        {
            Cartridge_Slot.SetCartrige(Cartrige_Prefab, ImagePlate_Prefab, Images, Back);
            for (int i = 0; i < Cartridge_Slot.ImagePlates.Length; i++)
            {
                Image_Slots[i].Set_CorrectImage(Cartridge_Slot.ImagePlates[i]);
                Dispenser_Slot.AddInQueue(Cartridge_Slot.ImagePlates[i]);
            }
            Dispenser_Slot.Open();

            string[] words = { "Хүхэ", "Ногөөн", "Улаан", "Шара" };
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].Set_Word(words[i]);
            }
        }
    }
}