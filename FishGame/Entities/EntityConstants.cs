using System.IO;

namespace FishGame.Entities
{
    public static class EntityConstants
    {
        // For a default resolution of 800 x 480 with tile size 16 x 16
        public static int ScreenWidthTiles = 50;
        public static int ScreenHeightTiles = 30;

        public static int TileWidthPx = 16;
        public static int TileHeightPx = 16;

        // Character cell size = 160 x 128px
        public static int CharacterWidthTiles = 5;
        public static int CharacterHeightTiles = 5;
        public static int CharacterFishingYOffsetPx = 8;
        public static int CharacterXOffsetPx = -8;
        public static int CharacterLocationXTiles = 8;
        public static int CharacterLocationYTiles = 10;

        public static int FishShadowWidthTiles = 2;
        public static int FishShadowHeightTiles = 2;
        public static int FishShadowLocationXTiles = 9;
        public static int FishShadowLocationYTiles = 16;

        public static int EmoteWidthTiles = 2;
        public static int EmoteHeightTiles = 2;

        // Environment constants
        public static int DockHeightTiles = 20;
        public static int DockWidthTiles = 15;

        public static int BackgroundWidthTiles = 20;
        public static int BackgroundHeightTiles = 26;
    }
}
