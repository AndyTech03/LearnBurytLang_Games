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
        if (Game.IsStarted == false || Game.Plate_Picked)
        {
            Camera_LevelSelectPos.transform.localPosition += Camera_Object.transform.forward * 4;
        }
        else
        {
            Camera_GamePos.transform.localPosition += Camera_Object.transform.forward * 4;
        }
    }
    public void Zoom_Out()
    {
        if (Game.IsStarted == false || Game.Plate_Picked)
        {
            Camera_LevelSelectPos.transform.localPosition += Camera_Object.transform.forward * -4;
        }
        else
        {
            Camera_GamePos.transform.localPosition += Camera_Object.transform.forward * -4;
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
