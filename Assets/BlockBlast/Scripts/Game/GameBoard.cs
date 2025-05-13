using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private Vector2Int gridSize;
        
        public Crystal[,] Crystals { get; private set; }
        
        private List<Crystal> tempCrystals = new();
        private bool _isCanSetShape;

        private void Awake()
        {
            Crystals = new Crystal[boardGridSettings.GridSize.x, boardGridSettings.GridSize.y];
        }

        public void RemoveTempShape()
        {
            foreach (var tempCrystal in tempCrystals)
            {
                crystalPool.Release(tempCrystal);
            }

            tempCrystals.Clear();
        }

        public void SetShapeFromTemp()
        {
            foreach (var tempCrystal in tempCrystals)
            {
                var coord = tempCrystal.Coordinates;
                Crystals[coord.x, coord.y] = SetCristal(new Vector2Int(coord.x, coord.y), tempCrystal.transform.position);
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
                    Debug.DrawLine(position, Vector3.zero, Color.red);

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
                    
                    tempCrystals.Add(SetCristal(new Vector2Int(positionInGrid.x, positionInGrid.y), position));
                }
            }

            return true;
        }

        private Crystal SetCristal(Vector2Int point, Vector3 position)
        {
            var newCrystal = crystalPool.Get();
            newCrystal.transform.position = position;
            newCrystal.Coordinates = new Vector2Int(point.x, point.y);
            return newCrystal;
        }

        public void ClearPoint(Vector2Int coordinates)
        {
            Crystals[coordinates.x, coordinates.y] = null;
        }
    }
}