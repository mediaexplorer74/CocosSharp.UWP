﻿using System;

namespace CocosSharp
{
    // CCNodeGrid allows the hosting of a target node that will have a CCGridAction effect applied to it.
    public class CCNodeGrid : CCNode
    {
        CCGridBase grid;
        CCCustomCommand renderGrid;

        bool disposed;

        #region Properties

        public CCNode Target { get; set; }

        public CCGridBase Grid 
        { 
            get { return grid; }
            set 
            {
                if (grid != null)
                    grid.Dispose();
                
                grid = value;
                if(value != null) 
                {
                    grid.Scene = Scene;
                    grid.Layer = Layer;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCNodeGrid() 
            : this(CCSize.Zero)
        {
        }

        public CCNodeGrid(CCSize contentSize) 
            : base(contentSize)
        {
            renderGrid = new CCCustomCommand(RenderGrid);
        }

        #endregion Constructors


        #region Clean up

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);

            if (disposed)
                return;

            if (disposing)
            {
                if (grid != null)
                    grid.Dispose();
            }

            disposed = true;
        }

        #endregion Clean up


        public override void Visit (ref CCAffineTransform parentWorldTransform)
        {
            if (!Visible || Scene == null)
                return;

            var worldTransform = CCAffineTransform.Identity;
            var affineLocalTransform = AffineLocalTransform;
            CCAffineTransform.Concat(ref affineLocalTransform, ref parentWorldTransform, out worldTransform);


            if (Grid != null && Grid.Active)
                Grid.BeforeDraw();

            SortAllChildren();

            VisitRenderer(ref worldTransform);

            if (Target != null)
                Target.Visit (ref worldTransform);
            
            if(Children != null)
            {
                var elements = Children.Elements;
                for(int i = 0, N = Children.Count; i < N; ++i)
                {
                    var child = elements[i];
                    if (child.Visible)
                        child.Visit(ref worldTransform);
                }
            }

            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
                Renderer.AddCommand(renderGrid);
            }
        }
            
        void RenderGrid ()
        {
            if (Grid != null && Grid.Active)
                Grid.Blit();
        }
    }
}

