using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImagePlate : MonoBehaviour, IEqualityComparer
    {
        public System.Action<ImagePlate> Grab;
        public System.Action<ImagePlate> UnGrab;
        public ImageSlot CurentSlot;
        public string Text;

        [SerializeField] private BoxCollider Collider;

        private bool _grabed;


        public bool CanGrab { get => Collider.enabled; set => Collider.enabled = value; }

        private void Awake()
        {
            CanGrab = false;
            _grabed = false;
            CurentSlot = null;
        }

        public void Clear()
        {
            Grab = new System.Action<ImagePlate>(Grab);
            UnGrab = new System.Action<ImagePlate>(UnGrab);
        }    

        private void FixedUpdate()
        {
            if (_grabed)
            {
                if (Input.GetMouseButton(0) == false)
                {
                    _grabed = false;
                    UnGrab?.Invoke(this);
                }
            }
        }

        private void OnMouseDown()
        {
            if (CanGrab)
            {
                _grabed = true;
                Grab?.Invoke(this);
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