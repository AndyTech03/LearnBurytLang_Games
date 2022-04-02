using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompareGame
{
    public class ImageSlot : MonoBehaviour
    {
        [SerializeField] private GameObject ImagePos;

        [SerializeField] private ImageLuke Luke;
        private ImagePlate CorrectImage;

        public void Set_CorrectImage(ImagePlate image)
        {
            CorrectImage = image;
        }
    }
}