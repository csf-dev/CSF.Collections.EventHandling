//
// IEventHandlingCollection.cs
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
    /// Interface for an event-raising collection wrapper.
    /// </summary>
    public interface IEventRaisingCollectionWrapper<TItem> : IHasModificationEvents<TItem>
      where TItem : class
    {
        /// <summary>
        /// Gets the collection instance which exposes the events.  Modifications to this collection will raise the events.
        /// </summary>
        /// <value>The event handling collection.</value>
        ICollection<TItem> Collection { get; }

        /// <summary>
        /// Gets or set the source collection, which is not wrapped with modification events.
        /// </summary>
        /// <value>The source collection.</value>
        ICollection<TItem> SourceCollection { get; set; }
    }

    /// <summary>
    /// Interface for a type which wraps a normal collection and triggers events when the collection is modified.
    /// </summary>
    public interface IEventRaisingCollectionWrapper<TCollection, TItem> : IEventRaisingCollectionWrapper<TItem>
      where TCollection : ICollection<TItem>
      where TItem : class
    {
        /// <summary>
        /// Gets the collection instance which exposes the events.  Modifications to this collection will raise the events.
        /// </summary>
        /// <value>The event handling collection.</value>
        new TCollection Collection { get; }

        /// <summary>
        /// Gets or set the source collection, which is not wrapped with modification events.
        /// </summary>
        /// <value>The source collection.</value>
        new TCollection SourceCollection { get; set; }

        /// <summary>
        /// Occurs before the <see cref="SourceCollection"/> is replaced with a new collection instance.
        /// </summary>
        event EventHandler<BeforeReplaceEventArgs<TCollection>> BeforeReplace;

        /// <summary>
        /// Occurs after the <see cref="SourceCollection"/> is replaced with a new collection instance.
        /// </summary>
        event EventHandler<AfterReplaceEventArgs<TCollection>> AfterReplace;
    }
}

