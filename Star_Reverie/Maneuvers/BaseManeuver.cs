using StarReverieCore.Grid;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie.Maneuvers
{
    public abstract class BaseManeuver : SyncScript
    {
        public Entity Actor { get; set; }
        public ActorActionSystem ActionSystem { get; set; }
        public LevelGrid LevelGrid { get; set; }
        public string Name { get; protected set; } = "Maneuver";
        internal bool isActive;
        internal bool IsOffensive;
        protected Action OnActionComplete;

        public BaseManeuver()
        {

        }
        public override void Update()
        {

        }

        public abstract void ActivateManeuver(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidManeuverGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidManeuverGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidManeuverGridPositionList();

    }
}
