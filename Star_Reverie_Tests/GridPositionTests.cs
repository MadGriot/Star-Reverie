using StarReverieCore.Grid;

namespace Star_Reverie_Tests
{
    public class GridPositionTests
    {
        [Fact]
        public void Equals_WithSameValues_ShouldBeTrue()
        {
            var a = new GridPosition(1, 2, 3);
            var b = new GridPosition(1, 2, 3);

            Assert.True(a == b);
            Assert.False(a != b);
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
        }

        [Fact]
        public void Equals_WithDifferentValues_ShouldBeFalse()
        {
            var a = new GridPosition(1, 2, 3);
            var b = new GridPosition(3, 2, 1);

            Assert.False(a == b);
            Assert.True(a != b);
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
        }

        [Fact]
        public void Operator_Addition_ShouldSumCoordinates()
        {
            var a = new GridPosition(1, 2, 3);
            var b = new GridPosition(4, 5, 6);
            var result = a + b;

            Assert.Equal(new GridPosition(5, 7, 9), result);
        }

        [Fact]
        public void Operator_Subtraction_ShouldSubtractCoordinates()
        {
            var a = new GridPosition(5, 7, 9);
            var b = new GridPosition(1, 2, 3);
            var result = a - b;

            Assert.Equal(new GridPosition(4, 5, 6), result);
        }

        [Fact]
        public void GetHashCode_WithEqualObjects_ShouldBeSame()
        {
            var a = new GridPosition(1, 2, 3);
            var b = new GridPosition(1, 2, 3);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var pos = new GridPosition(1, 2, 3);
            var expected = "x: 1; y: 2; z: 3";

            Assert.Equal(expected, pos.ToString());
        }
    }
}