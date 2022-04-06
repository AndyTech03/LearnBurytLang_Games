using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Dispenser_Slot : MonoBehaviour
    {
        [SerializeField] private ImageDispenser Dispenser;
        [SerializeField] private ObjectMover DispenserMover;

        public System.Action Closed;

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
            IsInited = false;
        }

        public void Init(int queue_size)
        {
            Dispenser.Init(queue_size);
            State = Dispenser_Slot_State.Closed;
            IsInited = true;

            DispenserMover.SetOnStart(Dispenser.gameObject);
            DispenserMover.EndReaching_Notification += delegate ()
            {
                State = Dispenser_Slot_State.Opend;
                Dispenser.IsDeployed = true;
            };
            DispenserMover.StartReaching_Notification += delegate ()
            {
                State = Dispenser_Slot_State.Closed;
                Closed?.Invoke();
                Dispenser.IsDeployed = false;
            };
            Dispenser.ImagesColected += delegate ()
            {
                State = Dispenser_Slot_State.Closing;
                DispenserMover.MoveBackvard(true);
            };
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
            DispenserMover.MoveForvard(true);
        }

        public void Close()
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            if (State == Dispenser_Slot_State.Closed)
                throw new System.Exception("Is Closed yet!");

            Dispenser.Close();
        }

        public void OnImage_Picked()
        {
            Dispenser.OnImage_Picked();
        }
    }
}