using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorFlip : SpriteTestDemo
    {
        const int numOfSprites = 3;
        CCSprite[] sprites;
        CCSprite[] pointSprites;

        CCAnimation animation;
        CCFiniteTimeAction seq;

        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode offset + anchor + flip"; }
        }
        public override string Subtitle
        {
            get { return "issue #1078"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeOffsetAnchorFlip()
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


            CCFlipY flip = new CCFlipY(true);
            CCFlipY flip_back = new CCFlipY(false);
            CCDelayTime delay = new CCDelayTime (1);

            seq = new CCSequence(delay, flip, delay, flip_back);

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

                sprites[i].RunAction(new CCRepeatForever(new CCAnimate(animation)));
                sprites[i].RepeatForever(seq);
            }
        }

        #endregion Setup content
    }
}