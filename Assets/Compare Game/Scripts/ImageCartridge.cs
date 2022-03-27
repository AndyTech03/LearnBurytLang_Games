using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class ImageCartridge : MonoBehaviour
    {
        [NonReorderable] public ImagePlate[] ImagePlates;

        [SerializeField] private GameObject[] Plate_Pos;

        public void Init(GameObject image_prefab, Material[] images, Material back)
        {
            int count = images.Length;
            if (count != Plate_Pos.Length)
                throw new System.Exception("Incorrect count!");

            ImagePlates = new ImagePlate[count];
            for (int i = 0; i < count; i++)
            {
                ImagePlates[i] = Instantiate(image_prefab, Plate_Pos[i].transform).GetComponent<ImagePlate>();
                ImagePlates[i].GetComponent<Renderer>().materials = new Material[] { back, images[i] };
            }
        }
    }
}