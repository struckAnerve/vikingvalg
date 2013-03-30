using System;
using System.Collections.Generic;
using System.Text;

namespace XnaDisplay
{
    public class XnaDisplay : GraphicsDeviceControl
    {
        public EventHandler InitializeHandler;
        public EventHandler DrawHandler;

        protected override void Initialize()
        {
            if (InitializeHandler != null)
            {
                InitializeHandler(this, null);
            }
        }

        protected override void Draw()
        {
            if (DrawHandler != null)
            {
                DrawHandler(this, null);
            }
        }
    }
}
