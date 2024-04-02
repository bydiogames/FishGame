

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
                    case Season.Spring: return Color.MediumSpringGreen;
                    case Season.Summer: return Color.LawnGreen;
                    case Season.Fall: return Color.Goldenrod;
                    case Season.Winter: return Color.FloralWhite;
                    default: return Color.White;
                }
            }
            else if (location == Location.Ocean)
            {
                switch (season)
                {
                    case Season.Spring: return Color.SandyBrown;
                    case Season.Summer: return Color.SandyBrown;
                    case Season.Fall: return Color.SandyBrown;
                    case Season.Winter: return Color.GhostWhite;
                    default: return Color.White;
                }
            }

            return Color.White;
        }
    }
}
