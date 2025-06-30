
namespace Star_Reverie.Globals
{
    public enum GameState
    {
        Paused,
        Encounter,
        Exploration,
        Dialogue,
        Scene
    }

    public static class CurrentGameState
    {
        public static GameState GameState = GameState.Exploration;
    }
}
