using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text Puzzle_B_Text;
    [SerializeField] private TMP_Text Combine_B_Text;
    [SerializeField] private TMP_Text FillBlanks_B_Text;
    [SerializeField] private TMP_Text Settings_B_Text;

    public void LoadGame(int number)
    {
        if (0 > number && number > SceneManager.sceneCount)
            throw new System.Exception("Wrong GameScene number!");
        SceneManager.LoadScene(number);
        SceneManager.UnloadSceneAsync(0);
    }
    public void Show_Settings()
    {

    }
}
