using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImageLuke : MonoBehaviour
    {
        private const float Angle = -15;

        private Quaternion Close_Andle;
        private Quaternion Open_Angle;

        public Luke_State State;

        public enum Luke_State
        {
            Opened,
            Closed
        }

        public void Start()
        {
            Close_Andle = Open_Angle = transform.rotation;
            Open_Angle.x += Angle;
            State = Luke_State.Closed;
        }


        public void Open()
        {
            if (State == Luke_State.Opened)
                throw new System.Exception("Is opend yet!");

            transform.rotation = Open_Angle;
            State = Luke_State.Opened;
        }

        public void Close()
        {
            if (State == Luke_State.Closed)
                throw new System.Exception("Is closed yet!");

            transform.rotation = Close_Andle;
            State = Luke_State.Closed;
        }
    }
}