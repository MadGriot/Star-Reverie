using System.Collections.Generic;
using Stride.Engine;
using StarReverieCore.Grid;

namespace Star_Reverie
{
    public class LevelGrid : SyncScript
    {
        public Entity GridDebugObject;

        public GridSystem GridSystem { get; set; }

        public override void Start()
        {
            GridSystem = new GridSystem(10, 1, 10, 2f);
            GridSystem.CreateDebugObjects(GridDebugObject);
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void AddActorAtGridPosition(GridPosition gridPosition, Actor actor)
        {
            GridObject gridObject = GridSystem.GetGridObject(gridPosition);
            gridObject.ActorList.Add(actor);
        }

        public void ActorMovedGridPosition(Actor actor, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveActorGridPosition(fromGridPosition, actor);
            AddActorAtGridPosition(toGridPosition, actor);
        }

        public List<Actor> GetActorListAtGridPosition(GridPosition gridPosition, Actor actor)
        {
            GridObject gridObject = GridSystem.GetGridObject(gridPosition);
            return gridObject.ActorList;
        }

        public void RemoveActorGridPosition(GridPosition gridPosition, Actor actor)
        {
            GridObject gridObject = GridSystem.GetGridObject(gridPosition);
            gridObject.ActorList.Remove(actor);
        }

        public bool HasAnyActorOnGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = GridSystem.GetGridObject(gridPosition);
            return gridObject.ActorList.Count > 0;
        }
    }
}
