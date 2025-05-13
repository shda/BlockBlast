using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    [Serializable]
    public class BoolRow
    {
        public List<int> row = new List<int>();
    }

    public class BoolGrid : MonoBehaviour
    {
        public int width = 5;
        public int height = 5;

        public int this[int x, int y]
        {
            get => grid[y].row[x];
            set => grid[y].row[x] = value;
        }
    
        [SerializeField]
        private List<BoolRow> grid = new();

        public List<BoolRow> Grid => grid;

        private void OnValidate()
        {
            // Автоматически корректируем размеры при изменении
            ResizeGrid();
        }

        public void ResizeGrid()
        {
            // Убедимся, что список имеет нужную высоту
            while (grid.Count < height)
                grid.Add(new BoolRow());
            while (grid.Count > height)
                grid.RemoveAt(grid.Count - 1);

            // Убедимся, что каждая строка имеет нужную ширину
            foreach (var row in grid)
            {
                while (row.row.Count < width)
                    row.row.Add(0);
                while (row.row.Count > width)
                    row.row.RemoveAt(row.row.Count - 1);
            }
        }
    }
}