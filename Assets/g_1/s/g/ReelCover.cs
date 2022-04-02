using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ReelCover : MonoBehaviour
    {
        [SerializeField] private Vector3 Open_Pos;
        [SerializeField] private Vector3 Close_Pos;
        [SerializeField] private Vector3 Move_Step;
        [SerializeField] private float Step_Count;
        private int _curent_step;

        public System.Action Opend;

        public enum Cover_State
        {
            Opened,
            Openning,
            Closed,
            Closing
        }

        public Cover_State State;

        public void Init(Vector3 open_pos, Vector3 close_pos)
        {
            Step_Count = 3;
            Open_Pos = open_pos;
            Close_Pos = close_pos;
            Move_Step = (close_pos - open_pos) / Step_Count;
            transform.position = close_pos;
            State = Cover_State.Closed;
        }

        public void FixedUpdate()
        {
            if (State == Cover_State.Closing)
            {
                transform.Translate(-Move_Step);
                _curent_step++;
                if (_curent_step == Step_Count)
                {
                    transform.position = Close_Pos;
                    State = Cover_State.Closed;
                }
            }

            if (State == Cover_State.Openning)
            {
                transform.Translate(Move_Step);
                _curent_step++;
                if (_curent_step == Step_Count)
                {
                    transform.position = Open_Pos;
                    Opend?.Invoke();
                    Opend = (System.Action)System.Action.RemoveAll(Opend, null);
                    State = Cover_State.Opened;
                }
            }
        }

        public void Open()
        {
            if (State == Cover_State.Opened || State == Cover_State.Openning)
                throw new System.Exception("Door is opend!");

            _curent_step = 0;
            State = Cover_State.Openning;
        }

        public void Close()
        {
            if (State == Cover_State.Closed || State == Cover_State.Closing)
                throw new System.Exception("Door is cloased!");

            _curent_step = 0;
            State = Cover_State.Closing;
        }
    }
}