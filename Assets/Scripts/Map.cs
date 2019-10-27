using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;

    public Sprite TileSprite;
    public int width;
    public int height;
    public GameObject TileObject;
    public Tiles[,] TileMap;
    public string selectmode;

    public Unit SelectedUnit;
    public GameObject Selected_Pointer;

    public GameObject TestHero;
    public GameObject TestEnemy;

    public Transform UnitParent;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Use this for initialization
    void Start()
    {
        Init();

        TestSpawn(TestHero);

        //TestSpawn(TestHero);
        //TestSpawn(TestHero);
        //Debug.Log(Tiles.GetDistancePoint(TileMap[0, 0], TileMap[9, 9]));
    }

    public void TestSpawn(GameObject obj)
    {
        GameObject go = Instantiate(obj, UnitParent);
        Tiles spawntile = TileMap[UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height)];
        while (spawntile.somethingOn)
            spawntile = TileMap[UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height)];
        go.transform.localPosition = spawntile.transform.position;
        go.GetComponent<Unit>().groundtile = spawntile;
        spawntile.somethingOn = true;
        spawntile.unit = go.GetComponent<Unit>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckTouch();
        PointerMove();
    }

    private void PointerMove()
    {
        if (SelectedUnit != null)
            if (Selected_Pointer.activeInHierarchy)
                Selected_Pointer.transform.localPosition = SelectedUnit.transform.localPosition;
    }

    private void Init()
    {
        TileMap = new Tiles[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                GameObject GO = Instantiate(TileObject, new Vector3((k % 2 == 0) ? i + 0.5f : i, -k * 0.86f), new Quaternion(), gameObject.transform);
                TileMap[i, k] = GO.GetComponent<Tiles>();
                TileMap[i, k].pos_x = i;
                TileMap[i, k].pos_y = k;
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                if (k % 2 == 0)
                {

                    TileMap[i, k].ArroundTiles[0] = TileMap[(i + 1 < width) ? i + 1 : i, (k > 0) ? k - 1 : k];
                    TileMap[i, k].ArroundTiles[2] = TileMap[(i + 1 < width) ? i + 1 : i, (k + 1 < height) ? k + 1 : k];
                    TileMap[i, k].ArroundTiles[3] = TileMap[i, (k + 1 < height) ? k + 1 : k];
                    TileMap[i, k].ArroundTiles[5] = TileMap[i, (k > 0) ? k - 1 : k];
                }
                else
                {
                    TileMap[i, k].ArroundTiles[0] = TileMap[i, (k > 0) ? k - 1 : k];
                    TileMap[i, k].ArroundTiles[2] = TileMap[i, (k + 1 < height) ? k + 1 : k];
                    TileMap[i, k].ArroundTiles[3] = TileMap[(i > 0) ? i - 1 : i, (k + 1 < height) ? k + 1 : k];
                    TileMap[i, k].ArroundTiles[5] = TileMap[(i > 0) ? i - 1 : i, (k > 0) ? k - 1 : k];
                }
                TileMap[i, k].ArroundTiles[1] = TileMap[(i + 1 < width) ? i + 1 : i, k];
                TileMap[i, k].ArroundTiles[4] = TileMap[(i > 0) ? i - 1 : i, k];

                if (i == 0)
                {
                    TileMap[i, k].ArroundTiles[4] = TileMap[i, k];
                    if (k % 2 == 1)
                    {
                        TileMap[i, k].ArroundTiles[5] = TileMap[i, k];
                        TileMap[i, k].ArroundTiles[3] = TileMap[i, k];
                    }
                }
                if (i == width - 1)
                {
                    TileMap[i, k].ArroundTiles[1] = TileMap[i, k];
                    if (k % 2 == 0)
                    {
                        TileMap[i, k].ArroundTiles[0] = TileMap[i, k];
                        TileMap[i, k].ArroundTiles[2] = TileMap[i, k];
                    }
                }
                if (k == 0)
                {
                    TileMap[i, k].ArroundTiles[0] = TileMap[i, k];
                    TileMap[i, k].ArroundTiles[5] = TileMap[i, k];
                }
                if (k == height - 1)
                {
                    TileMap[i, k].ArroundTiles[2] = TileMap[i, k];
                    TileMap[i, k].ArroundTiles[3] = TileMap[i, k];
                }

            }
        }
    }
    private void CheckTouch()
    {
        //Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    case "Tile":
                        Tiles t = hit.collider.GetComponent<Tiles>();
                        if (t.somethingOn)
                            if (t.unit.CompareTag("Hero"))
                                if (t.unit != SelectedUnit)
                                    SelectHero(t.unit);
                        break;
                    //case "Hero":
                    //    Unit sel = hit.transform.GetComponent<Unit>();
                    //    if (sel != SelectedUnit)
                    //        SelectHero(sel);
                    //    break;
                    default:
                        break;
                }
                //if (hit.collider.CompareTag("Tile"))
                //{


                //    //foreach (Tiles t_ in t.ArroundTiles + t)
                //    //{
                //    //    t_.ToggleSelect();
                //    //}
                //}

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Tiles t = hit.collider.GetComponent<Tiles>();
                    Debug.Log("move");
                    if (SelectedUnit != null)
                        SelectedUnit.FindPath(t);
                }
            }
        }
    }
    public void SelectHero(Unit unit)
    {
        GameObject Hero = unit.gameObject;
        SelectedUnit = unit;
        Vector3 unitpos = unit.transform.position;
        Selected_Pointer.SetActive(true);
        Selected_Pointer.transform.localPosition = unitpos;
    }
    public void Action(Tiles tile)
    {
        switch (selectmode)
        {
            case "move":
                break;
            default:
                break;
        }
    }
    public void MapFlush()
    {
        for (int i = 0; i < width; i++)
            for (int k = 0; k < height; k++)
                TileMap[i, k].Flush();
    }
}
