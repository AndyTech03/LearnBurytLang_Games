using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    [SerializeField] private Material Back_Image;
    [SerializeField] private ObjectMover PushMover;
    [SerializeField] private GameObject MovingPart;
    public System.Action ButtonClicked;
    private bool _isPushed;
    private bool _repiat_mode;

    public void Init(Material logo_image, bool repiat_mode = true)
    {
        GetComponentInChildren<Renderer>().materials = new Material[] { logo_image, Back_Image, Back_Image };
        PushMover.SetOnStart(MovingPart);
        PushMover.EndReaching_Notification += OnPushed;
        _repiat_mode = repiat_mode;
        _isPushed = false;
    }

    private void FixedUpdate()
    {
        if (_isPushed && _repiat_mode)
            ButtonClicked?.Invoke();
    }

    private void OnMouseDown()
    {
        if (PushMover.ObjectIsMoving == false)
        {
            PushMover.StopMoving();
        }

        PushMover.MoveForvard(notyfy: true);
    }

    private void OnPushed()
    {
        _isPushed = true;
        ButtonClicked?.Invoke();
    }

    private void OnMouseExit()
    {
        OnMouseUp();
        _isPushed = false;
    }

    private void OnMouseUp()
    {
        if (PushMover.ObjectIsMoving)
        {
            PushMover.StopMoving();
        }
        PushMover.MoveBackvard();
        _isPushed = false;
    }
}
