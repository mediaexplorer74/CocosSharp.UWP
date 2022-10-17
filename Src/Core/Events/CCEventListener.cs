﻿using System;
using System.Collections.Generic;

namespace CocosSharp
{

    public enum CCEventListenerType
    {
        UNKNOWN,
        TOUCH_ONE_BY_ONE,
        TOUCH_ALL_AT_ONCE,
        KEYBOARD,
        MOUSE,
        ACCELEROMETER,
		GAMEPAD,
        CUSTOM
    }


    /// <summary>
    /// The base class of event listener.
    /// If you need custom listener with a different callback, you need to inherit this class.
    /// For instance, you could refer to EventListenerAcceleration, EventListenerKeyboard, EventListenerTouchOneByOne, EventListenerCustom.
    /// </summary>
    public class CCEventListener : IDisposable
    {

		internal virtual string ListenerID { get; set; }

        /// <summary>
		/// Whether the listener is paused or not
		/// 
		/// The paused state is only used for scene graph priority listeners.
		///  'CCEventDispatcher.ResumeAll(node)` will set the paused state to `false`,
		///  while 'CCEventDispatcher.PauseAll(node)` will set it to `true`.
		/// 
		///  *Note* 1) Fixed priority listeners will never get paused. If a fixed priority doesn't want to receive events,
		///           call `IsEnabled = false` instead.
		///        2) In `CCNode`'s OnEnter and OnExit, listeners associated with that node will automatically update their `paused state`.
        /// </summary>
        internal virtual bool IsPaused { get; set; }

        /// <summary>
        /// Whether the listener has been added to dispatcher.
        /// </summary>
		internal virtual bool IsRegistered { get; set; }

        /// <summary>
        /// Event listener type
        /// </summary>
		internal virtual CCEventListenerType Type { get; set; }

        /// <summary>
        /// The priority of event listener
        /// The higher the number, the higher the priority, 0 is for scene graph base priority.
        /// </summary>
		internal virtual int FixedPriority { get; set; }

        /// <summary>
        /// Scene graph based priority
        /// </summary>
		internal virtual CCNode SceneGraphPriority { get; set; }

        /// <summary>
        /// Event callback function
        /// </summary>
		internal Action<CCEvent> OnEvent { get; set; }

        /// <summary>
        /// Sender of the event
        /// </summary>
        internal virtual CCNode Sender { get; set; }

		/// <summary>
		/// Enables or disables the listener.
		///  *Note* Only listeners with `enabled` state will be able to receive events.
		///        When an listener was initialized, it's enabled by default.
		///        For a `scene graph priority` listener, to receive an event, excepting it was `enabled`,
		///        it also shouldn't be in `pause` state.
		/// </summary>
		/// <returns><c>true</c> if this instance is enabled the specified enabled; otherwise, <c>false</c>.</returns>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public virtual bool IsEnabled { get; set; }



        protected CCEventListener() 
        {
            IsRegistered = false;
            IsPaused = true;
			IsEnabled = true;
        }

        protected CCEventListener(CCEventListenerType type, string listenerID)
            : this()
        {
            Type = type;
            ListenerID = listenerID;
        }

        /// <summary>
        /// Initializes event with type and callback function
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listenerID"></param>
        /// <param name="callback"></param>
        protected CCEventListener(CCEventListenerType type, string listenerID, Action<CCEvent> callback)
            : this(type, listenerID)
        {
            OnEvent = callback;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CocosSharp.CCEventListener"/> class using the event listener passed
		/// </summary>
		/// <param name="eventListener">Event listener.</param>
		protected CCEventListener(CCEventListener eventListener)
		{
			//throw new NotImplementedException ();
		}

        /// <summary>
        /// Checks whether the listener is available. 
        /// </summary>
        public virtual bool IsAvailable
        {
            get { return OnEvent != null; }
        }

        // Clones the listener, its subclasses have to override this method.
        public virtual CCEventListener Copy()
        {
			// TODO: fix me to do something
            return new CCEventListener();
        }

        #region IDisposable Methods

        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
				//Let us release all the managed resources
                // Release our Managed Resources
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC

        }

        #endregion

        ~CCEventListener()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
			//CCLog.Log("In the finalizer of CCEventListener. {0}", this);
        }

    }

}
