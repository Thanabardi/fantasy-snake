using UnityEngine;
using Thanabardi.FantasySnake.Core.GameWorld;
using System.Collections.Generic;
using Thanabardi.FantasySnake.Utility;

namespace Thanabardi.FantasySnake.Core.GameSystem
{
    public class GridManager : MonoBehaviour
    {
        [Header("Border Size")]
        [SerializeField]
        private int _boardWidth;
        [SerializeField]
        private int _boardHeight;

        [Header("Map Tile")]
        [SerializeField]
        private GridTile _gridTile;
        [SerializeField]
        private GameObject[] _wallTiles;

        [Space(20)]
        [SerializeField]
        private Obstacle[] _obstacles;

        private GridTile[,] _gridTiles;

        private List<GridTile> _emptyGridTiles;
        private Dictionary<WorldItem, GridTile> _usedGridTiles;

        private void Awake()
        {
            try
            {
                // load configuration with minimum board size of 10x10
                _boardWidth = Mathf.Max(10, GameConfig.ConfigData.BoardWidth);
                _boardHeight = Mathf.Max(10, GameConfig.ConfigData.BoardHeight);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            _gridTiles = new GridTile[_boardWidth, _boardHeight];
            _emptyGridTiles = new();
            _usedGridTiles = new();
            GenerateMap();
            PlaceObstacle();
        }

        public bool TryGetRandomEmptyTiles(int width, int height, out List<GridTile> gridTiles)
        {
            if (_emptyGridTiles.Count == 0)
            {
                gridTiles = null;
                return false;
            }
            List<GridTile> emptyTies = _emptyGridTiles;
            List<GridTile> selectedTiles = new();
            while (true)
            {
                GridTile gridTile = emptyTies[Random.Range(0, emptyTies.Count)];
                emptyTies.Remove(gridTile);
                selectedTiles.Clear();
                for (int row = 0; row < width; row++)
                {
                    for (int col = 0; col < height; col++)
                    {
                        if (GetWorldAreaOffset(gridTile, row, col, out GridTile selectedTile) &&
                            selectedTile.ContainedItem == null)
                        {
                            selectedTiles.Add(selectedTile);
                        }
                    }
                }
                if (selectedTiles.Count == width * height)
                {
                    gridTiles = selectedTiles;
                    return true;
                }
            }
        }

        public bool TryGetRandomEmptyTile(out GridTile gridTile)
        {
            gridTile = null;
            if (_emptyGridTiles.Count > 0)
            {
                GridTile emptyTile = _emptyGridTiles[Random.Range(0, _emptyGridTiles.Count)];
                gridTile = emptyTile;
                return true;
            }
            else { return false; }
        }

        public bool TryGetGridTile(WorldItem worldItem, out GridTile gridTile)
        {
            return _usedGridTiles.TryGetValue(worldItem, out gridTile);
        }

        public bool MoveWorldItemBy(WorldItem worldItem, int disX, int disY, out GridTile gridTile)
        {
            return GetWorldAreaOffset(_usedGridTiles[worldItem], disX, disY, out gridTile);
        }

        public void AddItemOnTile(WorldItem worldItem, GridTile destination)
        {
            if (_usedGridTiles.TryGetValue(worldItem, out GridTile usedGridTile))
            {
                // update and put usedGridTile back to emptyGridTiles list
                usedGridTile.SetContainedItem(null);
                _emptyGridTiles.Add(usedGridTile);
            }
            _usedGridTiles[worldItem] = destination;
            destination.SetContainedItem(worldItem);
            _emptyGridTiles.Remove(destination);
        }

        public void RemoveWorldItem(WorldItem worldItem)
        {
            if (_usedGridTiles.TryGetValue(worldItem, out GridTile currentGridTile))
            {
                currentGridTile.SetContainedItem(null);
                _emptyGridTiles.Add(currentGridTile);
                _usedGridTiles.Remove(worldItem);
            }
        }

        private void GenerateMap()
        {
            Queue<GameObject> _wallTileQueue = new(_wallTiles);
            GameObject board = new("Board");
            GameObject BoardWall = new("BoardWall");
            for (int row = 0; row < _boardWidth; row++)
            {
                for (int column = 0; column < _boardHeight; column++)
                {
                    GridTile gridTile = Instantiate(_gridTile, new Vector3(row, 0, column), Quaternion.Euler(90, 0, 0), board.transform);
                    gridTile.Initialize(row, column);
                    _gridTiles[row, column] = gridTile;
                    _emptyGridTiles.Add(gridTile);
                    if (row == 0)
                    {
                        // spawn wall on the left
                        SpawnWall(new Vector3(row - 0.5f, 0, column), Quaternion.Euler(0, -90, 0), BoardWall.transform, ref _wallTileQueue);
                    }
                    else if (row == _boardWidth - 1)
                    {
                        // spawn wall on right
                        SpawnWall(new Vector3(row + 0.5f, 0, column), Quaternion.Euler(0, 90, 0), BoardWall.transform, ref _wallTileQueue);
                    }
                    if (column == _boardHeight - 1)
                    {
                        // spawn wall on top
                        SpawnWall(new Vector3(row, 0, column + 0.5f), Quaternion.identity, BoardWall.transform, ref _wallTileQueue);
                    }
                }
            }
        }

        private void PlaceObstacle()
        {
            foreach (Obstacle obstacle in _obstacles)
            {
                if (TryGetRandomEmptyTiles(obstacle.Width, obstacle.Height, out List<GridTile> gridTiles))
                {
                    Vector3 position = gridTiles[0].transform.position;
                    Instantiate(obstacle, new Vector3(position.x, position.y + 0.01f, position.z), Quaternion.Euler(90, 0, 0));
                    foreach (GridTile gridTile in gridTiles)
                    {
                        gridTile.SetContainedItem(obstacle);
                        _emptyGridTiles.Remove(gridTile);
                    }
                }
                else { return; }
            }
        }

        private void SpawnWall(Vector3 position, Quaternion quaternion, Transform parent, ref Queue<GameObject> _wallTileQueue)
        {
            if (_wallTileQueue.TryDequeue(out GameObject wall))
            {
                Instantiate(wall, position, quaternion, parent);
                _wallTileQueue.Enqueue(wall);
            }
        }

        private bool GetWorldAreaOffset(GridTile currentGridTile, int offsetX, int offsetY, out GridTile gridTile)
        {
            int newRow = currentGridTile.GridRow + offsetX;
            int newColumn = currentGridTile.GridColumn + offsetY;
            // check next position is within world area
            if ((newRow >= 0 && newRow < _boardWidth) &&
                (newColumn >= 0 && newColumn < _boardHeight))
            {
                gridTile = _gridTiles[newRow, newColumn];
                return true;
            }
            gridTile = null;
            return false;
        }
    }
}