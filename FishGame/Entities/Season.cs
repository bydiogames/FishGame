
using System;
using System.Numerics;

namespace FishGame.Entities
{
    [Flags]
    public enum Season
    {
        None = 0,
        Spring = 1,
        Summer = 2,
        Fall = 4,
        Winter = 8,
        All = Spring | Summer | Fall | Winter,
    }

    public static class SeasonUtils
    {
        public static readonly string[] Names = new string[] 
        {
            "Spring",
            "Summer",
            "Fall",
            "Winter"
        };

        public static string GetName(this Season season)
        {
            return Names[BitOperations.Log2((uint)season)];
        }
    }
}
