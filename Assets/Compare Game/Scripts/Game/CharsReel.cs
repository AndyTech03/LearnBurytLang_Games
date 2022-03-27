using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CompareGame
{
    public class CharsReel : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] Fake_Chars_Before;
        [SerializeField] private TMP_Text Main_Char;
        [SerializeField] private TMP_Text[] Fake_Chars_After;

        private bool is_rotating = false;
        private char next_char = ' ';
        private Quaternion default_rotation;
        private Vector3 default_up;
        private char curent_char => Main_Char.text[0];
        private double rotation_speed = 0;
        private int curent_index;
        private int end_index;

        private int rotation_speed_modifer = 3;
        private int step = 3;


        private void Awake()
        {
            Set_Char(' ');
            default_rotation = transform.rotation;
            default_up = transform.up;
        }

        private void Set_Char(char c)
        {
            string chars = Buryat_Lang.GetNeighbors(c, Fake_Chars_Before.Length, Fake_Chars_After.Length, Is_Lower(c));

            for (int i = 0; i < Fake_Chars_Before.Length; i++)
            {
                Fake_Chars_Before[i].text = chars[i].ToString();
            }
            chars = chars.Substring(Fake_Chars_Before.Length);

            Main_Char.text = chars[0].ToString();
            chars = chars.Substring(1);

            for (int i = 0; i < Fake_Chars_After.Length; i++)
            {
                Fake_Chars_After[i].text = chars[i].ToString();
            }
        }

        private void Rotate()
        {
            if (rotation_speed != 0)
            {
                transform.Rotate(transform.right, (float)rotation_speed);
                double angle = Vector3.Angle(transform.up, default_up);
                int distanse = Mathf.Abs(curent_index - end_index);
                int curent_step = distanse > step ? step : distanse;
                if (angle >= 45 * curent_step)
                {
                    if (rotation_speed > 0)
                        curent_index -= curent_step;
                    else
                        curent_index += curent_step;
                    Set_Char(Buryat_Lang.GetByIndex(curent_index, Is_Lower(next_char)));
                    transform.rotation = default_rotation;
                }
            }

            if (curent_index == end_index)
            {
                transform.rotation = default_rotation;
                Set_Char(next_char);
                is_rotating = false;
            }
        }

        private bool Is_Lower(char c) => c == ' ' || char.IsLower(c);

        private void FixedUpdate()
        {
            if (is_rotating)
                Rotate();
        }

        public void RotateToChar(char c)
        {
            next_char = c;
            curent_index = Buryat_Lang.IndexOf(curent_char);
            end_index = Buryat_Lang.IndexOf(next_char);
            rotation_speed = 45 / rotation_speed_modifer * (curent_index > end_index ? 1 : -1);

            is_rotating = true;
        }
    }
}