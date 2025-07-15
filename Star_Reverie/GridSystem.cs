using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using Stride.Engine;
using System;

namespace Star_Reverie
{
    public class GridSystem
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Length { get; private set; }
        private float cellSize;
        private GridObject[,,] gridObjectArray;

        public GridSystem(int width, int height, int length, float cellSize)
        {
            Width = width;
            Height = height;
            Length = length;
            this.cellSize = cellSize;

            gridObjectArray = new GridObject[width, height, length];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        GridPosition gridPosition = new GridPosition(x, y, z);
                        gridObjectArray[x,y,z] = new GridObject(this, gridPosition);
                    }
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition) =>
            new Vector3(gridPosition.x, gridPosition.y, gridPosition.z) * cellSize;

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition
            (
                Convert.ToInt32(worldPosition.X / cellSize),
                Convert.ToInt32(worldPosition.Y / cellSize),
                Convert.ToInt32(worldPosition.Z / cellSize));
        }
        public GridObject GetGridObject(GridPosition gridPosition) =>
            gridObjectArray[gridPosition.x, gridPosition.y, gridPosition.z];

        public void CreateDebugObjects(Entity debugObject)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        Entity clone = debugObject.Clone();
                        clone.Transform.Position = GetWorldPosition(new GridPosition(x, y, z));
                        debugObject.Scene.Entities.Add(clone);
                    }
                }
            }
        }

    }
}
