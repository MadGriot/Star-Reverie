using System.Collections.Generic;
using System.Linq;
using Stride.Engine;
using StarReverieCore.Grid;
using Star_Reverie.Maneuvers;

namespace Star_Reverie
{
    public class GridSystemVisual : SyncScript
    {
        public Prefab GridSystemVisualSinglePrefab;
        public LevelGrid LevelGrid;
        public ActorActionSystem ActorActionSystem;

        private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
        private float gridCellOffset = 0.01f;

        public override void Start()
        {
            gridSystemVisualSingleArray = new GridSystemVisualSingle[
                 LevelGrid.GridSystem.Width, LevelGrid.GridSystem.Length];

            for (int x = 0; x < LevelGrid.GridSystem.Width; x++)
            {
                for (int z = 0; z < LevelGrid.GridSystem.Length; z++)
                {
                    Entity gridSystemVisualSingleTransform =
                        GridSystemVisualSinglePrefab.Instantiate().First();
                    gridSystemVisualSingleTransform.Transform.Position = LevelGrid.GridSystem
                        .GetWorldPosition(new GridPosition(x, 0, z));
                    gridSystemVisualSingleTransform.Transform.Position.Y += gridCellOffset;
                    Entity.Scene.Entities.Add(gridSystemVisualSingleTransform);

                    gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.Get<GridSystemVisualSingle>();
                }
            }
        }

        public override void Update()
        {
            UpdateGridVisual();
        }

        public void HideAllGridPositions()
        {
            for (int x = 0; x < LevelGrid.GridSystem.Width;x++)
            {
                for (int z = 0; z < LevelGrid.GridSystem.Length;z++)
                {
                    gridSystemVisualSingleArray[x, z].Hide();
                }
            }
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList)
        {
            foreach (GridPosition gridPosition in gridPositionList)
            {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
            }
        }

        private void UpdateGridVisual()
        {
            HideAllGridPositions();
            Entity selectedActor = ActorActionSystem.Actor;
            ShowGridPositionList(
                selectedActor.Get<MoveManeuver>().GetValidManeuverGridPositionList());
        }
    }
}
