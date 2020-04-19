//
// TestEventHandlingCollectionBase.cs
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
using Test.CSF.Collections.EventRaising.Mocks;
using System.Collections.Generic;
using NUnit.Framework;
using CSF.Collections.EventRaising;
using System.Linq;

namespace Test.CSF.Collections.EventRaising
{
    [TestFixture]
    public class TestEventRaisingCollection : EventRaisingCollectionTestBase
    {
        #region fields

        private ICollection<Person> _source;

        #endregion

        #region setup

        protected override void AdditionalSetup ()
        {
            _source = new List<Person> (SourceCollection);
        }

        #endregion

        #region tests

        [Test]
        public void Constructor_does_not_raise_an_exception ()
        {
            // Act
            var sut = new EventRaisingCollection<Person> (_source);

            // Assert
            Assert.NotNull (sut);
        }

        [Test]
        public void Add_triggers_both_add_events ()
        {
            // Arrange
            var sut = new EventRaisingCollection<Person> (_source);

            sut.BeforeAdd += RecordingCallbackOne;
            sut.AfterAdd += RecordingCallbackTwo;

            // Act
            sut.Add (new Person ());

            sut.BeforeRemove -= RecordingCallbackOne;
            sut.AfterRemove -= RecordingCallbackTwo;

            // Assert
            Assert.IsTrue (CallbackOneCalled, "Callback one");
            Assert.IsTrue (CallbackTwoCalled, "Callback two");
        }

        [Test]
        public void Remove_triggers_both_remove_events ()
        {
            // Arrange
            var sut = new EventRaisingCollection<Person> (_source);

            sut.BeforeRemove += RecordingCallbackOne;
            sut.AfterRemove += RecordingCallbackTwo;

            // Act
            sut.Remove (SourceCollection.First ());

            sut.BeforeRemove -= RecordingCallbackOne;
            sut.AfterRemove -= RecordingCallbackTwo;

            // Assert
            Assert.IsTrue (CallbackOneCalled, "Callback one");
            Assert.IsTrue (CallbackTwoCalled, "Callback two");
        }

        [Test]
        public void Clear_triggers_both_remove_events_for_every_item ()
        {
            // Arrange
            int beforeRemoveCalls = 0, afterRemoveCalls = 0;
            var sut = new EventRaisingCollection<Person> (_source);

            EventHandler<BeforeModifyEventArgs<Person>> incrementBefore = (sender, e) => beforeRemoveCalls++;
            EventHandler<AfterModifyEventArgs<Person>> incrementAfter = (sender, e) => afterRemoveCalls++;

            sut.BeforeRemove += incrementBefore;
            sut.AfterRemove += incrementAfter;

            // Act
            sut.Clear ();

            sut.BeforeRemove -= incrementBefore;
            sut.AfterRemove -= incrementAfter;

            // Assert
            Assert.AreEqual (SourceCollection.Count, beforeRemoveCalls, "Before remove");
            Assert.AreEqual (SourceCollection.Count, afterRemoveCalls, "After remove");
        }

        #endregion
    }
}

