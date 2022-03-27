using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePart_Manager : MonoBehaviour
{
    [SerializeField] Vector3 Correct_Pos;

    public bool IsCorrect_Pos => Correct_Pos.x == transform.position.x && Correct_Pos.z == transform.position.z;
    public System.Action<PuzzlePart_Manager> On_MouseDown;
    public System.Action<PuzzlePart_Manager> On_MouseUp;

    public void SaveCorrect_Pos()
    {
        Correct_Pos = transform.position;
    }

    private void OnMouseDown()
    {
        if (IsCorrect_Pos == false)
            On_MouseDown?.Invoke(this);
    }

    private void OnMouseUp()
    {
        On_MouseUp?.Invoke(this);
    }

}
