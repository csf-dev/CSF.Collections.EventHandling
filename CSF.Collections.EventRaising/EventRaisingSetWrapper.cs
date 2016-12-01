//
// EventHandlingSetWrapper.cs
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
  /// Implementation of <see cref="T:EventRaisingCollectionWrapperBase{TCollection,TItem}"/> for generic instances
  /// of <c>ISet</c>.
  /// </summary>
  public class EventRaisingSetWrapper<TItem> : EventRaisingCollectionWrapperBase<ISet<TItem>,TItem>
    where TItem : class
  {
    #region methods

    /// <summary>
    /// Creates the event handling collection implementation instance.
    /// </summary>
    /// <returns>The event handling collection.</returns>
    /// <param name="newSourceCollection">New source collection.</param>
    protected override Impl.IEventRaisingCollection<TItem> CreateEventHandlingCollection(ISet<TItem> newSourceCollection)
    {
      return new Impl.EventRaisingSet<TItem>(newSourceCollection);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:EventRaisingSetWrapper{TItem}"/> class.
    /// </summary>
    /// <param name="source">A source collection with which to initialise this instance.</param>
    public EventRaisingSetWrapper(ISet<TItem> source) : base(source) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:EventRaisingSetWrapper{TItem}"/> class.
    /// </summary>
    public EventRaisingSetWrapper() : base() {}

    #endregion
  }
}

