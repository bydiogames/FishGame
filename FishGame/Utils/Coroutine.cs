using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Utils
{
    public sealed class Coroutine
    {
        internal IEnumerator<IWaitable> routine;

        internal bool isRunning = true;
        internal IWaitable lastWaitable;

        public void Cancel()
        {
            isRunning = false;
        }
    }

    public interface IWaitable
    {
        public bool Update(GameTime time);
    }

    public sealed class Wait : IWaitable
    {
        private readonly TimeSpan waitingTime;
        private TimeSpan passedTime;

        public Wait( TimeSpan time )
        { 
            waitingTime = time;
        }

        public bool Update(GameTime time)
        {
            passedTime += time.ElapsedGameTime;
            return passedTime < waitingTime;
        }
    }

    public class CoroutineManager : IGameComponent, IUpdateable
    {
        private readonly List<Coroutine> allRoutines = new List<Coroutine>();

        public bool Enabled => true;

        public int UpdateOrder => 0;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public void Initialize()
        {
            allRoutines.Clear();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < allRoutines.Count; i++)
            {
                Coroutine routine = allRoutines[i];

                if (!routine.isRunning)
                {
                    allRoutines.RemoveAt(i);
                    --i;
                }

                if (routine.lastWaitable != null && routine.lastWaitable.Update(gameTime))
                    continue;

                routine.lastWaitable = null;

                if (!routine.routine.MoveNext())
                {
                    allRoutines.RemoveAt(i);
                    --i;
                }

                routine.lastWaitable = routine.routine.Current;
            }
        }

        public Coroutine Start(IEnumerator<IWaitable> routine)
        {
            var coro = new Coroutine()
            { 
                routine = routine,
            };

            allRoutines.Add(coro);

            return coro;
        }
    }
}
