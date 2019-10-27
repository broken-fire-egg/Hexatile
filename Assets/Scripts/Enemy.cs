using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public float idleTMin;
    public float idleTMax;
    public override void Update()
    {
        base.Update();
        if (action == "idle")
        {
            float waitT = Random.Range(idleTMin, idleTMax);
            action = "thinkingAboutMove";
            Invoke("MoveOneTile", waitT);
        }
    }
    public new void MoveCommand(Tiles tile)
    {
        StartCoroutine(Move(tile));
    }
    public void MoveOneTile()
    {
        
        Tiles t = groundtile.ArroundTiles[Random.Range(0, groundtile.ArroundTiles.Length)];
        while (t == groundtile || t.somethingOn)
            t = groundtile.ArroundTiles[Random.Range(0, groundtile.ArroundTiles.Length)];
        MoveCommand(t);

    }

}
