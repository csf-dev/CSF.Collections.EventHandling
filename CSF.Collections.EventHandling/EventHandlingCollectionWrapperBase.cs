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
using System.Collections;
using CSF.Collections.EventHandling.Impl;

namespace CSF.Collections.EventHandling
{
  /// <summary>
  /// Concrete implementation of <see cref="T:IEventHandlingCollectionWrapper{TCollection,TItem}"/>, which can deal
  /// with any collection implementation.
  /// </summary>
  public abstract class EventHandlingCollectionWrapperBase<TCollection,TItem>
    : IEventHandlingCollectionWrapper<TCollection,TItem>, IEventHandlingCollectionWrapper<TItem>
    where TCollection : class,ICollection<TItem>
    where TItem : class
  {
    #region properties

    private TCollection _wrappedCollection, _unwrappedCollection;

    #endregion

    #region properties

    /// <summary>
    /// Gets the collection instance which exposes the events.  Modifications to this collection will raise the events.
    /// </summary>
    /// <value>The event handling collection.</value>
    public TCollection Collection
    {
      get {
        return _wrappedCollection;
      }
    }

    /// <summary>
    /// Gets or set the source collection, which is not wrapped with modification events.
    /// </summary>
    /// <value>The source collection.</value>
    public TCollection SourceCollection
    {
      get {
        return _unwrappedCollection;
      }
      set {
        var replacement = value;
        if(HandleBeforeReplace(replacement))
        {
          var source = _unwrappedCollection;

          ReplaceWrappedCollection(replacement);
          _unwrappedCollection = replacement;

          HandleAfterReplace(source, replacement);
        }
      }
    }

    /// <summary>
    /// Gets the <see cref="Collection"/> as an event-handling collection instance.
    /// </summary>
    /// <value>The event handling collection.</value>
    protected Impl.IEventHandlingCollection<TItem> EventHandlingCollection
    {
      get {
        return (Impl.IEventHandlingCollection<TItem>) Collection;
      }
    }

    #endregion

    #region events

    /// <summary>
    /// Occurs before an item is added to the <see cref="Collection"/>.
    /// </summary>
    public event EventHandler BeforeAdd;

    /// <summary>
    /// Occurs after an item is added to the <see cref="Collection"/>.
    /// </summary>
    public event EventHandler AfterAdd;

    /// <summary>
    /// Occurs before an item is removed from the <see cref="Collection"/>.
    /// </summary>
    public event EventHandler BeforeRemove;

    /// <summary>
    /// Occurs after an item is removed from the <see cref="Collection"/>.
    /// </summary>
    public event EventHandler AfterRemove;

    /// <summary>
    /// Occurs before the <see cref="SourceCollection"/> is replaced with a new collection instance.
    /// </summary>
    public event EventHandler BeforeReplace;

    /// <summary>
    /// Occurs after the <see cref="SourceCollection"/> is replaced with a new collection instance.
    /// </summary>
    public event EventHandler AfterReplace;

    #endregion

    #region explicit interface implementations

    ICollection<TItem> IEventHandlingCollectionWrapper<TItem>.Collection
    {
      get {
        return Collection;
      }
    }

    ICollection<TItem> IEventHandlingCollectionWrapper<TItem>.SourceCollection
    {
      get {
        return SourceCollection;
      }
      set {
        SourceCollection = (TCollection) value;
      }
    }


    #endregion

    #region methods

    protected virtual void OnBeforeAdd(object sender, EventArgs ev)
    {
      BeforeAdd?.Invoke(this, ev);
    }

    protected virtual void OnAfterAdd(object sender, EventArgs ev)
    {
      AfterAdd?.Invoke(this, ev);
    }

    protected virtual void OnBeforeRemove(object sender, EventArgs ev)
    {
      BeforeRemove?.Invoke(this, ev);
    }

    protected virtual void OnAfterRemove(object sender, EventArgs ev)
    {
      AfterRemove?.Invoke(this, ev);
    }

    protected virtual bool HandleBeforeReplace(TCollection replacement)
    {
      var args = new BeforeReplaceCollectionEventArgs<TCollection>(SourceCollection, replacement);
      BeforeReplace?.Invoke(this, args);
      return !args.IsCancelled;
    }

    protected virtual void HandleAfterReplace(TCollection source, TCollection replacement)
    {
      var args = new ReplaceCollectionEventArgs<TCollection>(source, replacement);
      AfterReplace?.Invoke(this, args);
    }

    protected virtual void ReplaceWrappedCollection(TCollection newSourceCollection)
    {
      SetWrappedCollection(newSourceCollection);
    }

    private void SetWrappedCollection(TCollection sourceCollection)
    {
      var originalWrappedCollection = EventHandlingCollection;

      if(originalWrappedCollection != null)
      {
        originalWrappedCollection.BeforeAdd     -= OnBeforeAdd;
        originalWrappedCollection.AfterAdd      -= OnAfterAdd;
        originalWrappedCollection.BeforeRemove  -= OnBeforeRemove;
        originalWrappedCollection.AfterRemove   -= OnAfterRemove;
      }

      if(sourceCollection != null)
      {
        var replacement = CreateEventHandlingCollection(sourceCollection);

        replacement.BeforeAdd     += OnBeforeAdd;
        replacement.AfterAdd      += OnAfterAdd;
        replacement.BeforeRemove  += OnBeforeRemove;
        replacement.AfterRemove   += OnAfterRemove;

        _unwrappedCollection = (TCollection) replacement;
      }
      else
      {
        _wrappedCollection = null;
      }
    }

    protected abstract IEventHandlingCollection<TItem> CreateEventHandlingCollection(TCollection newSourceCollection);

    #endregion

    #region constructor

    public EventHandlingCollectionWrapperBase(TCollection sourceCollection)
    {
      SetWrappedCollection(sourceCollection);
      _unwrappedCollection = sourceCollection;
    }

    #endregion
  }
}

