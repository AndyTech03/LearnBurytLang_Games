using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Compare_Game : MonoBehaviour
    {
        private const int CARTRIDGE_CAPACITY = 4;
        private const int MIXER_ITERATIONS = 10;


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
        [SerializeField] private Material Get_Image;

        [SerializeField] private Button Verify_Button;
        [SerializeField] private Material Verify_Image;

        /////
        
        [SerializeField] private ObjectMover CartridgeMover;
        private ushort _game_time;

        private ImageCartridge Cartridge;
        private int _collecting_cartridges_count;
        private bool _get_cartridge_after_collect;

        public System.Action Cartridge_Geted_Notification;

        private void SetCartrige()
        {
            if (Cartridge != null)
                throw new System.Exception("Cartridge is seted!");

            Cartridge = CartridgeMover.Get_Object().GetComponent<ImageCartridge>();
            Start_Game();
        }

        private void GetCartrige()
        {
            if (Cartridge == null)
                throw new System.Exception("Cartridge is not seted!");

            _get_cartridge_after_collect = true;
            Clear_ImageSlots();
        }

        private void ReturnCartridge()
        {
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
                Image_Slots[i].Clear();
            Cartridge.Undock();
            Dispenser_Slot.Close();
        }

        private void Clear_ImageSlots()
        {
            if (Cartridge == null)
                throw new System.Exception("Cartridge is not seted!");

            _collecting_cartridges_count = 0;
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
            {
                if (Image_Slots[i].CollectImage())
                {
                    _collecting_cartridges_count++;
                }
            }
            
            if (_collecting_cartridges_count == 0)
                if (_get_cartridge_after_collect)
                    ReturnCartridge();
                else
                    Invoke(nameof(Randomize_CorrectImage), .1f);

        }

        private void OnImage_Collected()
        {
            if (--_collecting_cartridges_count <= 0)
            {
                if (_get_cartridge_after_collect) 
                    ReturnCartridge();
                else
                {
                    for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
                    {
                        ImagePlate image = Cartridge.ImagePlates[i];
                        if (Dispenser_Slot.IsInQueue(image) == false)
                        {
                            Cartridge.CollectImage(i);
                            Dispenser_Slot.AddInQueue(image);
                        }
                    }
                    Invoke(nameof(Randomize_CorrectImage), .1f);
                }

            }
        }

        public void Start()
        {
            if (Words.Length != CARTRIDGE_CAPACITY || Image_Slots.Length != CARTRIDGE_CAPACITY)
                throw new System.Exception("Wrong count!");

            Cartridge = null;
            _main_camera = Camera.main;
            PickedPlate = null;

            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
            {
                Image_Slots[i].Init(Reel_Prefab, Сover_Prefab, OnImage_Collected);
            }


            Dispenser_Slot.Init(CARTRIDGE_CAPACITY);
            Dispenser_Slot.Closed += delegate ()
            {
                Cartridge.CollectImages();
                CartridgeMover.MoveBackvard_FromFinish(Cartridge.gameObject, true);
                Cartridge = null;
                Cartridge_Geted_Notification?.Invoke();
            };

            CartridgeMover.EndReaching_Notification += SetCartrige;

            GetCartridge_Button.Init(Get_Image, false);
            GetCartridge_Button.ButtonClicked += delegate ()
            {
                if (Cartridge != null)
                    GetCartrige();
            };

            Verify_Button.Init(Verify_Image, false);
            Verify_Button.ButtonClicked += delegate ()
            {
                if (Cartridge != null)
                    if (VerifyGame())
                    {
                        Debug.Log(_game_time);
                        DataManager.LevelProgress_Data data = DataManager.Get_LevelsProgress_Data()[Cartridge.Level_Number];
                        if (data.Complited == false || data.Time < _game_time )
                            DataManager.Save_ComplitedLevelProgress_Data(Cartridge.Level_Number, _game_time);
                        GetCartrige();
                    }
                    else
                    {
                        _get_cartridge_after_collect = false;
                        Clear_ImageSlots();
                    }
            };
        }

        private void Update()
        {
            if (Cartridge != null)
                _game_time += (ushort)(Time.deltaTime * 100);
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

        private bool VerifyGame()
        {
            bool all_is_correct = true;
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
            {
                if (Image_Slots[i].IsCorrect == false)
                {
                    all_is_correct = false;
                    break;
                }
            }

            if (all_is_correct)
            {
                Debug.Log("All is correct!");
                return true;
            }
            return false;
        }

        private void OnImagePicked(object obj, System.EventArgs _)
        {
            if (obj is ImagePlate image)
            {
                PickedPlate = image;
                PickedPlate.transform.SetParent(transform);
            }
        }

        private void OnImageUnpicked(object obj, System.EventArgs _)
        {
            if (obj is ImagePlate image)
            {
                PickedPlate = null;
                if (image.CurentSlot == null)
                    PlaceInSlot(image);
                else
                    ChaingeSlot(image);
            }
        }

        private void ChaingeSlot(ImagePlate image)
        {
            ImageSlot nearest_slot = null;
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
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
                else
                {
                    image.CurentSlot.GetImage();
                    nearest_slot.SetImage(image);
                }
            }
        }

        private void PlaceInSlot(ImagePlate image)
        {
            ImageSlot nearest_slot = null;
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
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

        /// <summary>
        /// Use sintax like this:
        /// <code>Invoke(nameof(Randomize_CorrectImage), .1f);</code>
        /// </summary>
        private void Randomize_CorrectImage()
        {
            List<int> index_list = new List<int>();
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
                index_list.Add(i);

            for (int i = 0; i < MIXER_ITERATIONS; i++)
            {
                int i1 = Random.Range(0, CARTRIDGE_CAPACITY);
                int i2 = Random.Range(0, CARTRIDGE_CAPACITY);
                int temp = index_list[i1];
                index_list[i1] = index_list[i2];
                index_list[i2] = temp;
            }
            for (int i = 0; i < CARTRIDGE_CAPACITY; i++)
            {
                Image_Slots[i].Set_CorrectImage(Cartridge.ImagePlates[index_list[i]]);
            }
        }

        private void Start_Game()
        {
            Debug.Log($"Game Started: Level - {Cartridge.Level_Number} {Cartridge.Level_Title}");
            _game_time = 0;
            for (int i = CARTRIDGE_CAPACITY - 1; i >=0; i--)
            {
                Cartridge.ImagePlates[i].Grab += OnImagePicked;
                Cartridge.ImagePlates[i].UnGrab += OnImageUnpicked;
                Dispenser_Slot.AddInQueue(Cartridge.ImagePlates[i]);
            }
            Invoke(nameof(Randomize_CorrectImage), .1f);
            Dispenser_Slot.Open();
        }
    }
}