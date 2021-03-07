//
// EventHandlingCollectionBase.cs
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
using System.Linq;
using System.Collections;
using System.Collections.Specialized;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Base type for event-handling collections.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public abstract class EventRaisingCollectionBase<TItem> : IEventRaisingCollection<TItem>, INotifyCollectionChanged
#if !NETSTANDARD1_0
        , System.Runtime.Serialization.IDeserializationCallback
#endif
        where TItem : class
    {
        /// <summary>
        /// Gets the wrapped source collection instance.
        /// </summary>
        /// <value>The source collection.</value>
        protected ICollection<TItem> SourceCollection { get; }

        /// <summary>
        /// Gets the count of elements in this collection.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public virtual int Count => SourceCollection.Count;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsReadOnly => SourceCollection.IsReadOnly;

        /// <summary>
        /// Gets the enumerator for the current instance.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        public virtual IEnumerator<TItem> GetEnumerator () => SourceCollection.GetEnumerator ();

        /// <summary>
        /// Adds an item to the current instance.
        /// </summary>
        /// <param name='item'>
        /// The item to add.
        /// </param>
        public virtual void Add (TItem item)
        {
            if (HandleBeforeAdd (item)) {
                SourceCollection.Add (item);
                HandleAfterAdd (item);
                HandleAdditionChange(item);
            }
        }

        /// <summary>
        /// Clears all items from the current instance.
        /// </summary>
        public virtual void Clear ()
        {
            while (SourceCollection.Any ()) {
                Remove (SourceCollection.First ());
            }
        }

        /// <summary>
        /// Determines whether the current collection contains a specific value.
        /// </summary>
        /// <param name='item'>
        /// The item to search for.
        /// </param>
        public virtual bool Contains (TItem item) => SourceCollection.Contains (item);

        /// <summary>
        /// Copies the contents of the current instance to an array.
        /// </summary>
        /// <param name='array'>
        /// The array to copy to.
        /// </param>
        /// <param name='arrayIndex'>
        /// Array index.
        /// </param>
        public virtual void CopyTo (TItem [] array, int arrayIndex) => SourceCollection.CopyTo (array, arrayIndex);

        /// <summary>
        /// Removes the first occurrence of an item from the current collection.
        /// </summary>
        /// <param name='item'>
        /// The item to remove from the current collection.
        /// </param>
        public virtual bool Remove (TItem item)
        {
            if (HandleBeforeRemove (item)) {
                var output = SourceCollection.Remove (item);

                if (output) {
                    HandleAfterRemove (item);
                    HandleRemovalChange(item);
                }

                return output;
            }

            return false;
        }

        #region explicit interface implementations

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();

        void ICollection.CopyTo (Array array, int index)
        {
            TItem [] copy = new TItem [SourceCollection.Count];
            CopyTo (copy, 0);
            Array.Copy (copy, 0, array, index, SourceCollection.Count);
        }

        object ICollection.SyncRoot => ((ICollection)SourceCollection).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)SourceCollection).IsSynchronized;

        #endregion

        /// <summary>
        /// Occurs before an item is added to the collection.
        /// </summary>
        public event EventHandler<BeforeModifyEventArgs<TItem>> BeforeAdd;

        /// <summary>
        /// Occurs after an item is added to the collection.
        /// </summary>
        public event EventHandler<AfterModifyEventArgs<TItem>> AfterAdd;

        /// <summary>
        /// Occurs before an item is removed the collection.
        /// </summary>
        public event EventHandler<BeforeModifyEventArgs<TItem>> BeforeRemove;

        /// <summary>
        /// Occurs after an item is removed from the collection.
        /// </summary>
        public event EventHandler<AfterModifyEventArgs<TItem>> AfterRemove;

        /// <summary>
        /// Notifies listeners of dynamic changes, such as when an item is added and removed or the whole list is cleared.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the <see cref="BeforeAdd"/> event.
        /// </summary>
        /// <returns><c>true</c>, if it is OK for the add-item to continue, <c>false</c> if it is has been cancelled.</returns>
        /// <param name="item">The item to be added.</param>
        protected bool HandleBeforeAdd (TItem item)
        {
            var args = CreateBeforeActionEventArgs (item);
            BeforeAdd?.Invoke (this, args);

            return (!(args is ICancelable cancelable)) || !cancelable.IsCancelled;
        }

        /// <summary>
        /// Raises the <see cref="AfterAdd"/> event.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        protected void HandleAfterAdd (TItem item)
        {
            var args = CreateAfterActionEventArgs (item);
            AfterAdd?.Invoke (this, args);
        }

        /// <summary>
        /// Raises the <see cref="BeforeRemove"/> event.
        /// </summary>
        /// <returns><c>true</c>, if it is OK for the remove-item to continue, <c>false</c> if it is has been cancelled.</returns>
        /// <param name="item">The item to be removed.</param>
        protected bool HandleBeforeRemove (TItem item)
        {
            var args = CreateBeforeActionEventArgs (item);
            BeforeRemove?.Invoke (this, args);

            return (!(args is ICancelable cancelable)) || !cancelable.IsCancelled;
        }

        /// <summary>
        /// Raises the <see cref="AfterRemove"/> event.
        /// </summary>
        /// <param name="item">The associated item.</param>
        protected void HandleAfterRemove (TItem item)
        {
            var args = CreateAfterActionEventArgs (item);
            AfterRemove?.Invoke (this, args);
        }

        /// <summary>
        /// Handles the change event for the addition of an item.
        /// </summary>
        /// <param name="item">The item which was added.</param>
        protected void HandleAdditionChange(TItem item)
        {
            var changeArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
            CollectionChanged?.Invoke(this, changeArgs);
        }

        /// <summary>
        /// Handles the change event for a removal of an item.
        /// </summary>
        /// <param name="item">The item which was removed.</param>
        protected void HandleRemovalChange(TItem item)
        {
            var changeArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);
            CollectionChanged?.Invoke(this, changeArgs);
        }

        /// <summary>
        /// Creates a set of appropriately-populated before-action event arguments.
        /// </summary>
        /// <returns>The before-action event arguments.</returns>
        /// <param name="item">Item.</param>
        protected abstract BeforeModifyEventArgs<TItem> CreateBeforeActionEventArgs (TItem item);

        /// <summary>
        /// Creates a set of appropriately-populated after-action event arguments.
        /// </summary>
        /// <returns>The after-action event arguments.</returns>
        /// <param name="item">The associated item.</param>
        protected abstract AfterModifyEventArgs<TItem> CreateAfterActionEventArgs (TItem item);

#if !NETSTANDARD1_0
        void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization (object sender)
        {
            if (SourceCollection == null)
                throw new System.Runtime.Serialization.SerializationException ($"The serialized object must have a non-null {nameof(SourceCollection)}");
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventHandlingCollectionBase{TItem}"/> class.
        /// </summary>
        /// <param name='source'>The source collection that this instance wraps.</param>
        protected EventRaisingCollectionBase (ICollection<TItem> source)
        {
            SourceCollection = source ?? throw new ArgumentNullException (nameof (source));
        }
    }
}

