//
// TestEventHandlingSet.cs
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
using Test.CSF.Collections.EventHandling.Mocks;
using CSF.Collections.EventHandling.Impl;
using NUnit.Framework;

namespace Test.CSF.Collections.EventHandling.Impl
{
  [TestFixture]
  public class TestEventHandlingSet : EventHandlingCollectionTestBase
  {
    #region fields

    private ISet<Person> _source;

    #endregion

    #region setup

    protected override void AdditionalSetup()
    {
      _source = new HashSet<Person>(SourceCollection);
    }

    #endregion

    #region tests

    [Test]
    public void Constructor_does_not_raise_an_exception()
    {
      // Act
      var sut = new EventHandlingSet<Person>(_source);

      // Assert
      Assert.NotNull(sut);
    }

    // TODO: Add tests for Add, AddAll, RemoveAll, SymmetricExceptWith and UnionWith

    #endregion
  }
}

