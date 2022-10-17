﻿using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCEventListenerMouse : CCEventListener
    {

        public static string LISTENER_ID = "__cc_mouse";

        // Event callback function for Mouse Down events
        public Action<CCEventMouse> OnMouseDown { get; set; }
        // Event callback function for Mouse Up events
        public Action<CCEventMouse> OnMouseUp { get; set; }
        // Event callback function for Mouse Move events
        public Action<CCEventMouse> OnMouseMove { get; set; }
        // Event callback function for Mouse Scroll events
        public Action<CCEventMouse> OnMouseScroll { get; set; }

		public override bool IsAvailable {
			get {
				return true;
			}
		}

        public CCEventListenerMouse() : base(CCEventListenerType.MOUSE, LISTENER_ID)
        {
			// Set our call back action to be called on mouse events so they can be 
			// propagated to the listener.
            Action<CCEvent> listener = mEvent =>
                {
                    var mouseEvent = (CCEventMouse)mEvent;
                    switch (mouseEvent.MouseEventType)
                    {
                        case CCMouseEventType.MOUSE_DOWN:
                            if (OnMouseDown != null)
                                OnMouseDown(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_UP:
                            if (OnMouseUp != null)
                                OnMouseUp(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_MOVE:
                            if(OnMouseMove != null)
                                OnMouseMove(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_SCROLL:
                            if(OnMouseScroll != null)
                                OnMouseScroll(mouseEvent);
                            break;
                        default:
                            break;
                    }

                };
            OnEvent = listener;
        }

		internal CCEventListenerMouse(CCEventListenerMouse mouse)
			: this()
		{
			OnMouseDown = mouse.OnMouseDown;
			OnMouseMove = mouse.OnMouseMove;
			OnMouseUp = mouse.OnMouseUp;
			OnMouseScroll = mouse.OnMouseScroll;
		}

		public override CCEventListener Copy()
		{
			return new CCEventListenerMouse (this);
		}
    }
}
