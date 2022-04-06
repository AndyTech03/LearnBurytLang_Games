using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompareGame
{
    public class ImageSlot : MonoBehaviour
    {
        [SerializeField] private GameObject ImagePos;
        [SerializeField] private ImageLuke Luke;
        [SerializeField] private Word_By_Reels Word;
        public bool IsSeted => CurentImage != null;
        public bool IsCorrect => IsSeted && CorrectImage.Equals(CurentImage);

        private ImagePlate CorrectImage;
        private ImagePlate CurentImage;

        public void Set_CorrectImage(ImagePlate image)
        {
            Word.Set_Word(image.Text);
            CorrectImage = image;
        }

        public float Get_Distance(ImagePlate plate)
        {
            return (ImagePos.transform.position - plate.transform.position).magnitude;
        }

        public void SetImage(ImagePlate image)
        {
            CurentImage = image;
            image.CurentSlot = this;
            image.transform.SetParent(ImagePos.transform);
            image.transform.localPosition = Vector3.zero;
        }

        public ImagePlate GetImage()
        {
            if (IsSeted == false)
                throw new System.Exception("Image not seted!");

            ImagePlate p = CurentImage;
            CurentImage = null;
            return p;
        }
    }
}