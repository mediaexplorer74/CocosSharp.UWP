using System;

namespace CocosSharp
{
    public class CCEaseOut : CCEaseRateAction
    {
        #region Constructors

        public CCEaseOut (CCFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseOutState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseOut ((CCFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    public class CCEaseOutState : CCEaseRateActionState
    {
        public CCEaseOutState (CCEaseOut action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update ((float)(Math.Pow (time, 1 / Rate)));      
        }
    }

    #endregion Action state
}