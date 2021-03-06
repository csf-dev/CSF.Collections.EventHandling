﻿//
// Parent.cs
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
using CSF.Collections.EventRaising;
using System.Collections.Generic;

namespace Test.CSF.Collections.EventRaising.Mocks
{
  public class Parent
  {
    #region fields

    private EventRaisingSetWrapper<Child> _children;

    #endregion

    #region properties

    public virtual ISet<Child> Children { get { return _children.Collection; } }

    protected virtual ISet<Child> ChildrenSource {
      get { return _children.SourceCollection; }
      set { _children.SourceCollection = value; }
    }

    #endregion

    #region constructor

    public Parent()
    {
      _children = new EventRaisingSetWrapper<Child>(new HashSet<Child>());
      _children.AfterAdd += (sender, e) => e.Item.Parent = this;
      _children.AfterRemove += (sender, e) => e.Item.Parent = null;
    }

    #endregion
  }
}

