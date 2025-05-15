using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        
        public Crystal[,] Crystals { get; private set; }
        
        private readonly List<Crystal> _tempCrystals = new();
        private bool _isCanSetShape;

        private void Awake()
        {
            Crystals = new Crystal[boardGridSettings.GridSize.x, boardGridSettings.GridSize.y];
        }

        public void RemoveTempShape()
        {
            foreach (var tempCrystal in _tempCrystals)
            {
                crystalPool.Release(tempCrystal);
            }

            _tempCrystals.Clear();
        }

        public void SetShapeFromTemp()
        {
            foreach (var tempCrystal in _tempCrystals)
            {
                var coord = tempCrystal.Coordinates;
                var crystal = SetCristal(new Vector2Int(coord.x, coord.y), tempCrystal.transform.position);
                crystal.SetSprite(tempCrystal.GetSpriteIndex());
                Crystals[coord.x, coord.y] = crystal;
            }

            RemoveTempShape();
        }

        public bool SetTempShape(Shape shape, Vector3 point)
        {
            _isCanSetShape = false;
            RemoveTempShape();

            var gridStep = boardGridSettings.GridStep;
            var startGrid = boardGridSettings.transform.position;
            var pointStartShape = point - startGrid;

            int startPosX = Mathf.RoundToInt(pointStartShape.x / gridStep.x);
            int startPosY = Mathf.RoundToInt(pointStartShape.y / gridStep.y);

            var startPosition = new Vector3(startPosX * gridStep.x, startPosY * gridStep.y, 0);

            for (int y = 0; y < shape.Grid.height; y++)
            {
                for (int x = 0; x < shape.Grid.width; x++)
                {
                    if (shape.Grid[x, y] <= 0)
                        continue;

                    var positionInGrid = new Vector2Int(
                        startPosX + x,
                        startPosY + y);

                    var position = startGrid + startPosition + new Vector3(x * gridStep.x, y * gridStep.y, 0);

                    var outOfBounds =
                        position.x < startGrid.x ||
                        position.y < startGrid.y ||
                        position.x >= startGrid.x + gridStep.x * boardGridSettings.GridSize.x ||
                        position.y >= startGrid.y + gridStep.y * boardGridSettings.GridSize.y - 0.1f;

                    if (outOfBounds || Crystals[positionInGrid.x, positionInGrid.y])
                    {
                        RemoveTempShape();
                        return false;
                    }

                    var newCristal = SetCristal(new Vector2Int(positionInGrid.x, positionInGrid.y) , position);
                    newCristal.SetSprite(shape.CrystalColor);
                    _tempCrystals.Add(newCristal);
                }
            }

            return true;
        }

        public Vector2? FindFreeSpaceOnBoard(Shape shape)
        {
            for (int x = 0; x < boardGridSettings.GridSize.x; x++)
            {
                for (int y = 0; y < boardGridSettings.GridSize.y; y++)
                {
                    if (CheckCanInsertShape(x , y , shape))
                    {
                        return new Vector2(x , y);
                    }
                }
            }

            return null;
        }

        private bool CheckCanInsertShape(int startX , int startY , Shape shape)
        {
            for (int x = 0; x < shape.Size.x; x++)
            {
                for (int y = 0; y < shape.Size.y; y++)
                {
                    var val = shape.Grid[x, y];
                    if(val <= 0)
                        continue;
                            
                    var checkPointX = startX + x;
                    var checkPointY = startY + y;

                    if (checkPointX >= boardGridSettings.GridSize.x || 
                        checkPointY >= boardGridSettings.GridSize.y)
                    {
                        return false;
                    }
                    
                    var crystal = Crystals[checkPointX, checkPointY];
                    if (crystal)
                        return false;

                }
            }

            return true;
        }

        public Vector2Int GetCoordNearestPointByWorldPoint(Vector3 worldPoint)
        {
            var gridStep = boardGridSettings.GridStep;
            var startGrid = boardGridSettings.transform.position;
            var pointStartShape = worldPoint - startGrid;

            var startPosX = Mathf.RoundToInt(pointStartShape.x / gridStep.x);
            var startPosY = Mathf.RoundToInt(pointStartShape.y / gridStep.y);
            
            return new Vector2Int(startPosX , startPosY);
        }
        

        private Crystal SetCristal(Vector2Int point, Vector3 position)
        {
            var newCrystal = crystalPool.Get();
            newCrystal.transform.position = position;
            newCrystal.transform.localScale = Vector3.one;
            newCrystal.Coordinates = new Vector2Int(point.x, point.y);
            return newCrystal;
        }

        public void ClearPoint(Vector2Int coordinates)
        {
            Crystals[coordinates.x, coordinates.y] = null;
        }
        
        public void DebugInitLevel(IntGrid grid)
        {
            for (var x = 0; x < grid.width; x++)
            {
                for (var y = 0; y < grid.height; y++)
                {
                    if (grid[x, y] > 0)
                    {
                        var startGrid = boardGridSettings.transform.position;
                        var point = startGrid + (Vector3) (boardGridSettings.GridStep * new Vector2Int(x, y));
                        var crystal = SetCristal(new Vector2Int(x, y) , point);
                        Crystals[x, y] = crystal;
                    }
                }
            }
        }

        public Crystal GetCrystalToPoint(Vector2Int point)
        {
            if (boardGridSettings.GridSize.x > point.x && 
                boardGridSettings.GridSize.y > point.y && 
                point is { x: >= 0, y: >= 0 })
            {
                return Crystals[point.x, point.y];
            }

            return null;
        }
    }
}