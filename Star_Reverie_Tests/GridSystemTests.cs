using Star_Reverie;
using StarReverieCore.Grid;
using Stride.Core.Mathematics;

namespace Star_Reverie_Tests
{
    public class GridSystemTests
    {
        [Fact]
        public void Constructor_ShouldInitializeGridCorrectly()
        {
            GridSystem gridSystem = new GridSystem(3, 2, 4, 1.5f);

            Assert.Equal(3, gridSystem.Width);
            Assert.Equal(2, gridSystem.Height);
            Assert.Equal(4, gridSystem.Length);

            GridPosition gridPosition = new GridPosition(1, 1, 1);
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);

            Assert.NotNull(gridObject);
        }

        [Theory]
        [InlineData(0, 0, 0, 1f)]
        [InlineData(2, 3, 4, 2f)]
        public void GetWorldPosition_ShouldReturnCorrectVector(int x, int y, int z, float cellSize)
        {
            GridSystem gridSystem = new GridSystem(10, 10, 10, cellSize);
            GridPosition gridPosition = new GridPosition(x, y, z);
            Vector3 worldPosition = gridSystem.GetWorldPosition(gridPosition);

            Vector3 expected = new Vector3(x, y, z) * cellSize;
            Assert.Equal(expected, worldPosition);
        }


        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(5, 5, 5, true)]
        [InlineData(-1, 0, 0, false)]
        [InlineData(0, -1, 0, false)]
        [InlineData(10, 10, 10, false)]
        public void IsValidGridPosition_ShouldValidateCorrectly(int x, int y, int z, bool expectedResult)
        {
            GridSystem gridSystem = new GridSystem(10, 10, 10, 1.0f);
            GridPosition gridPosition = new GridPosition(x, y, z);
            bool isValid = gridSystem.IsValidGridPosition(gridPosition);

            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void GetGridObject_ShouldReturnCorrectObject()
        {
            GridSystem gridSystem = new GridSystem(5, 5, 5, 1.0f);
            GridPosition gridPosition = new GridPosition(2, 2, 2);
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);

            Assert.NotNull(gridObject);
        }
    }
}
