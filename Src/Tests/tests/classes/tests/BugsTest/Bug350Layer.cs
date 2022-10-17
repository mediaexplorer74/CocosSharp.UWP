using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Bug350Layer : BugsTestBaseLayer
    {

        public Bug350Layer()
        {
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            CCSprite background = new CCSprite("Hello");
            background.Position = new CCPoint(size.Width / 2, size.Height / 2);
            AddChild(background);

        }
    }
}
