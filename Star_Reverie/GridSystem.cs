using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie
{
    public class GridSystem
    {
        private int width;
        private int height;
        private int length;
        private float cellSize;

        public GridSystem(int width, int height, int length, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.length = length;
            this.cellSize = cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition
            (
                Convert.ToInt32(worldPosition.X / cellSize),
                Convert.ToInt32(worldPosition.Y / cellSize),
                Convert.ToInt32(worldPosition.Z / cellSize)
                );
        }
    }
}
