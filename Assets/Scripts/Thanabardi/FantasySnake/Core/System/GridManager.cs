using UnityEngine;
using Thanabardi.FantasySnake.Core.GameWorld;
using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;

namespace Thanabardi.FantasySnake.Core.System
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private int _boardWidth;
        [SerializeField]
        private int _boardHeight;

        [SerializeField]
        private GridTile _gridTile;

        public GridTile[,] GridTiles { get; private set; }

        private List<GridTile> _emptyGridTiles;
        private Dictionary<WorldItem, GridTile> _usedGridTiles;

        private void Awake()
        {
            // _boardWidth = GameConfig.ConfigData.BoardWidth;
            // _boardHeight = GameConfig.ConfigData.BoardHeight;
            GridTiles = new GridTile[_boardWidth, _boardHeight];
            _emptyGridTiles = new();
            _usedGridTiles = new();
            GenerateMap();
        }

        private void GenerateMap()
        {
            GameObject board = new("Board");
            for (int row = 0; row < _boardWidth; row++)
            {
                for (int column = 0; column < _boardHeight; column++)
                {
                    GridTile gridTile = Instantiate(_gridTile, new Vector3(row, 0, column), Quaternion.Euler(90, 0, 0), board.transform);
                    gridTile.Initialize(row, column);
                    GridTiles[row, column] = gridTile;
                    _emptyGridTiles.Add(gridTile);
                }
            }
        }

        public GridTile GetRandomEmptyTile()
        {
            GridTile emptyTile = _emptyGridTiles[Random.Range(0, _emptyGridTiles.Count)];
            return emptyTile;
        }

        public bool MoveWorldItemBy(WorldItem worldItem, int disX, int disY, out GridTile gridTile)
        {
            GridTile currentGridTile = _usedGridTiles[worldItem];
            gridTile = null;
            if (currentGridTile == null)
            {
                Debug.Log($"there is no grid tile that contain {worldItem.name}");
                return false;
            }
            int newRow = currentGridTile.GridRow + disX;
            int newColumn = currentGridTile.GridColumn + disY;
            // check next position is within world area
            if ((newRow >= 0 && newRow < _boardWidth) &&
                (newColumn >= 0 && newColumn < _boardHeight))
            {
                gridTile = GridTiles[newRow, newColumn];
                return true;
            }
            return false;
        }

        public void PlaceWorldItem(WorldItem worldItem, GridTile destination)
        {
            // Bug potential empty array out of range maybe doesn't return grid to empty arry properly
            worldItem.transform.position = destination.transform.position;
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
                Destroy(worldItem.gameObject);
            }
        }
    }
}