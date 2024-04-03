
using System;

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
}
