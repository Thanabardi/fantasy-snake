using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameWorld
{
    public class GridTile : MonoBehaviour
    {
        public int GridRow { get; private set; }
        public int GridColumn { get; private set; }
        public WorldItem ContainedItem { get; private set; }

        public void Initialize(int row, int column)
        {
            GridRow = row;
            GridColumn = column;
            gameObject.name = $"Tile {row}, {column}";
        }

        public void SetContainedItem(WorldItem worldItem)
        {
            ContainedItem = worldItem;
        }
    }
}