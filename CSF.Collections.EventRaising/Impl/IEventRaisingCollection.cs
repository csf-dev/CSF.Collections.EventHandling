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
using System.Collections;

namespace CSF.Collections.EventRaising.Impl
{
  /// <summary>
  /// Interface for an event-raising collection.
  /// </summary>
  public interface IEventRaisingCollection<TItem> : ICollection<TItem>, ICollection
    where TItem : class
  {
    /// <summary>
    /// Occurs before an item is added to the collection.
    /// </summary>
    event EventHandler<BeforeModifyEventArgs<TItem>> BeforeAdd;

    /// <summary>
    /// Occurs after an item is added to the collection.
    /// </summary>
    event EventHandler<AfterModifyEventArgs<TItem>> AfterAdd;

    /// <summary>
    /// Occurs before an item is removed the collection.
    /// </summary>
    event EventHandler<BeforeModifyEventArgs<TItem>> BeforeRemove;

    /// <summary>
    /// Occurs after an item is removed from the collection.
    /// </summary>
    event EventHandler<AfterModifyEventArgs<TItem>> AfterRemove;
  }
}

