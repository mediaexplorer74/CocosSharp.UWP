using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutTRTilesDemo
    {
		public static CCFiniteTimeAction ActionWithDuration(float t)
        {
            CCFadeOutTRTiles fadeout = new CCFadeOutTRTiles(t, new CCGridSize(16, 12));
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

			return new CCSequence(fadeout, delay, back);
        }

    }
}
