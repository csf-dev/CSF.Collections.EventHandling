//
// EventHandlingCollectionTestBase.cs
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
using NUnit.Framework;
using CSF.Collections.EventRaising;
using Test.CSF.Collections.EventRaising.Mocks;
using System.Collections.Generic;

namespace Test.CSF.Collections.EventRaising
{
  public abstract class EventRaisingCollectionTestBase
  {
    #region properties

    protected bool CallbackOneCalled
    {
      get;
      private set;
    }

    protected bool CallbackTwoCalled
    {
      get;
      private set;
    }

    protected Person Replacement
    {
      get;
      private set;
    }

    protected IList<Person> SourceCollection
    {
      get;
      private set;
    }

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      CallbackOneCalled = false;
      CallbackTwoCalled = false;

      SourceCollection = new [] {
        new Person() { Name = "Joe",    Age = 20 },
        new Person() { Name = "Susan",  Age = 30 },
        new Person() { Name = "Deepak", Age = 40 },
      };

      Replacement = new Person() { Name = "Claire", Age = 35 };

      AdditionalSetup();
    }

    protected virtual void AdditionalSetup()
    {
      return;
    }

    #endregion

    #region methods

    protected void RecordingCallbackOne(object sender, EventArgs ev)
    {
      CallbackOneCalled = true;
    }

    protected void RecordingCallbackTwo(object sender, EventArgs ev)
    {
      CallbackTwoCalled = true;
    }

    protected void CancellingCallback(object sender, EventArgs ev)
    {
      var cancelable = ev as ICancelable;
      if(cancelable != null)
      {
        cancelable.Cancel();
      }
    }

    #endregion
  }
}

