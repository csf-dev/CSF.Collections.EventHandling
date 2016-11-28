//
// EventHandlingCollectionExtensions.cs
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

namespace CSF.Collections.EventHandling
{
  public static class EventHandlingCollectionWrapperExtensions
  {
    #region extension methods

    public static void AfterAddItem<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                           Action<TItem> action)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }
      if(action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      wrapper.AfterAdd += (sender, e) => {
        var args = e as ICollectionItemInfo<TItem>;
        if(args != null)
        {
          action(args.Item);
        }
      };
    }

    public static void AfterRemoveItem<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                              Action<TItem> action)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }
      if(action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      wrapper.AfterRemove += (sender, e) => {
        var args = e as ICollectionItemInfo<TItem>;
        if(args != null)
        {
          action(args.Item);
        }
      };
    }

    public static void BeforeAddItem<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                            Func<TItem,bool> action)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }
      if(action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      wrapper.BeforeAdd += (sender, e) => {
        var args = e as ICollectionItemInfo<TItem>;
        bool okToContinue = true;
        if(args != null)
        {
          okToContinue = action(args.Item);
        }

        var cancelable = e as ICancelable;
        if(cancelable != null && !okToContinue)
        {
          cancelable.Cancel();
        }
      };
    }

    public static void BeforeRemoveItem<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                               Func<TItem,bool> action)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }
      if(action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      wrapper.BeforeRemove += (sender, e) => {
        var args = e as ICollectionItemInfo<TItem>;
        bool okToContinue = true;
        if(args != null)
        {
          okToContinue = action(args.Item);
        }

        var cancelable = e as ICancelable;
        if(cancelable != null && !okToContinue)
        {
          cancelable.Cancel();
        }
      };
    }

    #endregion
  }
}

