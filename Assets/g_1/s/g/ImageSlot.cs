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
        [SerializeField] private ObjectMover Luke_Mover;
        [SerializeField] private ObjectMover Plate_Mover;

        public bool IsSeted => CurentImage != null;
        public bool IsCorrect => IsSeted && CorrectImage.Equals(CurentImage);
        public System.Action ImagePlate_Collected;

        private ImagePlate CorrectImage;
        private ImagePlate CurentImage;

        private bool IsInited;

        public void Awake()
        {
            IsInited = false;
        }

        public void Init(GameObject reel_prefab, GameObject cover_prefab, System.Action on_image_collected)
        {
            Word.Init(reel_prefab, cover_prefab);
            Luke_Mover.SetOnStart(Luke.gameObject);
            Luke_Mover.EndReaching_Notification += MoveImage;
            Plate_Mover.EndReaching_Notification += CloseLuke;
            Luke_Mover.StartReaching_Notification += delegate () 
            {
                ImagePlate_Collected?.Invoke();
            };
            IsInited = true;
            ImagePlate_Collected += on_image_collected;
        }

        public void Clear()
        {
            CorrectImage = null;
            CurentImage = null;
            Word.Set_Word("");
        }

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

        public bool CollectImage()
        {
            if (IsInited == false)
                throw new System.Exception("Not inited!");

            if (CurentImage)
            {
                OpenLuke();
                return true;
            }
            Clear();
            return false;
        }

        private void OpenLuke()
        {
            CurentImage.transform.SetParent(Luke.transform);
            Luke_Mover.MoveForvard(true);
        }

        private void MoveImage()
        {
            Plate_Mover.MoveForvard_FromStart(CurentImage.gameObject, true);
        }

        private void CloseLuke()
        {
            Clear();
            Plate_Mover.Get_Object();
            Luke_Mover.MoveBackvard(true);
        }

    }
}