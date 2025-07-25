using Star_Reverie;
using StarReverieCore.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie_Tests
{
    public class GridObjectTests
    {
        [Fact]
        public void Constructor_ShouldInitializeActorList()
        {
            GridSystem gridSystem = new GridSystem(5, 5, 5, 1.0f);
            GridPosition gridPosition = new GridPosition(1, 1, 1);
            GridObject gridObject = new GridObject(gridSystem, gridPosition);

            Assert.NotNull(gridObject.ActorList);
            Assert.Empty(gridObject.ActorList);
        }

        [Fact]
        public void ActorList_ShouldAddAndRemoveActors()
        {
            GridSystem gridSystem = new GridSystem(5, 5, 5, 1.0f);
            GridPosition gridPosition = new GridPosition(2, 2, 2);
            GridObject gridObject = new GridObject(gridSystem, gridPosition);

            Actor actor1 = new Actor();
            Actor actor2 = new Actor();

            gridObject.ActorList.Add(actor1);
            gridObject.ActorList.Add(actor2);

            Assert.Equal(2, gridObject.ActorList.Count);
            Assert.Contains(actor1, gridObject.ActorList);
            Assert.Contains(actor2, gridObject.ActorList);

            gridObject.ActorList.Remove(actor1);

            Assert.Single(gridObject.ActorList);
            Assert.DoesNotContain(actor1, gridObject.ActorList);
        }
    }
}
