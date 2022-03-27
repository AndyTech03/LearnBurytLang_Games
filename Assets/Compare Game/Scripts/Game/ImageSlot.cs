using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompareGame
{
    public class ImageSlot : MonoBehaviour
    {
        [SerializeField] private GameObject ImagePos;
        [SerializeField] private GameObject Luke_Pos;

        private ImageLuke Luke;
        private ImagePlate CorrectImage;
        private bool IsInited;

        public void Awake()
        {
            IsInited = false;
        }

        public void Init(GameObject luke_prefab)
        {
            Luke = Instantiate(luke_prefab, Luke_Pos.transform).GetComponent<ImageLuke>();
            IsInited = true;
        }

        public void Set_CorrectImage(ImagePlate image)
        {
            CorrectImage = image;
        }
    }
}