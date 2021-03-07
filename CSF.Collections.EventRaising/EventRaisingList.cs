//
// EventHandlingList.cs
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
using System.Collections.Generic;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Implementation of <see cref="T:EventRaisingCollectionBase{TItem}"/> for the generic <c>IList</c>.
    /// </summary>
    public class EventRaisingList<TItem> : EventRaisingCollectionBase<TItem>, IList<TItem>
        where TItem : class
    {
        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name='index'>
        /// The numeric index to get/set to/from.
        /// </param>
        public virtual TItem this [int index] {
            get {
                return GetSourceCollection () [index];
            }
            set {
                bool removeDidNotCancelReplacement = true;

                if (Count > index) {
                    var item = GetSourceCollection () [index];
                    removeDidNotCancelReplacement = HandleBeforeRemove (item);

                    if (removeDidNotCancelReplacement && SourceCollection.Remove(item))
                    {
                        HandleAfterRemove (item);
                        HandleRemovalChange (item);
                    }
                }

                if (removeDidNotCancelReplacement) {
                    this.Insert (index, value);
                }
            }
        }

        /// <summary>
        /// Removes the item at the given <paramref name="index"/> from this collection.
        /// </summary>
        /// <param name='index'>
        /// The index at which to remove the item.
        /// </param>
        public virtual void RemoveAt (int index)
        {
            var item = GetSourceCollection() [index];

            if (HandleBeforeRemove (item) && SourceCollection.Remove(item))
            {
                HandleAfterRemove (item);
                HandleRemovalChange(item);
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the current instance.
        /// </summary>
        /// <returns>
        /// The numeric index of the item.
        /// </returns>
        /// <param name='item'>
        /// The item to search for.
        /// </param>
        public virtual int IndexOf (TItem item)
        {
            return GetSourceCollection ().IndexOf (item);
        }

        /// <summary>
        /// Inserts an item into the current collection at the specified index.
        /// </summary>
        /// <param name='index'>
        /// The index at which to insert the item.
        /// </param>
        /// <param name='item'>
        /// The item to insert.
        /// </param>
        public virtual void Insert (int index, TItem item)
        {
            if (HandleBeforeAdd (item)) {
                GetSourceCollection ().Insert (index, item);
                HandleAfterAdd (item);
                HandleAdditionChange(item);
            }
        }

        /// <summary>
        /// Gets a strongly-typed representation of the <see cref="P:SourceCollection"/>.
        /// </summary>
        /// <returns>The source collection.</returns>
        protected IList<TItem> GetSourceCollection () => (IList<TItem>)SourceCollection;

        /// <summary>
        /// Creates a set of appropriately-populated before-action event arguments.
        /// </summary>
        /// <returns>The before-action event arguments.</returns>
        /// <param name="item">Item.</param>
        protected override BeforeModifyEventArgs<TItem> CreateBeforeActionEventArgs (TItem item)
            => new BeforeModifyEventArgs<TItem> (SourceCollection, item);

        /// <summary>
        /// Creates a set of appropriately-populated after-action event arguments.
        /// </summary>
        /// <returns>The after-action event arguments.</returns>
        /// <param name="item">The associated item.</param>
        protected override AfterModifyEventArgs<TItem> CreateAfterActionEventArgs (TItem item)
            => new AfterModifyEventArgs<TItem> (SourceCollection, item);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventRaisingList{TItem}"/> class.
        /// </summary>
        /// <param name='source'>The source collection that this instance wraps.</param>
        public EventRaisingList (IList<TItem> source) : base (source) { }
    }
}

