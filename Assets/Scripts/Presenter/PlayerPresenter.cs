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

        public Tilemap GroundTilemap;

        /// <summary>
        /// 光标当前所在的Cell
        /// </summary>
        public Vector3Int CurrentCell;

        private Vector3Int _newCell;

        // Use this for initialization
        private void Start()
        {
            //更新鼠标当前cell
            Observable.EveryUpdate()
                .Subscribe(l =>
            {
                //行为：更新光标位置
                Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CurrentCell = GroundTilemap.WorldToCell(mouseInWorld);
            });

            //按下建设按钮就进入规划状态
            BuildButton.OnClickAsObservable().Subscribe(_ =>
            {
                CurrentOperateState = OperateState.Planning;
            });

            //规划状态下右键点击进入待机状态
            Observable.EveryUpdate()
                .Where(l => CurrentOperateState == OperateState.Planning)
                .Where(l => Input.GetMouseButtonUp(1))
                .Subscribe(_ =>
                {
                    CurrentOperateState = OperateState.Idle;
                });

            //规划状态下获取左键点下时的起始tile
            Observable.EveryUpdate()
                .Where(l => CurrentOperateState == OperateState.Planning)
                .Where(l => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    _cellPlanningStart = CurrentCell;
                    Debug.Log("Start = "+_cellPlanningStart);
                });

            //规划状态下获取左键抬起时的结束tile
            Observable.EveryUpdate()
                .Where(l => CurrentOperateState == OperateState.Planning)
                .Where(l => Input.GetMouseButtonUp(0))
                .Subscribe(_ =>
                {
                    _cellPlanningEnd = CurrentCell;
                    Debug.Log("End = " + _cellPlanningEnd);
                });
        }
    }
}