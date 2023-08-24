public static class GlobalData
{
    public enum GameState
    {
        MENU = 0,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_1_END_GAME,
    }

    public static GameState State;

    public static bool TrySetState(GameState state, bool shoud_check = true)
    {
        if (shoud_check)
        {
            if ((int)State + 1 != (int)state)
            {
                return false;
            }
        }

        State = state;
        return true;
    }
}