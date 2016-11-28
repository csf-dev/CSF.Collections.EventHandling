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

namespace CSF.Collections.EventHandling
{
  public interface IEventHandlingCollectionWrapper
  {
    #region events

    /// <summary>
    /// Occurs before an item is added to the <see cref="Collection"/>.
    /// </summary>
    event EventHandler BeforeAdd;

    /// <summary>
    /// Occurs after an item is added to the <see cref="Collection"/>.
    /// </summary>
    event EventHandler AfterAdd;

    /// <summary>
    /// Occurs before an item is removed from the <see cref="Collection"/>.
    /// </summary>
    event EventHandler BeforeRemove;

    /// <summary>
    /// Occurs after an item is removed from the <see cref="Collection"/>.
    /// </summary>
    event EventHandler AfterRemove;

    /// <summary>
    /// Occurs before the <see cref="SourceCollection"/> is replaced with a new collection instance.
    /// </summary>
    event EventHandler BeforeReplace;

    /// <summary>
    /// Occurs after the <see cref="SourceCollection"/> is replaced with a new collection instance.
    /// </summary>
    event EventHandler AfterReplace;

    #endregion
  }

  /// <summary>
  /// Interface for a type which wraps a normal collection and triggers events when the collection is modified.
  /// </summary>
  public interface IEventHandlingCollectionWrapper<TCollection,TItem> : IEventHandlingCollectionWrapper
    where TCollection : ICollection<TItem>
    where TItem : class
  {
    #region properties

    /// <summary>
    /// Gets the collection instance which exposes the events.  Modifications to this collection will raise the events.
    /// </summary>
    /// <value>The event handling collection.</value>
    TCollection Collection { get; }

    /// <summary>
    /// Gets or set the source collection, which is not wrapped with modification events.
    /// </summary>
    /// <value>The source collection.</value>
    TCollection SourceCollection { get; set; }

    #endregion
  }
}

