using UnityEngine;
using System;
using System.Text;

static class DataManager
{
    private const string LevelsProgress_Name = "Levels";
    private const string Settings_Name = "Settings";
    private const int Levels_Count = 7;

    public struct LevelProgress_Data
    {
        public bool Complited;
        public ushort Time;
    }

    public struct Settings_Data
    {
        public byte Zoom_Speed;
        public byte Scroll_Sensitivity;
        public byte Quality_Level;
    }

    private static Settings_Data Init_Settings_Data()
    {
        Settings_Data data = new Settings_Data();
        data.Scroll_Sensitivity = 20;
        data.Zoom_Speed = 4;
        data.Quality_Level = (byte)QualityLevel.Good;
        Write_Data(data);
        return data;
    }

    private static void Write_Data(Settings_Data data)
    {
        string result = "";
        result += (char)(data.Zoom_Speed + 1);
        result += (char)(data.Scroll_Sensitivity + 1);
        result += (char)(data.Quality_Level + 1);
        PlayerPrefs.SetString(Settings_Name, result);
    }

    public static Settings_Data Get_Settings_Data()
    {
        if (PlayerPrefs.HasKey(Settings_Name) == false)
        {
            return Init_Settings_Data();
        }

        Settings_Data result = new Settings_Data();
        string data = PlayerPrefs.GetString(Settings_Name);
        if (data.Length != 3)
            return Init_Settings_Data();

        result.Zoom_Speed = (byte)(data[0] - 1);
        result.Scroll_Sensitivity = (byte)(data[1] - 1);
        result.Quality_Level = (byte)(data[2] - 1);

        return result;
    }

    public static void Save_Settings_Data(Settings_Data data)
    {
        Write_Data(data);
    }

    private static LevelProgress_Data[] Init_LevelsProgress_Data()
    {
        LevelProgress_Data[] data = new LevelProgress_Data[Levels_Count];
        for (int i = 0; i < Levels_Count; i++)
        {
            data[i].Complited = false;
            data[i].Time = 0;
        }
        Write_Data(data);
        return data;
    }

    private static void Write_Data(LevelProgress_Data[] data)
    {
        string result = "";
        for (int i = 0; i < Levels_Count; i++)
        {
            result += (char)(data[i].Time+1);
        }
        PlayerPrefs.SetString(LevelsProgress_Name, result);
    }

    public static LevelProgress_Data[] Get_LevelsProgress_Data()
    {
        if (PlayerPrefs.HasKey(LevelsProgress_Name) == false)
        {
            return Init_LevelsProgress_Data();
        }

        LevelProgress_Data[] result = new LevelProgress_Data[Levels_Count];
        string data = PlayerPrefs.GetString(LevelsProgress_Name);
        if (data.Length != Levels_Count)
            return Init_LevelsProgress_Data();

        for (int i = 0; i < Levels_Count; i++)
        {
            result[i].Time = (ushort)(data[i]-1);
            result[i].Complited = result[i].Time != 0;
        }

        return result;
    }

    public static void Save_ComplitedLevelProgress_Data(int level_number, ushort time)
    {
        LevelProgress_Data[] data = Get_LevelsProgress_Data();
        data[level_number].Time = time;
        data[level_number].Complited = true;
        Write_Data(data);
    }
}