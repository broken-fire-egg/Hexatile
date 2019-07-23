using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour {
    public enum Ways { RightUp,Right,RightDown,LeftDown,Left,LeftUp }
    public bool selected;
    public int value;
    public int pos_x;
    public int pos_y;
    public Tiles[] ArroundTiles = new Tiles[6];
    SpriteRenderer spriteRenderer;
#region Operators
    public static Tiles[] operator +(Tiles _tiles, Tiles[] _tile)
    {
        if (_tile == null)
        {
            Tiles[] tmp = new Tiles[1];
            tmp[0] = _tiles;
            return tmp;
        }
        if (_tiles == null)
        {
            return _tile;
        }

        Tiles[] result = new Tiles[_tile.Length + 1];
        result[0] = _tiles;

        for (int i = 1; i < result.Length; i++)
            result[i] = _tile[i - 1];

        return result;
    }
    public static Tiles[] operator +(Tiles[] _tiles, Tiles _tile)
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

    #endregion

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    #region ControllFunctions
    public Tiles[] ControllStraightTiles(Ways way,int size,bool Bi_direction = false)
    {
        if (Bi_direction == true)
        {
            Tiles[] BI_result = new Tiles[size*2];
            BI_result = SumTiles( SearchTiles(null, way, size),SearchTiles(null,((int)way > 2) ? way-3:way + 3,size));
            return BI_result;
        }

        Tiles[] result = new Tiles[size];

        result = SearchTiles(null, way, size);
        return result;
    }
    public Tiles[] ControllArroundTiles(int size, bool includeSelf = true)
    {
       
        Tiles[] result = new Tiles[size *6 + (includeSelf ? 1 : 0)];
        if (includeSelf)
        {
            result = SumTiles(SumTiles(ControllStraightTiles(Ways.RightUp, size, true),
                        ControllStraightTiles(Ways.Right, size, true)),
                           ControllStraightTiles(Ways.RightDown, size, true)) + this;
        }
        else
            result = SumTiles(SumTiles(ControllStraightTiles(Ways.RightUp, size, true),
                      ControllStraightTiles(Ways.Right, size, true)),
                         ControllStraightTiles(Ways.RightDown, size, true));
        return result;
    }
    public Tiles[] SumTiles(Tiles[] tiles1, Tiles[] tiles2)
    {
        int startpoint = 0;
        int startpoint2 = 0;
        int index = 0;
 

        Tiles[] result = new Tiles[tiles1.Length + tiles2.Length];
        int j = 0;
        while (startpoint != tiles1.Length)
        {
            result[index] = tiles1[j];
            index++; j++; startpoint++;
        }
        j = 0;
        while (startpoint2 != tiles2.Length)
        {
            result[index] = tiles2[j];
            index++; j++; startpoint2++;
        }
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
    #endregion

#region Status
    public void ToggleSelect()
    {
        selected = !selected;
        if (selected)
            spriteRenderer.color = new Color(0, 0, 0);
        else
            spriteRenderer.color = new Color(1, 1, 1);

    }
    #endregion
}
