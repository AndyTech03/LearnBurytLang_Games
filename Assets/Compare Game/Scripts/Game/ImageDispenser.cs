using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImageDispenser : MonoBehaviour
    {
        [SerializeField] private GameObject Image_StartPos;
        [SerializeField] private GameObject Image_EndPos;
        [SerializeField] private int Image_MoveStep_Count;
        private int Image_MoveSteps;
        private Vector3 Image_MoveStep;

        private ImagePlate CurentImage;

        public Dispenser_State State;
        private Dispenser_Queue Queue;


        private bool IsInited;
        public bool IsDeployed;

        public enum Dispenser_State
        {
            Empty,
            Dispensing,
            Idle
        };

        struct Dispenser_Queue
        {
            private ImagePlate[] Plates;
            private int count;
            private int max_count;

            public int Count => count;
            public int Size => max_count;

            public Dispenser_Queue(int size)
            {
                Plates = new ImagePlate[size];
                count = 0;
                max_count = size;
            }

            public ImagePlate Get()
            {
                if (count == 0)
                    throw new System.Exception("Queue is empty!");

                return Plates[count--];
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
            Image_MoveStep = (Image_StartPos.transform.position - Image_EndPos.transform.position) / Image_MoveStep_Count;
            Image_MoveSteps = 0;
            IsInited = false;
        }

        public void Init(int images_count)
        {
            Queue = new Dispenser_Queue(images_count);
            IsInited = true;
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
                    case Dispenser_State.Dispensing:
                        {
                            CurentImage.transform.Translate(Image_MoveStep);
                            Image_MoveSteps++;
                            if (Image_MoveSteps == Image_MoveStep_Count)
                            {
                                CurentImage.transform.position = Image_EndPos.transform.position;
                                State = Dispenser_State.Idle;
                                Image_MoveSteps = 0;
                            }
                            break;
                        }
                }

            }

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
        }

        private void TryDispensImage()
        {
            if (Queue.Count == 0)
                return;

            CurentImage = Queue.Get();
            CurentImage.transform.position = Image_StartPos.transform.position;
            State = Dispenser_State.Dispensing;
        }
    }
}