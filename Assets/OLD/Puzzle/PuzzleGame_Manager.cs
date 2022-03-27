using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame_Manager : MonoBehaviour
{
    enum Puzzle_Type
    {
        Vertical,
        Horizontal,
        Rect
    }

    [SerializeField] GameObject Vertical_Puzzle;
    [SerializeField] GameObject Horizontal_Puzzle;
    [SerializeField] GameObject Rect_Puzzle;

    [SerializeField] GameObject Vertical_Table;
    [SerializeField] GameObject Horizontal_Table;
    [SerializeField] GameObject Rect_Table;

    [SerializeField] Material[] Puzzle_Back_Materials;

    [SerializeField] Material[] Vertical_Puzzles;
    [SerializeField] Material[] Horizontal_Puzzles;
    [SerializeField] Material[] Rect_Puzzles;

    [SerializeField] GameObject Stock_Place;
    [SerializeField] GameObject Stock_Start;
    [SerializeField] GameObject Game_Place;

    [SerializeField] TMPro.TMP_Text Congrad_Text;
    [SerializeField] GameObject Congrad_Panel;
    [SerializeField] TMPro.TMP_Text Timer_Text;
    [SerializeField] float GameTime;
    [SerializeField] bool IsGamePlaying = false;

    GameObject Curent_Puzzle;
    PuzzlePart_Manager Curent_Part;
    const int flying_height = 1;

    [SerializeField] int FatFinger_Scale = 2;

    public List<PuzzlePart_Manager> Stock_Cells;

    Camera mainCamera;

    public void Start()
    {
        BeginGame();
        mainCamera = Camera.main;
    }

    public void FixedUpdate()
    {
        if (Curent_Part)
        {
            Plane table = new Plane(Vector3.up, Vector3.zero);
            Ray ray_to_table = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (table.Raycast(ray_to_table, out float position))
            {
                Vector3 world_point = ray_to_table.GetPoint(position);
                int x = Mathf.RoundToInt(world_point.x / 2) * 2;
                float y = world_point.y;
                int z = Mathf.RoundToInt((world_point.z - FatFinger_Scale) / 2) * 2;
                Curent_Part.transform.position = new Vector3(x, y + flying_height, z);
            }
        }
    }

    string GetFormatedTime(float time)
    {
        int seconds = (int)GameTime % 60;
        int minutes = (int)GameTime / 60;
        int hours = minutes / 60;
        minutes %= 60;
        return $"{(hours > 0 ? $"{hours}:" : "")}{(minutes > 0 || hours > 0 ? $"{minutes}:" : "")}{seconds}";
    }

    public void Update()
    {
        if (IsGamePlaying)
        {
            GameTime += Time.deltaTime;
            Timer_Text.text = GetFormatedTime(GameTime);
        }
    }

    public void NewGame()
    {
        if (Curent_Puzzle)
        {
            Destroy(Curent_Puzzle);
        }
        if (Game_Place)
            Destroy(Game_Place);
        BeginGame();
    }

    Puzzle_Type Get_RandomType() => (Puzzle_Type)Random.Range(0, 3);

    Material Get_Random_VPuzzle() => Vertical_Puzzles[Random.Range(0, Vertical_Puzzles.Length)];
    Material Get_Random_HPuzzle() => Horizontal_Puzzles[Random.Range(0, Horizontal_Puzzles.Length)];
    Material Get_Random_RPuzzle() => Rect_Puzzles[Random.Range(0, Rect_Puzzles.Length)];

    void PaintPuzzle(GameObject puzzle, Material back, Material front)
    {
        foreach (Transform part in puzzle.transform)
        {
            part.GetComponent<Renderer>().materials = new Material[] { back, front };
            PuzzlePart_Manager manager = part.GetComponent<PuzzlePart_Manager>();
            manager.SaveCorrect_Pos();
            manager.On_MouseDown += Select_Puzzle_Part;
            manager.On_MouseUp += Deselect_Puzzle_Part;
            MoveToStock(manager);
        }
    }

    void BeginGame()
    {
        IsGamePlaying = true;
        GameTime = 0;
        Congrad_Panel.SetActive(false);
        Stock_Cells = new List<PuzzlePart_Manager>();
        Puzzle_Type type = Get_RandomType();
        Material back_image = Puzzle_Back_Materials[Random.Range(0, Puzzle_Back_Materials.Length)];
        Material puzle_image;
        GameObject puzzle_prefab;
        GameObject table_prefab;
        switch (type)
        {
            case Puzzle_Type.Vertical:
                puzle_image = Get_Random_VPuzzle();
                puzzle_prefab = Vertical_Puzzle;
                table_prefab = Vertical_Table;
                break;
            case Puzzle_Type.Horizontal:
                puzle_image = Get_Random_HPuzzle();
                puzzle_prefab = Horizontal_Puzzle;
                table_prefab = Horizontal_Table;
                break;
            case Puzzle_Type.Rect:
            default:
                puzle_image = Get_Random_RPuzzle();
                puzzle_prefab = Rect_Puzzle;
                table_prefab = Rect_Table;
                break;
        }

        Game_Place = Instantiate(table_prefab);
        Game_Place.GetComponent<Renderer>().materials = new Material[] { puzle_image, back_image };

        Curent_Puzzle = Instantiate(puzzle_prefab, Game_Place.transform.position, Game_Place.transform.rotation);
        PaintPuzzle(Curent_Puzzle, back_image, puzle_image);
    }

    void MoveFrom_Stock(PuzzlePart_Manager part)
    {
        Stock_Cells.Remove(part);
        RefreshStock();
    }

    void BackToStock(PuzzlePart_Manager part)
    {
        Stock_Cells.Insert(0, part);
        RefreshStock();
    }

    void MoveToStock(PuzzlePart_Manager part)
    {
        if (Random.Range(0, 2) == 0)
        {
            Stock_Cells.Insert(0, part);
        }
        else
        {
            Stock_Cells.Add(part);
        }
        RefreshStock();
    }

    void RefreshStock()
    {
        Vector3 start_pos = Stock_Start.transform.position;
        for (int i = 0; i < Stock_Cells.Count; i++)
        {
            Stock_Cells[i].transform.position = new Vector3(start_pos.x - 3 * i, start_pos.y, start_pos.z);
        }
    }

    void Select_Puzzle_Part(PuzzlePart_Manager part)
    {
        Curent_Part = part;
        MoveFrom_Stock(part);
    }

    void Deselect_Puzzle_Part(PuzzlePart_Manager part)
    {
        Vector3 pos = Curent_Part.transform.position;
        Curent_Part.transform.position = new Vector3(pos.x, pos.y - flying_height, pos.z);
        if (Curent_Part.IsCorrect_Pos == false)
            BackToStock(Curent_Part);
        Curent_Part = null;
        if (Stock_Cells.Count == 0)
        {
            IsGamePlaying = false;
            Congrad_Panel.SetActive(true);
            Congrad_Text.text = $"Вы решили пазл за {GetFormatedTime(GameTime)}!";
        }
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
    }
}
