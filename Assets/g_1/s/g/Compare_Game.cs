using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    /// TODO: Connect with <see cref="Container"/>
    public class Compare_Game : MonoBehaviour
    {
        [SerializeField] private Word_By_Reels[] Words;
        [SerializeField] private GameObject Reel_Prefab;
        [SerializeField] private GameObject Сover_Prefab;

        [SerializeField] private ImageSlot[] Image_Slots;

        [SerializeField] private Dispenser_Slot Dispenser_Slot;

        [SerializeField] private ObjectMover CartridgeMover;
        private ImageCartridge Cartridge;

        public System.Action Cartridge_Geted_Notification;

        private void SetCartrige()
        {
            if (Cartridge != null)
                throw new System.Exception("Cartridge is seted!");

            Cartridge = CartridgeMover.Get_Object().GetComponent<ImageCartridge>();
            Start_Game();
        }

        public void GetCartrige()
        {
            if (Cartridge == null)
                throw new System.Exception("Cartridge is not seted!");

            Dispenser_Slot.Close();
            CartridgeMover.MoveBackvard_FromFinish(Cartridge.gameObject, true);
            Cartridge = null;
            Cartridge_Geted_Notification?.Invoke();
        }

        public void Start()
        {
            int count;
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].Init(Reel_Prefab, Сover_Prefab);
            }

            count = Image_Slots.Length;

            Dispenser_Slot.Init(count);
            CartridgeMover.EndReaching_Notification += SetCartrige;
            Cartridge = null;
        }

        private void Start_Game()
        {
            Debug.Log("Game Started");
            Dispenser_Slot.Open();
            Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[3]);
            Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[2]);
            Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[1]);
            Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[0]);
        }
    }
}