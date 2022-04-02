using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Mover : MonoBehaviour
{
    [SerializeField] private ObjectMover CameraMover;
    [SerializeField] private GameObject Camera_Object;
    [SerializeField] private CompareGame.Compare_Game Game;
    [SerializeField] private CompareGame.Container Container;

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
    }

}
