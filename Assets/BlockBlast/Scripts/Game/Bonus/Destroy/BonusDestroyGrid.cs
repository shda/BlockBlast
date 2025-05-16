using System;
using BlockBlast.Scripts.Game.Bonus.Rotate;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlockBlast.Scripts.Game.Bonus.Destroy
{
    public class BonusDestroyGrid : BonusBase
    {
        [SerializeField] private Vector2Int destroySize = new Vector2Int(3, 3);

        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private Camera cam;
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;

        [SerializeField] private Transform dragRect;

        private bool _isDrag;

        public override void StartBonus()
        {
            dragRect.gameObject.SetActive(true);
            dragShapeHandler.IsDragDisable = true;
            _isDrag = true;
        }

        public override void StopBonus()
        {
            dragRect.gameObject.SetActive(false);
            dragShapeHandler.IsDragDisable = false;
            _isDrag = false;
        }

        public override bool IsCanUsingBonus()
        {
            return bonusCountData.CountDestroyBonus.Value > 0;
        }

        private void Update()
        {
            if (_isDrag)
            {
                if (Input.touchPressureSupported)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        UpdatePositonDragRect();
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        CompleteMoving();
                    }
                }
                else
                {
                    UpdatePositonDragRect();
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        CompleteMoving();
                    }
                }
            }
        }

        private void CompleteMoving()
        {
            var mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            var point = gameBoard.GetCoordNearestPointByWorldPoint(mouseWorldPos);
            
            bool isChange = DestroyCrystalsByStartPoint(point - new Vector2Int(1, 1), destroySize);
            StopBonus();
            OnComplete?.Invoke();

            if (isChange)
            {
                bonusCountData.CountDestroyBonus.Value--;
            }
        }


        private void UpdatePositonDragRect()
        {
            var mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            var startGrid = boardGridSettings.transform.position;
            var point = gameBoard.GetCoordNearestPointByWorldPoint(mouseWorldPos);

            var position = startGrid + (Vector3)(point * boardGridSettings.GridStep);

            dragRect.transform.position = position;
        }

        private bool  DestroyCrystalsByStartPoint(Vector2Int startPos, Vector2Int size)
        {
            bool isChange = false;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var coord = startPos + new Vector2Int(x, y);
                    var crystal = gameBoard.GetCrystalToPoint(coord);
                    if (crystal)
                    {
                        isChange = true;
                        crystal.PlayAnimationHide(() =>
                        {
                            crystalPool.Release(crystal);
                            crystal.transform.localScale = Vector3.one;
                        });

                        gameBoard.ClearPoint(crystal.Coordinates);
                    }
                }
            }
            
            return isChange;
        }
    }
}