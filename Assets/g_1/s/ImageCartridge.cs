using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImageCartridge : MonoBehaviour
    {
        [NonReorderable] public int Level_Number;
        [SerializeField] private string LevelTitle;
        public string Level_Title => LevelTitle;
        [SerializeField] private string LevelDescription;
        public string Level_Description => LevelDescription;
        [SerializeField] private Material Logo_Image;
        [SerializeField] private Material[] Images;
        [SerializeField] private Material[] BackImages;
        [SerializeField] private GameObject ImagePlate_Prefab;

        [SerializeField] private GameObject[] Plate_Pos;

        public ImagePlate[] ImagePlates => imagePlates;
        private ImagePlate[] imagePlates;

        public void Start()
        {
            int count = Images.Length;
            if (count != Plate_Pos.Length)
                throw new System.Exception("Incorrect count!");
            Material[] materials = GetComponentInChildren<Renderer>().materials;
            GetComponentInChildren<Renderer>().materials = new Material[] { materials[0], materials[1], materials[2], Logo_Image };

            imagePlates = new ImagePlate[count];
            for (int i = 0; i < count; i++)
            {
                imagePlates[i] = Instantiate(ImagePlate_Prefab, Plate_Pos[i].transform).GetComponent<ImagePlate>();
                imagePlates[i].GetComponentInChildren<Renderer>().materials = new Material[] { Images[i], BackImages[i], BackImages[i] };
            }
        }
    }
}