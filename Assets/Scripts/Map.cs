using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public Sprite TileSprite;
    public int width;
    public int height;
    public GameObject TileObject;
    public Tiles[,] TileMap;
    
    // Use this for initialization
    void Start() {
        Init();
        foreach (Tiles element in TileMap[1, 1].ControllArroundTiles(2, true))
        {
            element.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    // Update is called once per frame
    void Update() {

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
            for(int k=0; k < height; k++)
            {
                if(k % 2 == 0)
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

}
