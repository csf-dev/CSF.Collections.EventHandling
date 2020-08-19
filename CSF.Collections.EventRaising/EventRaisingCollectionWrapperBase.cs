//
// EventHandlingCollectionWrapper.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2016 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Concrete implementation of <see cref="T:IEventHandlingCollectionWrapper{TCollection,TItem}"/>, which can deal
    /// with any collection implementation.
    /// </summary>
    public abstract class EventRaisingCollectionWrapperBase<TCollection, TItem>
        : IEventRaisingCollectionWrapper<TCollection, TItem>, IEventRaisingCollectionWrapper<TItem>
        where TCollection : class, ICollection<TItem>
        where TItem : class
    {
        private TCollection _unwrappedCollection;

        /// <summary>
        /// Gets the collection instance which exposes the events.  Modifications to this collection will raise the events.
        /// </summary>
        /// <value>The event handling collection.</value>
        public TCollection Collection { get; private set; }

        /// <summary>
        /// Gets or set the source collection, which is not wrapped with modification events.
        /// </summary>
        /// <value>The source collection.</value>
        public TCollection SourceCollection {
            get {
                return _unwrappedCollection;
            }
            set {
                var replacement = value;
                if (HandleBeforeReplace (replacement)) {
                    var source = _unwrappedCollection;

                    ReplaceWrappedCollection (replacement);
                    _unwrappedCollection = replacement;

                    HandleAfterReplace (source, replacement);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Collection"/> as an event-handling collection instance.
        /// </summary>
        /// <value>The event handling collection.</value>
        protected IEventRaisingCollection<TItem> EventHandlingCollection
            => (IEventRaisingCollection<TItem>)Collection;

        /// <summary>
        /// Occurs before an item is added to the <see cref="Collection"/>.
        /// </summary>
        public event EventHandler<BeforeModifyEventArgs<TItem>> BeforeAdd;

        /// <summary>
        /// Occurs after an item is added to the <see cref="Collection"/>.
        /// </summary>
        public event EventHandler<AfterModifyEventArgs<TItem>> AfterAdd;

        /// <summary>
        /// Occurs before an item is removed from the <see cref="Collection"/>.
        /// </summary>
        public event EventHandler<BeforeModifyEventArgs<TItem>> BeforeRemove;

        /// <summary>
        /// Occurs after an item is removed from the <see cref="Collection"/>.
        /// </summary>
        public event EventHandler<AfterModifyEventArgs<TItem>> AfterRemove;

        /// <summary>
        /// Occurs before the <see cref="SourceCollection"/> is replaced with a new collection instance.
        /// </summary>
        public event EventHandler<BeforeReplaceEventArgs<TCollection>> BeforeReplace;

        /// <summary>
        /// Occurs after the <see cref="SourceCollection"/> is replaced with a new collection instance.
        /// </summary>
        public event EventHandler<AfterReplaceEventArgs<TCollection>> AfterReplace;

        #region explicit interface implementations

        ICollection<TItem> IEventRaisingCollectionWrapper<TItem>.Collection => Collection;

        ICollection<TItem> IEventRaisingCollectionWrapper<TItem>.SourceCollection {
            get => SourceCollection;
            set { SourceCollection = (TCollection) value; }
        }

        #endregion

        /// <summary>
        /// Raises the before add event.  This occurs by listening to an event from the wrapper implementation type.
        /// </summary>
        /// <param name="sender">The sender of the original event.</param>
        /// <param name="ev">Event arguments.</param>
        protected virtual void OnBeforeAdd (object sender, BeforeModifyEventArgs<TItem> ev) => BeforeAdd?.Invoke (this, ev);

        /// <summary>
        /// Raises the after add event.  This occurs by listening to an event from the wrapper implementation type.
        /// </summary>
        /// <param name="sender">The sender of the original event.</param>
        /// <param name="ev">Event arguments.</param>
        protected virtual void OnAfterAdd (object sender, AfterModifyEventArgs<TItem> ev) => AfterAdd?.Invoke (this, ev);

        /// <summary>
        /// Raises the before remove event.  This occurs by listening to an event from the wrapper implementation type.
        /// </summary>
        /// <param name="sender">The sender of the original event.</param>
        /// <param name="ev">Event arguments.</param>
        protected virtual void OnBeforeRemove (object sender, BeforeModifyEventArgs<TItem> ev) => BeforeRemove?.Invoke (this, ev);

        /// <summary>
        /// Raises the after remove event.  This occurs by listening to an event from the wrapper implementation type.
        /// </summary>
        /// <param name="sender">The sender of the original event.</param>
        /// <param name="ev">Event arguments.</param>
        protected virtual void OnAfterRemove (object sender, AfterModifyEventArgs<TItem> ev) => AfterRemove?.Invoke (this, ev);

        /// <summary>
        /// Raises the before replace event.
        /// </summary>
        /// <returns><c>true</c>, if the replacement is permitted, <c>false</c> otherwise.</returns>
        /// <param name="replacement">The replacement collection.</param>
        protected virtual bool HandleBeforeReplace (TCollection replacement)
        {
            var args = new BeforeReplaceEventArgs<TCollection> (SourceCollection, replacement);
            BeforeReplace?.Invoke (this, args);
            return !args.IsCancelled;
        }

        /// <summary>
        /// Raises the after replace event.
        /// </summary>
        /// <returns><c>true</c>, if the replacement is permitted, <c>false</c> otherwise.</returns>
        /// <param name="source">The original collection (to be replaced).</param>
        /// <param name="replacement">The replacement collection.</param>
        protected virtual void HandleAfterReplace (TCollection source, TCollection replacement)
        {
            var args = new AfterReplaceEventArgs<TCollection> (source, replacement);
            AfterReplace?.Invoke (this, args);
        }

        /// <summary>
        /// Replaces the wrapped collection using a new source collection.
        /// </summary>
        /// <param name="newSourceCollection">New source collection.</param>
        protected virtual void ReplaceWrappedCollection (TCollection newSourceCollection)
        {
            SetWrappedCollection (newSourceCollection);
        }

        void SetWrappedCollection (TCollection sourceCollection)
        {
            var originalWrappedCollection = EventHandlingCollection;

            if (originalWrappedCollection != null) {
                originalWrappedCollection.BeforeAdd -= OnBeforeAdd;
                originalWrappedCollection.AfterAdd -= OnAfterAdd;
                originalWrappedCollection.BeforeRemove -= OnBeforeRemove;
                originalWrappedCollection.AfterRemove -= OnAfterRemove;
            }

            if (sourceCollection != null) {
                var replacement = CreateEventHandlingCollection (sourceCollection);

                replacement.BeforeAdd += OnBeforeAdd;
                replacement.AfterAdd += OnAfterAdd;
                replacement.BeforeRemove += OnBeforeRemove;
                replacement.AfterRemove += OnAfterRemove;

                Collection = (TCollection)replacement;
            } else {
                Collection = null;
            }
        }

        /// <summary>
        /// Creates the event handling collection implementation instance.
        /// </summary>
        /// <returns>The event handling collection.</returns>
        /// <param name="newSourceCollection">New source collection.</param>
        protected abstract IEventRaisingCollection<TItem> CreateEventHandlingCollection (TCollection newSourceCollection);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventRaisingCollectionWrapperBase{TCollection,TItem}"/> class.
        /// </summary>
        protected EventRaisingCollectionWrapperBase () : this (null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventRaisingCollectionWrapperBase{TCollection,TItem}"/> class.
        /// </summary>
        /// <param name="sourceCollection">A source collection with which to initialise this instance.</param>
        protected EventRaisingCollectionWrapperBase (TCollection sourceCollection)
        {
            SetWrappedCollection (sourceCollection);
            _unwrappedCollection = sourceCollection;
        }
    }
}

