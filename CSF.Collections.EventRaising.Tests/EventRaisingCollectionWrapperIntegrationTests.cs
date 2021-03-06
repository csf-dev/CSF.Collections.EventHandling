﻿//
// EventHandlingCollectionWrapperIntegrationTests.cs
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
    [TestFixture]
    public class EventRaisingCollectionWrapperIntegrationTests
    {
        #region fields

        private bool _beforeReplaceCalled, _afterReplaceCalled;

        #endregion

        #region setup

        [SetUp]
        public void Setup ()
        {
            _beforeReplaceCalled = false;
            _afterReplaceCalled = false;
        }

        #endregion

        #region tests

        [Test]
        public void Reference_filling_collection_backfills_parent_on_add ()
        {
            // Arrange
            var parent = new Parent ();
            var child = new Child ();

            // Act
            parent.Children.Add (child);

            // Assert
            Assert.AreSame (parent, child.Parent);
        }

        [Test]
        public void Reference_filling_collection_sets_parent_to_null_on_remove ()
        {
            // Arrange
            var parent = new Parent ();
            var child = new Child ();

            // Act
            parent.Children.Add (child);
            parent.Children.Remove (child);

            // Assert
            Assert.IsNull (child.Parent);
        }

        [Test]
        public void Replacing_the_collection_triggers_both_replace_events ()
        {
            // Arrange
            var wrapper = new EventRaisingSetWrapper<Child> (new HashSet<Child> ());

            var beforeReplaceHandler = GetBeforeReplaceHandler ();
            var afterReplaceHandler = GetAfterReplaceHandler ();

            wrapper.BeforeReplace += beforeReplaceHandler;
            wrapper.AfterReplace += afterReplaceHandler;

            // Act
            wrapper.SourceCollection = new HashSet<Child> ();

            wrapper.BeforeReplace -= beforeReplaceHandler;
            wrapper.AfterReplace -= afterReplaceHandler;

            // Assert
            Assert.IsTrue (_beforeReplaceCalled, "Before replace");
            Assert.IsTrue (_afterReplaceCalled, "After replace");
        }

        #endregion

        #region methods

        private EventHandler<BeforeReplaceEventArgs<ISet<Child>>> GetBeforeReplaceHandler ()
        {
            return (sender, e) => _beforeReplaceCalled = true;
        }

        private EventHandler<AfterReplaceEventArgs<ISet<Child>>> GetAfterReplaceHandler ()
        {
            return (sender, e) => _afterReplaceCalled = true;
        }

        #endregion
    }
}

