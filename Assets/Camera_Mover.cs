using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Mover : MonoBehaviour
{
    [SerializeField] private ObjectMover CameraMover;
    [SerializeField] private GameObject Camera_Object;
    [SerializeField] private CompareGame.Compare_Game Game;
    [SerializeField] private CompareGame.Container Container;
    [SerializeField] private GameObject Camera_GamePos;
    [SerializeField] private GameObject Camera_LevelSelectPos;

    private bool IsJest;
    private Vector2 StartPos;
    private Vector2 EndPos;

    private void Start()
    {
        CameraMover.SetOnStart(Camera_Object);
        Container.Cartridge_Geted_Notification += delegate ()
        {
            CameraMover.MoveForvard();
        };

        Game.Cartridge_Geted_Notification += delegate ()
        {
            CameraMover.MoveBackvard();
        };
        IsJest = false;
    }

    public void Zoom_In()
    {
        Vector3 pos;
        if (Game.IsStarted == false || Game.Plate_Picked)
        {
            pos = Camera_LevelSelectPos.transform.localPosition;
            pos.y -= 4;
            Camera_LevelSelectPos.transform.localPosition = pos;
        }
        else
        {
            pos = Camera_GamePos.transform.localPosition;
            pos.y -= 4;
            Camera_GamePos.transform.localPosition = pos;
        }
    }
    public void Zoom_Out()
    {
        Vector3 pos;
        if (Game.IsStarted == false || Game.Plate_Picked)
        {
            pos = Camera_LevelSelectPos.transform.localPosition;
            pos.y += 4;
            Camera_LevelSelectPos.transform.localPosition = pos;
        }
        else
        {
            pos = Camera_GamePos.transform.localPosition;
            pos.y += 4;
            Camera_GamePos.transform.localPosition = pos;
        }
    }

    private void FixedUpdate()
    {
        if (Game.IsStarted == false || Game.Plate_Picked)
            return;

        if (Input.GetMouseButton(0))
        {
            if (IsJest)
            {
                EndPos = Input.mousePosition;
                Vector3 camera_pos = Camera_GamePos.transform.position;
                camera_pos.z += (EndPos.y - StartPos.y) / 20;
                Camera_GamePos.transform.position = camera_pos;
            }
            else
            {
                IsJest = true;
            }
            StartPos = Input.mousePosition;
        }
        else
        {
            IsJest = false;
        }
    }

}
