using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Cartridge_Slot : MonoBehaviour
    {
        private ImageCartridge Cartridge;
        public ImagePlate[] ImagePlates => Cartridge.ImagePlates;

        [SerializeField] private GameObject StartPos;
        [SerializeField] private GameObject EndPos;
        [SerializeField] private int MoveStep_Count;
        private int MoveSteps;
        private Vector3 MoveStep;

        private Cartridge_Slot_State State;

        private bool IsSeted;

        public enum Cartridge_Slot_State
        {
            Extracting,
            Empty,
            Insertiïg,
            Seted
        }


        public void Awake()
        {
            MoveStep = (EndPos.transform.position - StartPos.transform.position) / MoveStep_Count;
            MoveSteps = 0;
            State = Cartridge_Slot_State.Empty;
            IsSeted = false;
        }

        public void SetCartrige(GameObject cartrige_prefab, GameObject image_prefab, Material[] images, Material back)
        {
            Cartridge = Instantiate(cartrige_prefab, StartPos.transform.position, StartPos.transform.rotation, transform).GetComponent<ImageCartridge>();
            Cartridge.Init(image_prefab, images, back);
            State = Cartridge_Slot_State.Insertiïg;
            IsSeted = true;
        }

        public void FixedUpdate()
        {
            if (IsSeted)
            {
                switch (State)
                {
                    case Cartridge_Slot_State.Insertiïg:
                        {
                            Cartridge.transform.Translate(MoveStep);
                            MoveSteps++;
                            if (MoveSteps == MoveStep_Count)
                            {
                                Cartridge.transform.position = EndPos.transform.position;
                                State = Cartridge_Slot_State.Seted;
                                MoveSteps = 0;
                            }
                            break;
                        }
                }
            }
        }
    }
}