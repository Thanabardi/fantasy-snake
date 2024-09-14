using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;

namespace Thanabardi.FantasySnake.Utility
{
    public class GridReferenceUtility
    {
        private readonly Dictionary<GridTile, WorldItem> _gridTileDict;
        private readonly Dictionary<WorldItem, GridTile> _worldItemDict;

        public GridReferenceUtility(GridTile[] gridTiles)
        {
            _gridTileDict = new();
            foreach (var tile in gridTiles)
            {
                _gridTileDict.Add(tile, null);
            }
            _worldItemDict = new();
        }

        public void AddWorldItem(GridTile gridTile, WorldItem worldItem)
        {
            if (_worldItemDict.ContainsKey(worldItem))
            {
                Debug.LogError($"{worldItem.name} already in {_worldItemDict[worldItem].name}");
                return;
            }
            _gridTileDict[gridTile] = worldItem;
            _worldItemDict.Add(worldItem, gridTile);
        }

        public void RemoveWorldItem(WorldItem worldItem)
        {
            GridTile gridTile = _worldItemDict[worldItem];
            _worldItemDict.Remove(worldItem);
            _gridTileDict[gridTile] = null;
        }

        public void UpdateItem(WorldItem worldItem, GridTile newGridTile)
        {
            // remove worldItem on current GridTile
            GridTile currentTile = _worldItemDict[worldItem];
            _gridTileDict[currentTile] = null;

            // update worldItem and next nextGridTile
            if (_gridTileDict[newGridTile] != null)
            {
                Debug.LogError($"{newGridTile.name} already contain {_gridTileDict[newGridTile].name}");
                return;
            }
            _worldItemDict[worldItem] = newGridTile;
            _gridTileDict[newGridTile] = worldItem;
        }

        public GridTile GetGridTile(WorldItem worldItem)
        {
            return _worldItemDict[worldItem];
        }

        public WorldItem GetWorldItem(GridTile gridTile)
        {
            return _gridTileDict[gridTile];
        }
    }
}
