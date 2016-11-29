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

namespace CSF.Collections.EventHandling
{
  public static class EventHandlingCollectionWrapperExtensions
  {
    public static void SetupEvents<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                          Func<TItem,bool> beforeAdd = null,
                                          Func<TItem,bool> beforeRemove = null,
                                          Action<TItem> afterAdd = null,
                                          Action<TItem> afterRemove = null)
      where TItem : class
    {
      if(wrapper == null)
      {
        throw new ArgumentNullException(nameof(wrapper));
      }

      if(beforeAdd != null)
      {
        wrapper.BeforeAdd += (sender, e) => {
          var ok = beforeAdd(e.Item);
          if(!ok)
          {
            e.Cancel();
          }
        };
      }

      if(beforeRemove != null)
      {
        wrapper.BeforeRemove += (sender, e) => {
          var ok = beforeRemove(e.Item);
          if(!ok)
          {
            e.Cancel();
          }
        };
      }

      if(afterAdd != null)
      {
        wrapper.AfterAdd += (sender, e) => afterAdd(e.Item);
      }

      if(afterRemove != null)
      {
        wrapper.AfterRemove += (sender, e) => afterRemove(e.Item);
      }
    }

    public static void SetupAfterEvents<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                               Action<TItem> add = null,
                                               Action<TItem> remove = null)
      where TItem : class
    {
      SetupEvents(wrapper, afterAdd: add, afterRemove: remove);
    }

    public static void SetupBeforeEvents<TItem>(this IEventHandlingCollectionWrapper<TItem> wrapper,
                                                Func<TItem,bool> add = null,
                                                Func<TItem,bool> remove = null)
      where TItem : class
    {
      SetupEvents(wrapper, beforeAdd: add, beforeRemove: remove);
    }
  }
}

