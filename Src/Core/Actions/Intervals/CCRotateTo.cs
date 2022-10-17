using System;

namespace CocosSharp
{
    public class CCRotateTo : CCFiniteTimeAction
    {
        public float DistanceAngleX { get; private set; }
        public float DistanceAngleY { get; private set; }


        #region Constructors

        public CCRotateTo (float duration, float deltaAngleX, float deltaAngleY) : base (duration)
        {
            DistanceAngleX = deltaAngleX;
            DistanceAngleY = deltaAngleY;
        }

        public CCRotateTo (float duration, float deltaAngle) : this (duration, deltaAngle, deltaAngle)
        {
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCRotateToState (this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }


    public class CCRotateToState : CCFiniteTimeActionState
    {
        protected float DiffAngleY;
        protected float DiffAngleX;

        protected float DistanceAngleX { get; set; }

        protected float DistanceAngleY { get; set; }

        protected float StartAngleX;
        protected float StartAngleY;

        public CCRotateToState (CCRotateTo action, CCNode target)
            : base (action, target)
        { 
            DistanceAngleX = action.DistanceAngleX;
            DistanceAngleY = action.DistanceAngleY;

            // Calculate X
            StartAngleX = Target.RotationX;
            if (StartAngleX > 0)
            {
                StartAngleX = StartAngleX % 360.0f;
            }
            else
            {
                StartAngleX = StartAngleX % -360.0f;
            }

            DiffAngleX = DistanceAngleX - StartAngleX;
            if (DiffAngleX > 180)
            {
                DiffAngleX -= 360;
            }
            if (DiffAngleX < -180)
            {
                DiffAngleX += 360;
            }

            //Calculate Y: It's duplicated from calculating X since the rotation wrap should be the same
            StartAngleY = Target.RotationY;

            if (StartAngleY > 0)
            {
                StartAngleY = StartAngleY % 360.0f;
            }
            else
            {
                StartAngleY = StartAngleY % -360.0f;
            }

            DiffAngleY = DistanceAngleY - StartAngleY;
            if (DiffAngleY > 180)
            {
                DiffAngleY -= 360;
            }

            if (DiffAngleY < -180)
            {
                DiffAngleY += 360;
            }
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
                Target.RotationX = StartAngleX + DiffAngleX * time;
                Target.RotationY = StartAngleY + DiffAngleY * time;
            }
        }

    }
}