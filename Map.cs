using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using Random = UnityEngine.Random;


public class Map
{
    private Grid<MapGridObject> grid;
    public event EventHandler OnEnTireMapRevealed;

    public Map()
    {
        grid = new Grid<MapGridObject>(10, 10, 1f, Vector3.zero, (Grid<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));

        int minesPlaced = 0;
        int generateMineAmount = 10;
        while (minesPlaced < generateMineAmount)
        {
            int x = Random.Range(0, grid.GetWidth());
            int y = Random.Range(0, grid.GetHeight());

            MapGridObject mapGridObject = grid.GetGridObject(x, y);

            if (mapGridObject.GetGridType() != MapGridObject.Type.Mine)
            {
                mapGridObject.SetGridType(MapGridObject.Type.Mine);
                minesPlaced++;
            }
        }

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapGridObject.Type.Empty)
                {
                    //tinh gtri o ke ben bom
                    List<MapGridObject> neighbourList = GetNeighbourList(x, y);

                    int mineCount = 0;
                    foreach (MapGridObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapGridObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }
                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapGridObject.Type.MineNum_1); break;
                        case 2: mapGridObject.SetGridType(MapGridObject.Type.MineNum_2); break;
                        case 3: mapGridObject.SetGridType(MapGridObject.Type.MineNum_3); break;
                        case 4: mapGridObject.SetGridType(MapGridObject.Type.MineNum_4); break;
                        case 5: mapGridObject.SetGridType(MapGridObject.Type.MineNum_5); break;
                        case 6: mapGridObject.SetGridType(MapGridObject.Type.MineNum_6); break;
                        case 7: mapGridObject.SetGridType(MapGridObject.Type.MineNum_7); break;
                        case 8: mapGridObject.SetGridType(MapGridObject.Type.MineNum_8); break;
                    }
                }
            }
        }
    }

    public Grid<MapGridObject> GetGrid()
    {
        return grid;
    }

    private List<MapGridObject> GetNeighbourList(MapGridObject mapGridObject)
    {
        return GetNeighbourList(mapGridObject.GetX(), mapGridObject.GetY());
    }

    private List<MapGridObject> GetNeighbourList(int x, int y)
    {
        List<MapGridObject> neighbourList = new List<MapGridObject>();

        if (x - 1 >= 0)
        {
            //Left
            neighbourList.Add(grid.GetGridObject(x - 1, y));
            //Left Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x - 1, y - 1));
            //Left Up
            if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x - 1, y + 1));
        }
        if (x + 1 < grid.GetWidth())
        {
            //Right
            neighbourList.Add(grid.GetGridObject(x + 1, y));
            //Right Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x + 1, y - 1));
            //Right Up
            if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x + 1, y + 1));
        }
        //Up
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x, y - 1));
        //Down
        if (y + 1 < grid.GetHeight()) neighbourList.Add(grid.GetGridObject(x, y + 1));

        return neighbourList;
    }

    public void FlagGridPosition(Vector3 worldPosition)
    {
        MapGridObject mapGridObject = grid.GetGridObject(worldPosition);
        if (mapGridObject != null)
        {
            mapGridObject.SetFlagged();
        }
    }

    public MapGridObject.Type RevealGridPosition(Vector3 worldPosition)
    {
        MapGridObject mapGridObject = grid.GetGridObject(worldPosition);
        return RevealGridPosition(mapGridObject);
    }

    public MapGridObject.Type RevealGridPosition(MapGridObject mapGridObject)
    {
        mapGridObject.Reveal();
        if (mapGridObject.GetGridType() == MapGridObject.Type.Empty)
        {
            List<MapGridObject> alreadyCheckedNeighbourList = new List<MapGridObject>();
            List<MapGridObject> checkNeighbourList = new List<MapGridObject>();
            checkNeighbourList.Add(mapGridObject);

            while (checkNeighbourList.Count > 0)
            {
                MapGridObject checkMapGridObject = checkNeighbourList[0];
                checkNeighbourList.RemoveAt(0);
                alreadyCheckedNeighbourList.Add(checkMapGridObject);

                foreach (MapGridObject neighbour in GetNeighbourList(checkMapGridObject))
                {
                    if (neighbour.GetGridType() != MapGridObject.Type.Mine)
                    {
                        neighbour.Reveal();
                        if (neighbour.GetGridType() == MapGridObject.Type.Empty)
                        {
                            if (!alreadyCheckedNeighbourList.Contains(neighbour))
                            {
                                checkNeighbourList.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }
        if(IsEntireMapRevealed())
        {
            //win game
            OnEnTireMapRevealed?.Invoke(this, EventArgs.Empty);
        }

        return mapGridObject.GetGridType();
    }
    private bool IsEntireMapRevealed()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() != MapGridObject.Type.Mine)
                {
                    if (!mapGridObject.IsRevealed())
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void RevealEntireMap()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MapGridObject mapGridObject = grid.GetGridObject(x, y);
                mapGridObject.Reveal();
            }
        }
    }
}
