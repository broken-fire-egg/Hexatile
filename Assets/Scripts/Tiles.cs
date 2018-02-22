using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour {
    public enum Ways { RightUp,Right,RightDown,LeftDown,Left,LeftUp }
    public int value;
    public int pos_x;
    public int pos_y;
    public Tiles[] ArroundTiles = new Tiles[6];

    public static Tiles[] operator+(Tiles[] _tiles,Tiles _tile)
    {
        if (_tiles == null)
        {
            Tiles[] tmp = new Tiles[1];
            tmp[0] = _tile;
            return tmp;
        }
        if(_tile == null)
        {
            return _tiles;
        }

        Tiles[] result = new Tiles[_tiles.Length + 1];
        result[0] = _tile;

        for(int i=1;i< result.Length;i++)
            result[i] = _tiles[i - 1];

        return result;
    }
    public static Tiles[] operator +(Tiles tile1, Tiles tile2)
    {
        if (tile1 == null|| tile2 == null)
            return null;
       

        Tiles[] result = new Tiles[2];
        result[0] = tile2;
        result[1] = tile1;
        return result;
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Tiles[] ControllTiles(Ways way,int size,bool Bi_direction = false)
    {
        Tiles[] result = new Tiles[size];
        result = SearchTiles(null, way, size);
        return result;
    }
    Tiles[] SearchTiles(Tiles[] Searched,Ways way,int size)
    {
        if(size == 0)
          return null;

        Debug.Log(size.ToString());

        if(ArroundTiles[(int)way] == this)
            return ArroundTiles[(int)way].SearchTiles(Searched, way, 0) + this;

        return ArroundTiles[(int)way].SearchTiles(Searched,way,size-1) + this;

    }
}
