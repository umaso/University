using UnityEngine;
using UnityEngine.Tilemaps;

namespace Zephyr.University.Presenter
{
    public class Utils
    {
        public static Vector3Int GetMouseCell(Tilemap inTileMap)
        {
            Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inTileMap.WorldToCell(mouseInWorld);
        }
    }
}