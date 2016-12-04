//
// EventHandlingCollectionWrapperExtensions.cs
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

namespace CSF.Collections.EventRaising
{
  /// <summary>
  /// Extension methods for implementors of <see cref="T:IHasModificationEvents{TItem}"/>.
  /// </summary>
  public static class ModificationEventExtensions
  {
    /// <summary>
    /// Sets up all of the possible collection-modification events using short delegates.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whilst this method (and related extension methods defined here) may be convenient, it comes with the drawback
    /// that it is impossible to use this mechanism to unsubscribe these event handler delegates, should you wish to.
    /// In order to do that you must use the full event mechanism and subscribe to the wrapper's events explicitly.
    /// </para>
    /// <para>
    /// This mechanism provides a convenient shortcut to the events themselves, but please only use it when it is
    /// applicable.
    /// </para>
    /// </remarks>
    /// <param name="wrapper">The event-raising collection wrapper.</param>
    /// <param name="beforeAdd">The before-add handler.</param>
    /// <param name="beforeRemove">The before-remove handler.</param>
    /// <param name="afterAdd">The after-add handler.</param>
    /// <param name="afterRemove">The after-remove handler.</param>
    /// <typeparam name="TItem">The type of object contained within the collection.</typeparam>
    public static void SetupActions<TItem>(this IHasModificationEvents<TItem> wrapper,
                                           Action<IBeforeModify<TItem>> beforeAdd = null,
                                           Action<IBeforeModify<TItem>> beforeRemove = null,
                                           Action<IAfterModify<TItem>> afterAdd = null,
                                           Action<IAfterModify<TItem>> afterRemove = null)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }

      if(beforeAdd != null)
      {
        wrapper.BeforeAdd += (sender, e) => beforeAdd(e);
      }

      if(beforeRemove != null)
      {
        wrapper.BeforeRemove += (sender, e) => beforeRemove(e);
      }

      if(afterAdd != null)
      {
        wrapper.AfterAdd += (sender, e) => afterAdd(e);
      }

      if(afterRemove != null)
      {
        wrapper.AfterRemove += (sender, e) => afterRemove(e);
      }
    }

    /// <summary>
    /// Sets up the possible before-modify collection events using short delegates.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whilst this method (and related extension methods defined here) may be convenient, it comes with the drawback
    /// that it is impossible to use this mechanism to unsubscribe these event handler delegates, should you wish to.
    /// In order to do that you must use the full event mechanism and subscribe to the wrapper's events explicitly.
    /// </para>
    /// <para>
    /// This mechanism provides a convenient shortcut to the events themselves, but please only use it when it is
    /// applicable.
    /// </para>
    /// </remarks>
    /// <param name="wrapper">The event-raising collection wrapper.</param>
    /// <param name="add">The before-add handler.</param>
    /// <param name="remove">The before-remove handler.</param>
    /// <typeparam name="TItem">The type of object contained within the collection.</typeparam>
    public static void SetupAfterActions<TItem>(this IHasModificationEvents<TItem> wrapper,
                                                Action<IAfterModify<TItem>> add = null,
                                                Action<IAfterModify<TItem>> remove = null)
      where TItem : class
    {
      SetupActions(wrapper, afterAdd: add, afterRemove: remove);
    }

    /// <summary>
    /// Sets up the possible after-modify collection events using short delegates.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whilst this method (and related extension methods defined here) may be convenient, it comes with the drawback
    /// that it is impossible to use this mechanism to unsubscribe these event handler delegates, should you wish to.
    /// In order to do that you must use the full event mechanism and subscribe to the wrapper's events explicitly.
    /// </para>
    /// <para>
    /// This mechanism provides a convenient shortcut to the events themselves, but please only use it when it is
    /// applicable.
    /// </para>
    /// </remarks>
    /// <param name="wrapper">The event-raising collection wrapper.</param>
    /// <param name="add">The after-add handler.</param>
    /// <param name="remove">The after-remove handler.</param>
    /// <typeparam name="TItem">The type of object contained within the collection.</typeparam>
    public static void SetupBeforeActions<TItem>(this IHasModificationEvents<TItem> wrapper,
                                                 Action<IBeforeModify<TItem>> add = null,
                                                 Action<IBeforeModify<TItem>> remove = null)
      where TItem : class
    {
      SetupActions(wrapper, beforeAdd: add, beforeRemove: remove);
    }
  }
}

