using UnityEngine;
using UnityEngine.UI;

public class Settings_Menu : MonoBehaviour
{
    [SerializeField] private Slider ZoomSpeed_Slider;
    [SerializeField] private Slider ScrollSensitivity_Slider;

    [SerializeField] private Image Blur_Image;
    [SerializeField] private RectTransform Settings_Panel;
    [SerializeField] private AnimationCurve Move_Curve;
    [SerializeField] private AnimationCurve Blur_Curve;

    public System.Action Settings_Chainging;

    private float Animation_Time;
    private Color Blur_Color;
    private Color Transparent_Color;
    private Vector3 Panel_Opened_Pos;
    private Vector3 Panel_Closed_Pos;
    private Menu_State State;
    private Vector3 Delta_Pos;
    private Color Delta_Color;

    enum Menu_State
    {
        Closed,
        Opening,
        Opened,
        Closing,
    }

    private void Start()
    {
        State = Menu_State.Closed;
        Panel_Opened_Pos = Settings_Panel.position;
        Settings_Panel.position = Panel_Closed_Pos = new Vector3(Panel_Opened_Pos.x, Panel_Opened_Pos.y + Settings_Panel.rect.height, Panel_Opened_Pos.z);
        Blur_Color = Blur_Image.color;
        Blur_Image.color = Transparent_Color = new Color(Blur_Color.r, Blur_Color.g, Blur_Color.b, 0);
        Delta_Pos = Panel_Closed_Pos - Panel_Opened_Pos;
        Delta_Color = Transparent_Color - Blur_Color;
        Animation_Time = Move_Curve.keys[1].time;
    }

    public void FixedUpdate()
    {
        switch(State)
        {
            case Menu_State.Closed:
            case Menu_State.Opened:
                break;
            case Menu_State.Closing:
                if (Animation_Time >= Move_Curve.keys[1].time)
                {
                    Settings_Panel.position = Panel_Closed_Pos;
                    Blur_Image.color = Transparent_Color;
                    State = Menu_State.Closed;
                    Animation_Time = Move_Curve.keys[1].time;
                    break;
                }
                Settings_Panel.position = Panel_Opened_Pos + Delta_Pos * Move_Curve.Evaluate(Animation_Time);
                Blur_Image.color = Blur_Color + Delta_Color * Blur_Curve.Evaluate(Animation_Time);
                Animation_Time += Time.deltaTime;
                break;
            case Menu_State.Opening:
                if (Animation_Time <= Move_Curve.keys[0].time)
                {
                    Settings_Panel.position = Panel_Opened_Pos;
                    Blur_Image.color = Blur_Color;
                    State = Menu_State.Opened;
                    Animation_Time = Move_Curve.keys[0].time;
                    break;
                }
                Settings_Panel.position = Panel_Opened_Pos + Delta_Pos * Move_Curve.Evaluate(Animation_Time);
                Blur_Image.color = Blur_Color + Delta_Color * Blur_Curve.Evaluate(Animation_Time);
                Animation_Time -= Time.deltaTime;
                break;
        }
    }

    public void Toggle_Settings()
    {
        if (State == Menu_State.Closed)
        {
            DataManager.Settings_Data data = DataManager.Get_Settings_Data();
            ZoomSpeed_Slider.value = data.Zoom_Speed;
            ScrollSensitivity_Slider.value = data.Scroll_Sensitivity;
            State = Menu_State.Opening;
        }
        else
            State = Menu_State.Closing;
    }

    public void Save_Settings()
    {
        DataManager.Settings_Data data = new DataManager.Settings_Data();
        data.Zoom_Speed = (byte)ZoomSpeed_Slider.value;
        data.Scroll_Sensitivity = (byte)ScrollSensitivity_Slider.value;
        DataManager.Save_Settings_Data(data);

        Settings_Chainging?.Invoke();
        Toggle_Settings();
    }

    public void Cancel_Settings()
    {
        Toggle_Settings();
    }
}
