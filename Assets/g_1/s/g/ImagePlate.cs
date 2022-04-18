using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CompareGame
{
    public class ImagePlate : MonoBehaviour, IEqualityComparer
    {
        private List<EventHandler> _grab_delegates;
        private event EventHandler _grab;
        private List<EventHandler> _ungrab_delegates;
        private event EventHandler _ungrab;
        public event EventHandler Grab
        {
            add
            {
                _grab += value;
                _grab_delegates.Add(value);
            }
            remove
            {
                _grab -= value;
                _grab_delegates.Remove(value);
            }
        }
        public event EventHandler UnGrab
        {
            add
            {
                _ungrab += value;
                _grab_delegates.Add(value);
            }
            remove
            {
                _ungrab -= value;
                _grab_delegates.Remove(value);
            }
        }

        public ImageSlot CurentSlot;
        public string Text;

        [SerializeField] private BoxCollider Collider;

        private bool _grabed;


        public bool CanGrab { get => Collider.enabled; set => Collider.enabled = value; }

        private void Awake()
        {
            _grab_delegates = new List<EventHandler>();
            _ungrab_delegates = new List<EventHandler>();
            CanGrab = false;
            _grabed = false;
            CurentSlot = null;
        }

        public void Clear()
        {
            foreach (EventHandler eh in _grab_delegates)
            {
                _grab -= eh;
            }
            _grab_delegates.Clear();

            foreach (EventHandler eh in _ungrab_delegates)
            {
                _ungrab -= eh;
            }
            _ungrab_delegates.Clear();
            CanGrab = false;
        }

        private void FixedUpdate()
        {
            if (_grabed)
            {
                if (Input.GetMouseButton(0) == false)
                {
                    _grabed = false;
                    _ungrab?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void OnMouseDown()
        {
            if (CanGrab)
            {
                _grabed = true;
                _grab?.Invoke(this, EventArgs.Empty);
            }
        }

        public new bool Equals(object x, object y)
        {
            if (x is ImagePlate p1 && y is ImagePlate p2)
                return p1.Text == p2.Text;

            throw new System.NotImplementedException();
        }

        public int GetHashCode(object obj)
        {
            if (obj is ImagePlate p)
                return p.Text.GetHashCode();

            throw new System.NotImplementedException();
        }
    }
}