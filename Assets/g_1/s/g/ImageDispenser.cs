using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImageDispenser : MonoBehaviour
    {
        [SerializeField] private ObjectMover ImageMover;

        private ImagePlate CurentImage;

        public Dispenser_State State;
        public System.Action ImagesColected;
        private Dispenser_Queue Queue;


        private bool IsInited;
        public bool IsDeployed;

        public enum Dispenser_State
        {
            Empty,
            Dispensing,
            Idle
        };

        class Dispenser_Queue
        {
            private ImagePlate[] Plates;
            private int count;
            private int max_count;

            public int Count => count;
            public int Size => max_count;

            public Dispenser_Queue(int size)
            {
                max_count = size;
                Clear();
            }

            public ImagePlate Get()
            {
                if (count == 0)
                    throw new System.Exception("Queue is empty!");
                ImagePlate geting = Plates[0];
                for (int i = 1; i < count; i++)
                {
                    Plates[i - 1] = Plates[i];
                }
                count--;
                return geting;
            }

            public void Clear()
            {
                count = 0;
                Plates = new ImagePlate[max_count];
            }

            public void Add(ImagePlate plate)
            {
                if (count == max_count)
                    throw new System.Exception("Queue is full!");

                Plates[count++] = plate;
            }
        }

        public void Awake()
        {
            IsDeployed = false;
            State = Dispenser_State.Empty;
            IsInited = false;
        }

        public void Init(int images_count)
        {
            Queue = new Dispenser_Queue(images_count);
            IsInited = true;
            ImageMover.EndReaching_Notification += delegate ()
            {
                State = Dispenser_State.Idle;
                CurentImage.CanGrab = true;
            };
            ImageMover.StartReaching_Notification += delegate ()
            {
                State = Dispenser_State.Empty;
                ImageMover.Get_Object();
                CollectImages();

            };
        }

        public void FixedUpdate()
        {
            if (IsDeployed)
            {
                if (IsInited == false)
                    throw new System.Exception("Not Inited!");
                switch (State)
                {
                    case Dispenser_State.Empty:
                        {
                            TryDispensImage();
                            break;
                        }
                }
            }

        }

        public void Close()
        {
            Queue.Clear();
            if (CurentImage == null)
            {
                State = Dispenser_State.Empty;
                CollectImages();
            }
            else
                ImageMover.MoveBackvard(true);
        }

        public void CollectImages()
        {
            ImagesColected?.Invoke();
        }

        public void AddInQueue(ImagePlate image)
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            Queue.Add(image);
        }

        public void OnImage_Picked()
        {
            if (IsInited == false)
                throw new System.Exception("Not Inited!");

            if (State != Dispenser_State.Idle)
                throw new System.Exception("Dispenser is not Idle!");

            State = Dispenser_State.Empty;
            ImageMover.Get_Object();
            CurentImage.Grab -= OnGrab;
             CurentImage = null;
        }

        private void OnGrab(ImagePlate image)
        {
            if (State == Dispenser_State.Idle)
            {
                OnImage_Picked();
            }
        }

        private void TryDispensImage()
        {
            if (Queue.Count == 0)
                return;

            CurentImage = Queue.Get();
            CurentImage.Grab += OnGrab;

            CurentImage.CanGrab = false;
            ImageMover.MoveForvard_FromStart(CurentImage.gameObject, true);
            State = Dispenser_State.Dispensing;
        }
    }
}