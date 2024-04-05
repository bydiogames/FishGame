using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Events
{
    public class Publisher
    {
        // Declare the delegate (if using non-generic pattern).
        public delegate void AnimationStartedEventHandler(object sender, EventArgs e);

        // Declare the event.
        public event AnimationStartedEventHandler AnimationStartedEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseAnimationStartedEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            AnimationStartedEvent?.Invoke(this, new EventArgs());
        }
    }
}
