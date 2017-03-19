using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Zephyr.University.Presenter
{
    public class PlayerPresenter : MonoBehaviour
    {

        /// <summary>
        /// 玩家操作状态
        /// </summary>
        public enum OperateState
        {
            Idle, //待机
            Planning //规划
        }

        public OperateState CurrentOperateState;

        /// <summary>
        /// todo 临时：直接从这里连接到按钮，正式应该从按钮组进行统一和动态划分
        /// </summary>
        public Button BuildButton;

        private Vector3Int _cellPlanningStart, _cellPlanningEnd;

        public Tilemap GroundTilemap, GroundPreviewLayer;

        public TileBase GroundPreviewTile;

        /// <summary>
        /// 光标当前所在的Cell
        /// </summary>
        public Vector3Int CurrentCell;

        private Vector3Int _newCell;

        /// <summary>
        /// 地表预览的绘制cell列表，玩家移动鼠标时更新
        /// </summary>
        private List<Vector3Int> _groundPreviewCells = new List<Vector3Int>();

        // Use this for initialization
        private void Start()
        {
            //更新鼠标当前cell
            Observable.EveryUpdate()
                .Subscribe(l =>
            {
                //行为：更新光标位置
                CurrentCell = Utils.GetMouseCell(GroundTilemap);
            });

            //按下建设按钮就进入规划状态
            BuildButton.OnClickAsObservable().Subscribe(_ =>
            {
                CurrentOperateState = OperateState.Planning;
            });

            //规划状态下的各种处理
            Observable.EveryUpdate()
                .Where(l => CurrentOperateState == OperateState.Planning)
                .Subscribe(_ =>
                { 
                    if (Input.GetMouseButtonUp(1))
                    {
                        //右键点击回到待机状态
                        ClearGroundPreviewCells();
                        CurrentOperateState = OperateState.Idle;
                    }else if (!Input.GetMouseButton(0))
                    {
                        //单一块预览(鼠标没按下时)
                        var mouseCell = Utils.GetMouseCell(GroundTilemap);
                        UpdateGroundPreviewCells(mouseCell, mouseCell);
                    }else if (Input.GetMouseButtonDown(0))
                    {
                        //左键点下时记录起始tile
                        _cellPlanningStart = CurrentCell;
                    }else if (Input.GetMouseButton(0))
                    {
                        //按住时获取结束tile
                        _cellPlanningEnd = CurrentCell;
                        //基于起始位置更新绘制区方块
                        UpdateGroundPreviewCells(_cellPlanningStart, _cellPlanningEnd);
                    }

                    //地表预览层更新要绘制的cell
                    GroundPreviewLayer.ClearAllTiles();
                    foreach (var cell in _groundPreviewCells)
                    {
                        GroundPreviewLayer.SetTile(cell, GroundPreviewTile);
                    }
                });
        }

        /// <summary>
        /// 清空地表预览区
        /// </summary>
        private void ClearGroundPreviewCells()
        {
            _groundPreviewCells.Clear();
        }

        /// <summary>
        /// 更新地表预览区要绘制的cell数组
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        private void UpdateGroundPreviewCells(Vector3Int startCell, Vector3Int endCell)
        {
            ClearGroundPreviewCells();

            int minX = Math.Min(startCell.x, endCell.x);
            int maxX = Math.Max(startCell.x, endCell.x);

            int minY = Math.Min(startCell.y, endCell.y);
            int maxY = Math.Max(startCell.y, endCell.y);

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    _groundPreviewCells.Add(new Vector3Int(i, j, 0));
                }
            }
        }
    }
}