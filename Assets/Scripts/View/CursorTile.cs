using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Zephyr.University.View
{
    public class CursorTile : MonoBehaviour
    {

        public Tilemap GroundTilemap;

        /// <summary>
        /// 光标当前所在的Cell
        /// </summary>
        public Vector3Int CurrentCell;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            //鼠标转世界
            Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //以2d tile的比例缩小后转cell pos
            CurrentCell = GroundTilemap.WorldToCell(mouseInWorld);
            //更新cursor位置
            transform.DOMove(GroundTilemap.CellToWorld(CurrentCell), 0.2f);
        }
    }
}