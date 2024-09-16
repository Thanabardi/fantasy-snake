using UnityEngine;
using Thanabardi.FantasySnake.Core.GameWorld;
using System.Collections.Generic;

namespace Thanabardi.FantasySnake.Core.System
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
            // _boardWidth = GameConfig.ConfigData.BoardWidth;
            // _boardHeight = GameConfig.ConfigData.BoardHeight;
            _gridTiles = new GridTile[_boardWidth, _boardHeight];
            _emptyGridTiles = new();
            _usedGridTiles = new();
            GenerateMap();
            PlaceObstacle();
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
                        SpawnWallHandler(new Vector3(row - 0.5f, 0, column), Quaternion.Euler(0, -90, 0), BoardWall.transform, ref _wallTileQueue);
                    }
                    else if (row == _boardWidth - 1)
                    {
                        // spawn wall on right
                        SpawnWallHandler(new Vector3(row + 0.5f, 0, column), Quaternion.Euler(0, 90, 0), BoardWall.transform, ref _wallTileQueue);
                    }
                    if (column == _boardHeight - 1)
                    {
                        // spawn wall on top
                        SpawnWallHandler(new Vector3(row, 0, column + 0.5f), Quaternion.identity, BoardWall.transform, ref _wallTileQueue);
                    }
                }
            }
        }

        private void PlaceObstacle()
        {
            foreach (Obstacle obstacle in _obstacles)
            {
                List<GridTile> gridTiles = GetRandomEmptyTiles(obstacle.Width, obstacle.Height);
                Vector3 position = gridTiles[0].transform.position;
                Instantiate(obstacle, new Vector3(position.x, position.y + 0.01f, position.z), Quaternion.Euler(90, 0, 0));
                foreach (GridTile gridTile in gridTiles)
                {
                    gridTile.SetContainedItem(obstacle);
                    _emptyGridTiles.Remove(gridTile);
                    // _usedGridTiles.Add(obstacle, gridTile);
                }
            }
        }

        public List<GridTile> GetRandomEmptyTiles(int width, int height)
        {
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
                    return selectedTiles;
                }
            }
        }

        public bool TryGetGridTile(WorldItem worldItem, out GridTile gridTile)
        {
            return _usedGridTiles.TryGetValue(worldItem, out gridTile);
        }

        private void SpawnWallHandler(Vector3 position, Quaternion quaternion, Transform parent, ref Queue<GameObject> _wallTileQueue)
        {
            if (_wallTileQueue.TryDequeue(out GameObject wall))
            {
                Instantiate(wall, position, quaternion, parent);
                _wallTileQueue.Enqueue(wall);
            }
        }

        public GridTile GetRandomEmptyTile()
        {
            GridTile emptyTile = _emptyGridTiles[Random.Range(0, _emptyGridTiles.Count)];
            return emptyTile;
        }

        public bool MoveWorldItemBy(WorldItem worldItem, int disX, int disY, out GridTile gridTile)
        {
            return GetWorldAreaOffset(_usedGridTiles[worldItem], disX, disY, out gridTile);
        }

        public void AddItemOnTile(WorldItem worldItem, GridTile destination)
        {
            // remove old grid tile on update position
            if (_usedGridTiles.TryGetValue(worldItem, out var usedGridTile))
            {
                _usedGridTiles[worldItem].SetContainedItem(null);
                _usedGridTiles.Remove(worldItem);
                _emptyGridTiles.Add(usedGridTile);
            }
            destination.SetContainedItem(worldItem);
            _emptyGridTiles.Remove(destination);
            _usedGridTiles.Add(worldItem, destination);
        }

        public void RemoveWorldItem(WorldItem worldItem)
        {
            GridTile currentGridTile = _usedGridTiles[worldItem];
            if (currentGridTile != null)
            {
                currentGridTile.SetContainedItem(null);
                _emptyGridTiles.Add(currentGridTile);
                _usedGridTiles.Remove(worldItem);
                // Destroy(worldItem.gameObject);
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