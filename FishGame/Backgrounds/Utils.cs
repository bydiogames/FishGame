

using Microsoft.Xna.Framework;

namespace FishGame.Backgrounds
{
    internal static class Utils
    {
        internal static Color GetSeasonalColorMask(Season season, Location location)
        {
            if (location == Location.River || location == Location.Pond)
            {
                switch (season)
                {
                    case Season.Winter: return Color.LightGray;
                    case Season.Fall: return Color.OrangeRed;
                    default: return Color.White;
                }
            }
            else if (location == Location.Ocean)
            {
                switch (season)
                {
                    case Season.Winter: return Color.LightGray;
                    default: return Color.White;
                }
            }

            return Color.White;
        }
    }
}
