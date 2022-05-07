using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private GameObject Button_Prefab;
        [SerializeField] private Material NextButton_Logo;
        [SerializeField] private Material BackButton_Logo;
        [SerializeField] private Material OKButton_Logo;
        [SerializeField] private Container_Buttons Buttons;
        [SerializeField] private GameObject Display_Prefab;
        [SerializeField] private GameObject MediumDisplay_Prefab;
        [SerializeField] private GameObject LargeDisplay_Prefab;
        [SerializeField] private Displays Displays;

        [SerializeField] private ImageCartridge[] Cartridges;

        [SerializeField] private GameObject Pos_0;
        [SerializeField] private GameObject Pos_1;
        [SerializeField] private GameObject Pos_2;

        [SerializeField] private ObjectMover LeftMover;
        [SerializeField] private ObjectMover RightMover;
        [SerializeField] private ObjectMover CenterFrontMover;
        [SerializeField] private ObjectMover CenterBackMover;

        [SerializeField] private ObjectMover OutputMover;

        public System.Action Cartridge_Geted_Notification;

        private bool IsBusy => RightMover.ObjectIsMoving || LeftMover.ObjectIsMoving || CenterBackMover.ObjectIsMoving || CenterFrontMover.ObjectIsMoving || Cartrige_Geted;
        private bool Cartrige_Geted;

        private void MoveCartrigesForward()
        {
            RightMover.MoveForvard_FromStart(Cartridges[0].gameObject, true); 
            CenterFrontMover.MoveBackvard_FromFinish(Cartridges[1].gameObject, true);
            CenterBackMover.MoveBackvard_FromFinish(Cartridges[2].gameObject, true);

            for (int i = 0; i < Cartridges.Length-1; i++)
            {
                ImageCartridge t = Cartridges[i];
                Cartridges[i] = Cartridges[i + 1];
                Cartridges[i + 1] = t;
            }

            Displays.SetLevel(Cartridges[0]);
        }

        private void MoveCartrigesBackward()
        {
            LeftMover.MoveBackvard_FromFinish(Cartridges[Cartridges.Length - 1].gameObject, true);
            CenterFrontMover.MoveForvard_FromStart(Cartridges[0].gameObject, true);
            CenterBackMover.MoveForvard_FromStart(Cartridges[1].gameObject, true);

            for (int i = Cartridges.Length - 1; i > 0; i--)
            {
                ImageCartridge t = Cartridges[i];
                Cartridges[i] = Cartridges[i - 1];
                Cartridges[i - 1] = t;
            }

            Displays.SetLevel(Cartridges[0]);
        }

        private void InitCartriges()
        {
            Cartrige_Geted = false;
            int count = Cartridges.Length;
            Buttons.Init(Button_Prefab, NextButton_Logo, BackButton_Logo, OKButton_Logo);
            Displays.Init(Display_Prefab, MediumDisplay_Prefab, LargeDisplay_Prefab, count);

            Cartridges[0].Level_Number = 0;
            Cartridges[0].transform.SetParent(Pos_0.transform);
            Cartridges[0].transform.localPosition = Vector3.zero;
            Cartridges[0].transform.localEulerAngles = Vector3.zero;

            Cartridges[1].Level_Number = 1;
            Cartridges[1].transform.SetParent(Pos_1.transform);
            Cartridges[1].transform.localPosition = Vector3.zero;
            Cartridges[1].transform.localEulerAngles = Vector3.zero;

            for (int i = 2; i < count; i++)
            {
                Cartridges[i].Level_Number = i;
                Cartridges[i].transform.SetParent(Pos_2.transform);
                Cartridges[i].transform.localPosition = Vector3.zero;
                Cartridges[i].transform.localEulerAngles = Vector3.zero;
            }

            Displays.SetLevel(Cartridges[0]);
        }

        public void Start()
        {
            InitCartriges();
            RightMover.EndReaching_Notification += delegate () { RightMover.Get_Object(); };
            LeftMover.StartReaching_Notification += delegate () { LeftMover.Get_Object(); };

            CenterFrontMover.StartReaching_Notification += delegate () { CenterFrontMover.Get_Object(); };
            CenterFrontMover.EndReaching_Notification += delegate () { CenterFrontMover.Get_Object(); };

            CenterBackMover.StartReaching_Notification += delegate () { CenterBackMover.Get_Object(); };
            CenterBackMover.EndReaching_Notification += delegate () { CenterBackMover.Get_Object(); };

            OutputMover.StartReaching_Notification += delegate ()
            {
                Cartrige_Geted = false;
                Displays.SetLevel(Cartridges[0]);
                OutputMover.Get_Object();
            };

            Buttons.OKButtonClick += delegate ()
            {
                if (Cartrige_Geted == false)
                {
                    OutputMover.MoveForvard_FromStart(Cartridges[0].gameObject, true);
                    Cartridge_Geted_Notification?.Invoke();
                    Cartrige_Geted = true;
                }
            };

            Buttons.NextButtonClick += delegate ()
            {
                if (IsBusy == false)
                    MoveCartrigesForward();
            };

            Buttons.BackButtonClick += delegate ()
            {
                if (IsBusy == false)
                    MoveCartrigesBackward();
            };
        }
    }
}
