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
  public static class EventRaisingCollectionWrapperExtensions
  {
    public static void SetupEvents<TItem>(this IEventRaisingCollectionWrapper<TItem> wrapper,
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

    public static void SetupAfterEvents<TItem>(this IEventRaisingCollectionWrapper<TItem> wrapper,
                                               Action<IAfterModify<TItem>> add = null,
                                               Action<IAfterModify<TItem>> remove = null)
      where TItem : class
    {
      SetupEvents(wrapper, afterAdd: add, afterRemove: remove);
    }

    public static void SetupBeforeEvents<TItem>(this IEventRaisingCollectionWrapper<TItem> wrapper,
                                                Action<IBeforeModify<TItem>> add = null,
                                                Action<IBeforeModify<TItem>> remove = null)
      where TItem : class
    {
      SetupEvents(wrapper, beforeAdd: add, beforeRemove: remove);
    }
  }
}

