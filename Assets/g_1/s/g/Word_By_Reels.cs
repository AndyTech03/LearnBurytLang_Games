using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareGame
{
    public class Word_By_Reels : MonoBehaviour
    {
        [SerializeField] private GameObject[] Сover_Points;
        [SerializeField] private GameObject[] Reel_Points;

        private CharsReel[] Reels;
        private ReelCover[] Covers;

        private bool IsInited;

        private void Awake()
        {
            IsInited = false;
        }

        public void Init(GameObject reel_prefab, GameObject cover_prefab)
        {
            Init_Reels(reel_prefab, cover_prefab);
            IsInited = true;
        }

        public void Set_Word(string word)
        {
            if (IsInited == false)
                throw new Exception($"Not Inited! {gameObject.name}");

            for (int i = 0; i < Reels.Length; i++)
            {
                char c = ' ';
                if (i < word.Length)
                    c = word[i];

                Rotate_Reel(Reels[i], Covers[i], c);
            }
        }

        private void Rotate_Reel(CharsReel reel, ReelCover cover, char c)
        {
            if (cover.State == ReelCover.Cover_State.Closed)
            {
                if (c != ' ')
                {
                    cover.Open();
                    System.Action on_Opend = delegate ()
                    {
                        reel.transform.position = new Vector3(reel.transform.position.x, .3f, reel.transform.position.z);
                        reel.RotateToChar(c);
                    };
                    cover.Opend += on_Opend;
                }
            }
            else if (cover.State == ReelCover.Cover_State.Opened)
            {
                if (c == ' ')
                {
                    cover.Close();
                    reel.transform.position = new Vector3(reel.transform.position.x, 0, reel.transform.position.z);
                }
                reel.RotateToChar(c);
            }
        }

        private void Init_Reels(GameObject reel_prefab, GameObject cover_prefab)
        {
            int cells_count = Reel_Points.Length;
            if (cells_count != Сover_Points.Length)
                throw new Exception("Incorect count!");

            Reels = new CharsReel[cells_count];
            Covers = new ReelCover[cells_count];

            for (int i = 0; i < cells_count; i++)
            {
                Reels[i] = Instantiate(reel_prefab, Reel_Points[i].transform).GetComponent<CharsReel>();

                ReelCover cover = Instantiate(cover_prefab, Сover_Points[i].transform).GetComponent<ReelCover>();
                Vector3 open_pos = Сover_Points[i].transform.position;
                Vector3 close_pos = Сover_Points[i].transform.position;
                close_pos.z = Reel_Points[i].transform.position.z;
                cover.Init(open_pos, close_pos);
                Covers[i] = cover;
            }
        }
    }
}