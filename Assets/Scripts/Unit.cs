using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TileScore
{
    public Tiles tile;
    public Tiles prev;
    public bool parent = false;
    public int h = 0, g = 0;
    public int getscore() { return h + g; }
    public TileScore(Tiles start, Tiles target,Tiles _prev,int _g=0)
    {
        g = _g;
        tile = start;
        prev = _prev;
        h = Tiles.GetDistancePoint(start, target);
    }
}
public class Unit : MonoBehaviour
{
    enum action_enum { idle, move };
    public float hp;
    public float max_hp;

    public SpriteRenderer hp_bar;
    public SpriteRenderer max_hp_bar;
    public SpriteRenderer sr;
    public Tiles groundtile;
    Vector2 destination_pos;
    Vector2 origin_pos;
    float move_progress;
    public float move_speed;
    public int move_range = 2;
    public string action;
    Queue<Tiles> moveQue;

    public void Start()
    {
        action = "idle";
        //FindPath(Map.instance.TileMap[3, 3]);
    }
    public virtual void Update()
    {
        UIupdate();
            
    }

    private void UIupdate()
    {

        Vector3 bar_pos = new Vector3(-max_hp_bar.sprite.rect.width / 200, 1.4f, 0);
        max_hp_bar.transform.localPosition = bar_pos;
        hp_bar.transform.localPosition = bar_pos;
        hp_bar.transform.localScale = new Vector3(hp / max_hp, 1, 1);
    }
    public IEnumerator Idle()
    {
        while (true)
        {
            yield return null;
        }
    }
    public IEnumerator Move(Tiles tile)
    {
        bool cross = false;
        action = "move";
        origin_pos = transform.localPosition;
        var target_pos = tile.transform.position;
        tile.somethingOn = true;
        tile.unit = this;

        move_progress = 0f;
        while (true)
        {
            transform.localPosition = new Vector2(Mathfx.Hermite(origin_pos.x, target_pos.x, move_progress), Mathfx.Hermite(origin_pos.y, target_pos.y, move_progress));
            //transform.localPosition = Vector2.Lerp(origin_pos, tile.transform.localPosition, move_progress);
            if (!cross && move_progress > 0.5f)
            {
                groundtile.Flush();
                groundtile = tile;
                cross = true;
            }
            if (move_progress == 1.0f)
            {
                action = "idle";
                //groundtile.Flush();
                //groundtile = tile;

                yield break;
            }
            move_progress += move_speed * Time.deltaTime;
            move_progress = move_progress > 1.0f ? 1.0f : move_progress;

            yield return null;
        }
    }
    public void MoveCommand(Tiles tile)
    {
        if (action != "idle")
            return;
        StartCoroutine(Move(tile));
    }
    public void FindPath(Tiles target_tile)
    {
        Map.instance.MapFlush();
        //길찾기는 A* 알고리즘 사용중임
        List<Tiles> close = new List<Tiles>();
        List<Tiles> open = new List<Tiles>();
        Dictionary<Tiles, TileScore> scores = new Dictionary<Tiles, TileScore>();
        Tiles currentT = null;
        open.Add(groundtile);
        scores.Add(groundtile, new TileScore(groundtile, target_tile,null));

        while (true)
        {
            if (open.Count == 0) //실패
            {
                Debug.Log("실패");
                return;
            }
            else if (close.Contains(target_tile)) //성공
            {
                Debug.Log("성공");
                Stack<Tiles> tmpway = new Stack<Tiles>();
                Tiles tmp = target_tile;
                foreach (TileScore score in scores.Values)
                    if (score.parent == true)
                        score.tile.Highlight(0.8f);
                while (tmp != groundtile)
                {
                    tmp.Highlight(0.5f);
                    tmp = scores[tmp].prev;
                }
                target_tile.SetColor(1, 0, 0);
                
                return;
            }

            int minsc = 999999;
            foreach (TileScore score in scores.Values)
            {
                if(open.Contains(score.tile))
                if (minsc > score.getscore())
                {
                    minsc = score.getscore();
                    currentT = score.tile;
                }
            }
            open.Remove(currentT);
            close.Add(currentT);


            foreach (Tiles t in currentT.ArroundTiles)
            {
                if (t == null)
                    continue;
                if (t.blocker || t.somethingOn || close.Contains(t))
                    continue;
                if (!open.Contains(t))
                {
                    open.Add(t);
                    scores.Add(t, new TileScore(t, target_tile, currentT,scores[currentT].g + 1));
                    scores[currentT].parent = true;
                }
                else
                {
                    if (scores[t].g > scores[currentT].g + 1)
                    {
                        Updatescore(scores, t, scores[currentT].g + 1,currentT);
                        scores[currentT].parent = true;
                        break;
                    }
                }

            }
        }
    }
    void Updatescore(Dictionary<Tiles, TileScore> scores, Tiles tile, int n, Tiles newprev)
    {
        scores[tile].g = n;
        scores[tile].prev = newprev;
        foreach(TileScore ts in scores.Values)
            if (ts.prev == tile)
                Updatescore(scores, ts.tile, n + 1,tile);
    }
}
