//
// TestEventHandlingList.cs
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
using NUnit.Framework;
using Test.CSF.Collections.EventRaising.Mocks;
using CSF.Collections.EventRaising;
using System.Collections.Specialized;

namespace Test.CSF.Collections.EventRaising
{
    [TestFixture]
    public class TestEventRaisingList : EventRaisingCollectionTestBase
    {
        #region fields

        private IList<Person> _source;

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
            var sut = new EventRaisingList<Person> (_source);

            // Assert
            Assert.NotNull (sut);
        }

        [Test]
        public void Get_Item_returns_correct_item ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            // Act
            var result = sut [1];

            // Assert
            Assert.AreEqual (_source [1], result);
        }

        [Test]
        public void Set_Item_modifies_source_list ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            // Act
            sut [1] = Replacement;

            // Assert
            Assert.AreSame (Replacement, _source [1]);
        }

        [Test]
        public void Set_Item_invokes_before_add_callback ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.BeforeAdd += RecordingCallbackOne;

            // Act
            sut [1] = Replacement;

            sut.BeforeAdd -= RecordingCallbackOne;

            // Assert
            Assert.IsTrue (CallbackOneCalled);
        }

        [Test]
        public void Set_Item_invokes_after_add_callback ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.AfterAdd += RecordingCallbackOne;

            // Act
            sut [1] = Replacement;

            sut.AfterAdd -= RecordingCallbackOne;

            // Assert
            Assert.IsTrue (CallbackOneCalled);
        }

        [Test]
        public void Set_Item_does_not_replace_item_if_before_add_cancels ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.BeforeAdd += CancellingCallback;

            // Act
            sut [1] = Replacement;

            sut.BeforeAdd -= CancellingCallback;

            // Assert
            Assert.AreNotSame (Replacement, _source [1]);
        }

        [Test]
        public void Set_Item_does_not_trigger_after_add_if_before_add_cancels ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.BeforeAdd += CancellingCallback;
            sut.AfterAdd += RecordingCallbackOne;

            // Act
            sut [1] = Replacement;

            sut.BeforeAdd -= CancellingCallback;
            sut.AfterAdd -= RecordingCallbackOne;

            // Assert
            Assert.IsFalse (CallbackOneCalled);
        }

        [Test]
        public void RemoveAt_triggers_both_remove_events ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.BeforeRemove += RecordingCallbackOne;
            sut.AfterRemove += RecordingCallbackTwo;

            // Act
            sut.RemoveAt (1);

            sut.BeforeRemove -= RecordingCallbackOne;
            sut.AfterRemove -= RecordingCallbackTwo;

            // Assert
            Assert.IsTrue (CallbackOneCalled, "Callback one");
            Assert.IsTrue (CallbackTwoCalled, "Callback two");
        }

        [Test]
        public void Insert_triggers_both_add_events ()
        {
            // Arrange
            var sut = new EventRaisingList<Person> (_source);

            sut.BeforeAdd += RecordingCallbackOne;
            sut.AfterAdd += RecordingCallbackTwo;

            // Act
            sut.Insert (1, new Person ());

            sut.BeforeRemove -= RecordingCallbackOne;
            sut.AfterRemove -= RecordingCallbackTwo;

            // Assert
            Assert.IsTrue (CallbackOneCalled, "Callback one");
            Assert.IsTrue (CallbackTwoCalled, "Callback two");
        }

        [Test]
        public void Insert_triggers_CollectionChanged()
        {
            var sut = new EventRaisingList<Person> (_source);
            NotifyCollectionChangedEventArgs capturedArgs = default;
            void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) => capturedArgs = args;
            sut.CollectionChanged += OnCollectionChanged;

            sut.Insert(1, new Person());

            sut.CollectionChanged -= OnCollectionChanged;

            Assert.That(capturedArgs, Is.Not.Null);
        }

        [Test]
        public void RemoveAt_triggers_CollectionChanged()
        {
            var sut = new EventRaisingList<Person> (_source);
            NotifyCollectionChangedEventArgs capturedArgs = default;
            void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) => capturedArgs = args;
            sut.CollectionChanged += OnCollectionChanged;

            sut.RemoveAt(1);

            sut.CollectionChanged -= OnCollectionChanged;

            Assert.That(capturedArgs, Is.Not.Null);
        }

        #endregion
    }
}

