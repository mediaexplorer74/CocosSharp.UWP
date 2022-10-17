using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorSkew : SpriteTestDemo
    {
        const int numOfSprites = 3;
        CCSprite[] sprites;
        CCSprite[] pointSprites;

        CCAnimation animation;
        CCFiniteTimeAction seq_scale;
        CCFiniteTimeAction seq_skew;


        #region Properties

        public override string Title
        {
            get { return "Testing SpriteBatchNode"; }
        }

        public override string Subtitle
        {
            get
            {
                return "offset + anchor + skew";
            }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeOffsetAnchorSkew()
        {
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");

            // Create animations and actions

            var animFrames = new List<CCSpriteFrame>();
            string tmp = "";
            for (int j = 0; j < 14; j++)
            {
                string temp = "";
                if (j+1<10)
                {
                    temp = "0" + (j + 1);
                }
                else
                {
                    temp = (j + 1).ToString();
                }
                tmp = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = cache[tmp];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            // skew
            CCSkewBy skewX = new CCSkewBy (2, 45, 0);
            CCFiniteTimeAction skewX_back = (CCFiniteTimeAction)skewX.Reverse();
            CCSkewBy skewY = new CCSkewBy (2, 0, 45);
            CCFiniteTimeAction skewY_back = (CCFiniteTimeAction)skewY.Reverse();

            // scale 
            CCScaleBy scale = new CCScaleBy(2, 2);
            CCFiniteTimeAction scale_back = (CCFiniteTimeAction)scale.Reverse();

            seq_scale = new CCSequence(scale, scale_back);
            seq_skew = new CCSequence(skewX, skewX_back, skewY, skewY_back);

            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            for (int i = 0; i < numOfSprites; i++)
            {
                // Animation using Sprite batch
                sprites[i] = new CCSprite("grossini_dance_01.png");
                pointSprites[i] = new CCSprite("Images/r1");

                CCSpriteBatchNode spritebatch = new CCSpriteBatchNode("animations/grossini");
                AddChild(spritebatch);
                AddChild(pointSprites[i], 200);
                spritebatch.AddChild(sprites[i], i);
            }
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            for (int i = 0; i < numOfSprites; i++) 
            {
                sprites[i].Position = new CCPoint(windowSize.Width / 4 * (i + 1), windowSize.Height / 2);
                pointSprites[i].Scale = 0.25f;
                pointSprites[i].Position = sprites[i].Position;

                switch(i)
                {
                case 0:
                    sprites[i].AnchorPoint = new CCPoint(0, 0);
                    break;
                case 1:
                    sprites[i].AnchorPoint = new CCPoint(0.5f, 0.5f);
                    break;
                case 2:
                    sprites[i].AnchorPoint = new CCPoint(1, 1);
                    break;
                }

                sprites[i].RunAction(new CCRepeatForever(new CCAnimate (animation)));
                sprites[i].RunAction(new CCRepeatForever((CCFiniteTimeAction)seq_skew));
            }
        }

        #endregion Setup content
    }
}
