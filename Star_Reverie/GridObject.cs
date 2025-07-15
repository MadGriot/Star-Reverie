using StarReverieCore.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie
{
    public class GridObject
    {
        private GridSystem gridSystem;
        private GridPosition gridPosition;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
        }
    }
}
