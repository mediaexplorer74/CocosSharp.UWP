using System;

namespace CocosSharp
{
    public class CCEaseExponentialInOut : CCActionEase
    {
        #region Constructors

        public CCEaseExponentialInOut (CCFiniteTimeAction action) : base(action)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseExponentialInOutState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseExponentialInOut ((CCFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class CCEaseExponentialInOutState : CCActionEaseState
    {
        public CCEaseExponentialInOutState (CCEaseExponentialInOut action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (CCEaseMath.ExponentialInOut (time));
        }
    }

    #endregion Action state
}