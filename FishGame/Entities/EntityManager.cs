using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Entities
{
    internal sealed class EntityManager : IGameComponent, IUpdateable, IDrawable
    {
        private readonly ContentManager contentManager;

        public EntityManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        private readonly List<IEntity> entities = new List<IEntity>();

        public bool Enabled => true;

        public int UpdateOrder => 0;

        int IDrawable.DrawOrder => 0;

        bool IDrawable.Visible => true;

        public SpriteBatch Sb { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;


        void IDrawable.Draw(GameTime gameTime)
        {
            Sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            foreach (var entity in entities.OrderBy((entity) => { return entity.UpdateOrder; }))
            {
                entity.Draw(Sb);
            }
            Sb.End();
        }

        void IGameComponent.Initialize()
        {
            entities.Clear();
        }

        void IUpdateable.Update(GameTime gameTime)
        {
            foreach (var entity in entities.OrderBy((entity) => { return entity.UpdateOrder; }))
            {
                entity.Update(gameTime);
            }
        }

        public void AddEntity(IEntity entity) 
        {
            entities.Add(entity);
        }

        public void DestroyEntity(IEntity entity)
        { 
            entities.Remove(entity);
        }
    }
}
