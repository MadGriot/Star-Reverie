using Star_Reverie;
using StarReverieCore.Grid;

namespace Star_Reverie_Tests
{
    public class LevelGridTests
    {
        [Fact]
        public void AddActorAtGridPosition_ShouldAddActor()
        {
            LevelGrid levelGrid = CreateLevelGrid();
            GridPosition gridPosition = new GridPosition(1, 0, 1);
            Actor actor = new Actor();

            levelGrid.AddActorAtGridPosition(gridPosition, actor);

            List<Actor> actors = levelGrid.GetActorListAtGridPosition(gridPosition, actor);
            Assert.Single(actors);
            Assert.Contains(actor, actors);
        }

        [Fact]
        public void RemoveActorGridPosition_ShouldRemoveActor()
        {
            LevelGrid levelGrid = CreateLevelGrid();
            GridPosition gridPosition = new GridPosition(2, 0, 2);
            Actor actor = new Actor();

            levelGrid.AddActorAtGridPosition(gridPosition, actor);
            levelGrid.RemoveActorGridPosition(gridPosition, actor);

            List<Actor> actors = levelGrid.GetActorListAtGridPosition(gridPosition, actor);
            Assert.Empty(actors);
        }

        [Fact]
        public void ActorMovedGridPosition_ShouldMoveActorCorrectly()
        {
            LevelGrid levelGrid = CreateLevelGrid();
            Actor actor = new Actor();
            GridPosition from = new GridPosition(3, 0, 3);
            GridPosition to = new GridPosition(4, 0, 4);

            levelGrid.AddActorAtGridPosition(from, actor);
            levelGrid.ActorMovedGridPosition(actor, from, to);

            List<Actor> fromList = levelGrid.GetActorListAtGridPosition(from, actor);
            List<Actor> toList = levelGrid.GetActorListAtGridPosition(to, actor);

            Assert.Empty(fromList);
            Assert.Single(toList);
            Assert.Contains(actor, toList);
        }

        [Fact]
        public void HasAnyActorOnGridPosition_ShouldReturnTrueIfActorsPresent()
        {
            LevelGrid levelGrid = CreateLevelGrid();
            GridPosition gridPosition = new GridPosition(5, 0, 5);
            Actor actor = new Actor();

            levelGrid.AddActorAtGridPosition(gridPosition, actor);
            bool result = levelGrid.HasAnyActorOnGridPosition(gridPosition);

            Assert.True(result);
        }

        [Fact]
        public void HasAnyActorOnGridPosition_ShouldReturnFalseIfNoActors()
        {
            LevelGrid levelGrid = CreateLevelGrid();
            GridPosition emptyPosition = new GridPosition(6, 0, 6);

            bool result = levelGrid.HasAnyActorOnGridPosition(emptyPosition);
            Assert.False(result);
        }

        // Utility method to create a testable LevelGrid instance
        private LevelGrid CreateLevelGrid()
        {
            LevelGrid levelGrid = new LevelGrid();
            levelGrid.GridSystem = new GridSystem(10, 1, 10, 2f); // manually assign to avoid engine context
            return levelGrid;
        }
    }
}
