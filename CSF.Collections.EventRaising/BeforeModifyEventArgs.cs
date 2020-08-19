//
// CollectionItemBeforeActionEventArgs.cs
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
    /// Implementation of <c>EventArgs</c> for events which are raised before the modification of a collection.
    /// </summary>
    public class BeforeModifyEventArgs<TItem> : AfterModifyEventArgs<TItem>, IBeforeModify<TItem>
      where TItem : class
    {
        /// <summary>
        /// Gets a value indicating whether this instance is cancelled.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Cancels the current action.
        /// </summary>
        public void Cancel () => IsCancelled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.Collections.EventRaising.BeforeModifyEventArgs{TItem}"/>
        /// class.
        /// </summary>
        /// <param name="collection">The collection which is to be modified.</param>
        /// <param name="item">The item to be added or removed to/from the collection.</param>
        public BeforeModifyEventArgs (ICollection<TItem> collection, TItem item) : base (collection, item)
        {
            IsCancelled = false;
        }
    }
}

