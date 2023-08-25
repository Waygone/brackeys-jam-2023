public static class Utils
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
    }

    public static int[][] Directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };
}