using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Zephyr.University.Presenter
{
    public class CursorTilePresenter : MonoBehaviour
    {

        public Tilemap GroundTilemap;

        /// <summary>
        /// 光标当前所在的Cell
        /// </summary>
        public Vector3Int CurrentCell;

        private Vector3Int _newCell;

        public ReactiveProperty<bool> Active;

        public SpriteRenderer CursorTileImage;

        // Use this for initialization
        private void Start()
        {
            Active = new ReactiveProperty<bool>();

            //激活时动效
            Active.Where(active => active).Subscribe(_ =>
            {
                CursorTileImage.DOFade(1, 0.4f);
            });

            //关闭时动效
            Active.Where(active => !active).Subscribe(_ =>
            {
                CursorTileImage.DOFade(0, 0.4f);
            });

            //tile光标跟随鼠标
            Observable.EveryUpdate()
                .Where(l =>
            {
                //条件：鼠标移动到别的cell中
                Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _newCell = GroundTilemap.WorldToCell(mouseInWorld);
                return _newCell != CurrentCell;
            }).Subscribe(l =>
            {
                //行为：更新光标位置
                CurrentCell = _newCell;
                transform.DOMove(GroundTilemap.CellToWorld(CurrentCell), 0.2f);
            });
        }
    }
}