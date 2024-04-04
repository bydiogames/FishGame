using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Interface
{
    internal class MainUI : IGameComponent, IDrawable
    {
        private SpriteBatch _spriteBatch;
        private TestBackgroundManager _background;
        private FishJournal _fishJournal;

        public MainUI(SpriteBatch spriteBatch, TestBackgroundManager background, FishJournal fishJournal)
        {
            _spriteBatch = spriteBatch;
            _background = background;
            _fishJournal = fishJournal;
        }

        private Texture2D _mainUiOverlayTex, _mainUiTilesTex;
        private Texture2D _fishAllTex, _fishAllMissingTex;

        private Texture2D _seasonCardsTex;

        private static readonly int[] SeasonCardIdxForSeason = new int[] 
        { 
            1,
            2,
            3,
            0
        };

        public void Load(ContentManager content)
        {
            // Load fish texture maps.
            {
                _fishAllTex = content.Load<Texture2D>("fish_all");
                _fishAllMissingTex = content.Load<Texture2D>("inv_fish_shadow");
            }

            {
                _mainUiOverlayTex = content.Load<Texture2D>("Main_ui__Ui_tiles");
                _mainUiTilesTex = content.Load<Texture2D>("Main_ui__Tiles");
            }

            _seasonCardsTex = content.Load<Texture2D>("Season_Cards__Tiles");
        }

        int IDrawable.DrawOrder => 50;

        bool IDrawable.Visible => true;

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        void IDrawable.Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_mainUiTilesTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_mainUiOverlayTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            _fishJournal.Draw(
                _spriteBatch,
                _fishAllMissingTex,
                _fishAllTex,
                _spriteBatch.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.1f),
                _background.GetSeason(),
                _background.GetLocation()
            );

            {
                var seasonCardIdx = SeasonCardIdxForSeason[System.Numerics.BitOperations.Log2((uint)_background.GetSeason())];
                var seasonCardXOffset = seasonCardIdx * 32 * 2;

                var seasonCardSrcRect = new Rectangle(seasonCardXOffset, 0, 32, 16 * 3);
                var seasonCardDstRect = new Rectangle(160 * 2, 185 * 2, 32 * 2, 16 * 3 * 2 + 1);
                _spriteBatch.Draw(_seasonCardsTex, seasonCardDstRect, seasonCardSrcRect, Color.White);
            }

            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
