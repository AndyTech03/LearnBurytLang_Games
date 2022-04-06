using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Buryat_Lang
{//Хүхэ
    static readonly char[] ABC = { ' ', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'Ө', 'П', 'Р', 'С', 'Т', 'У', 'Ү', 'Ф', 'Х', 'H', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э' ,'Ю', 'Я' };
    static readonly char[] abc = { ' ', 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'ө', 'п', 'р', 'с', 'т', 'у', 'ү', 'ф', 'х', 'h', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э' ,'ю', 'я' };

    private static string Get_Neighbors(int char_index, int count_left, int count_right, char[] arr)
    {
        string result = "";
        int len = arr.Length;

        for (int i = char_index - count_left; i < char_index + count_right + 1; i++)
        {
            int index = Normalize_Index(i, abc);
            result += arr[index];
        }

        return result;
    }

    private static int Index_Of(char c, char[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (c == arr[i])
                return i;
        }
        return -1;
    }

    public static int IndexOf(char c)
    {
        if (c == ' ')
            return 0;

        if (char.IsLower(c))
            return Index_Of(c, abc);
        else
            return Index_Of(c, ABC);
    }

    private static int Normalize_Index(int index, char[] arr)
    {
        if (index < 0)
            index += arr.Length;

        index %= arr.Length;
        return index;
    }

    public static char GetByIndex(int index, bool is_lower = false)
    {
        if (is_lower)
            return abc[Normalize_Index(index, abc)];
        else
            return ABC[Normalize_Index(index, ABC)];
    }
   
    public static string GetNeighbors(char c, int count_left, int count_right, bool is_lower = true)
    {
        int char_index = IndexOf(c);
        if (is_lower)
            return Get_Neighbors(char_index, count_left, count_right, abc);
        else
            return Get_Neighbors(char_index, count_left, count_right, ABC);
    }
}
