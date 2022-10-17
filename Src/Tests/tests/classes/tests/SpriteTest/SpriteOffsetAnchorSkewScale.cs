using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteOffsetAnchorSkewScale : SpriteTestDemo
    {
        const int numOfSprites = 3;

        CCSprite[] sprites;
        CCSprite[] pointSprites;

        CCAnimation animation;
        CCFiniteTimeAction seq_skew;
        CCFiniteTimeAction seq_scale;

        #region Properties

        public override string Title
        {
            get { return "Sprite anchor + skew + scale"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteOffsetAnchorSkewScale()
        {
            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            // Create actions

            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");
            var animFrames = new List<CCSpriteFrame>();
            string tmp = "";
            for (int j = 0; j < 14; j++)
            {
                tmp = string.Format("grossini_dance_{0:00}.png", j + 1);
                CCSpriteFrame frame = cache[tmp];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            // Skew
            CCSkewBy skewX = new CCSkewBy (2, 45, 0);
            CCFiniteTimeAction skewX_back = (CCFiniteTimeAction)skewX.Reverse();
            CCSkewBy skewY = new CCSkewBy (2, 0, 45);
            CCFiniteTimeAction skewY_back = (CCFiniteTimeAction)skewY.Reverse();

            seq_skew = new CCSequence(skewX, skewX_back, skewY, skewY_back);

            // Scale
            CCScaleBy scale = new CCScaleBy(2, 2);
            CCFiniteTimeAction scale_back = (CCFiniteTimeAction)scale.Reverse();

            seq_scale = new CCSequence(scale, scale_back);


            for (int i = 0; i < numOfSprites; i++)
            {
                // Animation using Sprite batch
                sprites[i] = new CCSprite("grossini_dance_01.png");
                AddChild(sprites[i], 0);

                pointSprites[i] = new CCSprite("Images/r1");
                AddChild(pointSprites[i], 1);
            }
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            for(int i = 0; i < numOfSprites; i++) 
            {
                sprites[i].Position = (new CCPoint(windowSize.Width / 4 * (i + 1), windowSize.Height / 2));
                pointSprites[i].Scale = 0.25f;
                pointSprites[i].Position = sprites[i].Position;

                switch(i)
                {
                    case 0:
                        sprites[i].AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprites[i].AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprites[i].AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                sprites[i].RunAction(new CCRepeatForever(new CCAnimate(animation)));
                sprites[i].RunAction(new CCRepeatForever ((CCFiniteTimeAction)seq_skew));
                sprites[i].RunAction(new CCRepeatForever ((CCFiniteTimeAction)seq_scale));
            }
        }

        #endregion Setup content
    }
}
