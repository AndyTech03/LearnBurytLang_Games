using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Compare_Game : MonoBehaviour
    {
        public bool IsStarted => Cartridge != null;
        public bool Plate_Picked => PickedPlate != null;
        private ImagePlate PickedPlate;
        private Camera _main_camera;
        [SerializeField] private float flying_height;

        [SerializeField] private Word_By_Reels[] Words;
        [SerializeField] private GameObject Reel_Prefab;
        [SerializeField] private GameObject Сover_Prefab;

        [SerializeField] private ImageSlot[] Image_Slots;

        [SerializeField] private Dispenser_Slot Dispenser_Slot;

        [SerializeField] private Button GetCartridge_Button;
        [SerializeField] private Material Button_Image;

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

            for (int i = 0; i < 4; i++)
            {
                Cartridge.ImagePlates[i].Grab -= OnImagePicked;
                Cartridge.ImagePlates[i].UnGrab -= OnImageUnpicked;
                Words[i].Set_Word("");
            }

            if (Dispenser_Slot.State == Dispenser_Slot.Dispenser_Slot_State.Opend)
                Dispenser_Slot.Close();
        }

        public void Start()
        {
            _main_camera = Camera.main;
            PickedPlate = null;
            int count;
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].Init(Reel_Prefab, Сover_Prefab);
            }

            count = Image_Slots.Length;

            Dispenser_Slot.Init(count);
            CartridgeMover.EndReaching_Notification += SetCartrige;
            Cartridge = null;
            GetCartridge_Button.ButtonClicked += delegate ()
            {
                if (Cartridge != null)
                    GetCartrige();
            };
            GetCartridge_Button.Init(Button_Image);

            Dispenser_Slot.Closed += delegate ()
            {
                Cartridge.CollectImages();
                CartridgeMover.MoveBackvard_FromFinish(Cartridge.gameObject, true);
                Cartridge = null;
                Cartridge_Geted_Notification?.Invoke();
            };
        }

        private void FixedUpdate()
        {
            if (PickedPlate != null)
            {
                Plane table = new Plane(Vector3.up, Vector3.zero);
                Ray ray_to_table = _main_camera.ScreenPointToRay(Input.mousePosition);

                if (table.Raycast(ray_to_table, out float position))
                {
                    Vector3 world_point = ray_to_table.GetPoint(position);
                    int x = Mathf.RoundToInt(world_point.x / 2) * 2;
                    float y = world_point.y;
                    int z = Mathf.RoundToInt((world_point.z) / 2) * 2;
                    PickedPlate.transform.position = new Vector3(x, y + flying_height, z);
                }
            }
        }

        private void OnImagePicked(ImagePlate image)
        {
            PickedPlate = image;
            PickedPlate.transform.SetParent(transform);
        }

        private void OnImageUnpicked(ImagePlate image)
        {
            PickedPlate = null;
            if (image.CurentSlot == null)
                PlaceInSlot(image);
            else
                ChaingeSlot(image);
        }

        private void ChaingeSlot(ImagePlate image)
        {
            ImageSlot nearest_slot = null;
            for (int i = 0; i < 4; i++)
            {
                ImageSlot curent_slot = Image_Slots[i];

                if (nearest_slot == null)
                    nearest_slot = curent_slot;
                else
                {
                    if (curent_slot.Get_Distance(image) < nearest_slot.Get_Distance(image))
                    {
                        nearest_slot = curent_slot;
                    }
                }
            }

            if (nearest_slot == image.CurentSlot)
            {
                nearest_slot.SetImage(image);
            }
            else
            {
                if (nearest_slot.IsSeted)
                {
                    image.CurentSlot.SetImage(nearest_slot.GetImage());
                    nearest_slot.SetImage(image);
                }
            }
        }

        private void PlaceInSlot(ImagePlate image)
        {
            ImageSlot nearest_slot = null;
            for (int i = 0; i < 4; i++)
            {
                ImageSlot curent_slot = Image_Slots[i];
                if (curent_slot.IsSeted == false)
                {
                    if (nearest_slot == null)
                        nearest_slot = curent_slot;
                    else
                    {
                        if (curent_slot.Get_Distance(image) < nearest_slot.Get_Distance(image))
                        {
                            nearest_slot = curent_slot;
                        }
                    }
                }
            }
            nearest_slot.SetImage(image);
        }

        private void Start_Game()
        {
            Debug.Log($"Game Started: Level - {Cartridge.Level_Number} {Cartridge.Level_Title}");
            for (int i = 3; i >=0; i--)
            {
                Cartridge.ImagePlates[i].Grab += OnImagePicked;
                Cartridge.ImagePlates[i].UnGrab += OnImageUnpicked;
                Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[i]);
            }
            List<int> index_list = new List<int>();
            index_list.AddRange(new int []{ 0, 1, 2, 3});
            for (int i = 0; i < 10; i++)
            {
                int i1 = Random.Range(0, 3);
                int i2 = Random.Range(0, 3);
                int temp = index_list[i1];
                index_list[i1] = index_list[i2];
                index_list[i2] = temp;
            }

            for (int i = 0; i < 4; i++)
            {
                Image_Slots[i].Set_CorrectImage(Cartridge.ImagePlates[index_list[i]]);
            }
            Dispenser_Slot.Open();
        }
    }
}