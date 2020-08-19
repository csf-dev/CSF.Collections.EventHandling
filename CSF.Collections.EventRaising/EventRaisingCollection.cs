//
// EventHandlingCollection.cs
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
    /// Implementation of <see cref="T:EventRaisingCollectionBase{TItem}"/> for <c>ICollection</c>.
    /// </summary>
    public class EventRaisingCollection<TItem> : EventRaisingCollectionBase<TItem>
      where TItem : class
    {
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
        /// Initializes a new instance of the <see cref="T:EventRaisingCollection{TItem}"/> class.
        /// </summary>
        /// <param name="source">The source collection, to be wrapped.</param>
        public EventRaisingCollection (ICollection<TItem> source) : base (source) { }
    }
}

