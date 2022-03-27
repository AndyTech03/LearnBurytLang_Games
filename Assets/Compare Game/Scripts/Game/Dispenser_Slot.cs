using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Dispenser_Slot : MonoBehaviour
    {
        private ImageDispenser Dispenser;

        [SerializeField] private GameObject ClosePos;
        [SerializeField] private GameObject OpenPos;
        [SerializeField] private int MoveStep_Count;
        private int Curent_MoveSteps;
        [SerializeField] private Vector3 MoveStep;

        public Dispenser_Slot_State State;

        private bool IsInited;

        public enum Dispenser_Slot_State
        {
            Opend,
            Closed,
            Opening,
            Closing
        }

        public void Awake()
        {
            MoveStep = (OpenPos.transform.position - ClosePos.transform.position) / MoveStep_Count;
            Curent_MoveSteps = 0;
            IsInited = false;
        }

        public void Init(GameObject dispenser_prefab, int queue_size)
        {
            Dispenser = Instantiate(dispenser_prefab, ClosePos.transform.position, transform.rotation, transform).GetComponent<ImageDispenser>();
            Dispenser.Init(queue_size);
            State = Dispenser_Slot_State.Closed;
            IsInited = true;
        }

        private void FixedUpdate()
        {
            if (IsInited)
                switch (State)
                {
                    case Dispenser_Slot_State.Opening:
                        {
                            Dispenser.transform.Translate(MoveStep);
                            Curent_MoveSteps++;
                            if (Curent_MoveSteps == MoveStep_Count)
                            {
                                State = Dispenser_Slot_State.Opend;
                                Dispenser.transform.position = OpenPos.transform.position;
                                Dispenser.IsDeployed = true;
                                Curent_MoveSteps = 0;
                            }
                            break;
                        }
                    case Dispenser_Slot_State.Closing:
                        {
                            transform.Translate(-MoveStep);
                            Curent_MoveSteps++;
                            if (Curent_MoveSteps == MoveStep_Count)
                            {
                                State = Dispenser_Slot_State.Closed;
                                Dispenser.transform.position = ClosePos.transform.position;
                                Dispenser.IsDeployed = false;
                                Curent_MoveSteps = 0;
                            }
                            break;
                        }
                }
        }

        public void AddInQueue(ImagePlate image)
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            Dispenser.AddInQueue(image);
        }

        public void Open()
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            if (State == Dispenser_Slot_State.Opend)
                throw new System.Exception("Is Oppend yet!");

            State = Dispenser_Slot_State.Opening;
        }

        public void Close()
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            if (State == Dispenser_Slot_State.Closed)
                throw new System.Exception("Is Closed yet!");

            State = Dispenser_Slot_State.Closed;
        }

        public void OnImage_Picked()
        {
            Dispenser.OnImage_Picked();
        }
    }
}