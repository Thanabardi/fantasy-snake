using UnityEngine;
using Thanabardi.FantasySnake.Core.GameWorld;

namespace Thanabardi.FantasySnake.Core.System
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private int _boardWidth;
        [SerializeField, Min(1)]
        private int _boardHeigh;

        [SerializeField]
        private GridTile _gridTile;

        private GridTile[,] _gridTileArray;

        private void Start()
        {
            _gridTileArray = new GridTile[_boardWidth, _boardHeigh];
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                for (int y = 0; y < _boardHeigh; y++)
                {
                    GameObject tiles = Instantiate(_gridTile.gameObject, new Vector3(x, 0, y), Quaternion.Euler(90, 0, 0), transform);
                    tiles.name = $"Tile {x}, {y}";
                    _gridTileArray[x, y] = tiles.GetComponent<GridTile>();
                }
            }
        }
    }
}