//
// EventHandlingSet.cs
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
using System.Linq;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Implementation of <see cref="T:EventRaisingCollectionBase{TItem}"/> for the generic <c>ISet</c>.
    /// </summary>
    public class EventRaisingSet<TItem> : EventRaisingCollectionBase<TItem>, ISet<TItem>
        where TItem : class
    {
        /// <summary>
        /// Adds an item to the current instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the instance was added (IE: it was not already contained); <c>false</c> otherwise.
        /// </returns>
        /// <param name='item'>
        /// The item to add.
        /// </param>
        public new bool Add (TItem item)
        {
            if (HandleBeforeAdd (item)) {
                var output = GetSourceCollection ().Add(item);

                if (output) {
                    HandleAfterAdd (item);
                    HandleAdditionChange(item);
                }

                return output;
            }

            return false;
        }

        /// <summary>
        /// Adds all of the given items to the current collection.
        /// </summary>
        /// <returns>
        /// <c>true</c> if at least one of the instances was added (IE: it was not already contained); <c>false</c>
        /// otherwise.
        /// </returns>
        /// <param name='c'>
        /// The items to add.
        /// </param>
        public bool AddAll (IEnumerable<TItem> c)
        {
            if (c == null) {
                throw new ArgumentNullException (nameof (c));
            }

            bool output = false;

            foreach (var item in c) {
                output |= this.Add (item);
            }

            return output;
        }

        /// <summary>
        /// Removes all of the given items from the current collection.
        /// </summary>
        /// <returns>
        /// <c>true</c> if at least one of the instances was removed (IE: it was previously contained); <c>false</c>
        /// otherwise.
        /// </returns>
        /// <param name='c'>
        /// The items to remove.
        /// </param>
        public bool RemoveAll (ICollection<TItem> c)
        {
            if (c == null) {
                throw new ArgumentNullException (nameof (c));
            }

            bool output = false;

            foreach (var item in c) {
                output |= this.Remove (item);
            }

            return output;
        }

        /// <summary>
        /// Removes all of the given items from the current collection.
        /// </summary>
        /// <param name='other'>The items to remove.</param>
        public void ExceptWith (IEnumerable<TItem> other)
        {
            if (other == null) throw new ArgumentNullException (nameof (other));

            RemoveAll (other.ToArray ());
        }

        /// <summary>
        /// Removes items from the current collection, except those which exist in the given collection.
        /// </summary>
        /// <param name='other'>The items to keep.</param>
        public void IntersectWith (IEnumerable<TItem> other)
        {
            if (other == null) throw new ArgumentNullException (nameof (other));

            ExceptWith (GetSourceCollection ().Except (other));
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool IsProperSubsetOf (IEnumerable<TItem> other) => GetSourceCollection ().IsProperSubsetOf (other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool IsProperSupersetOf (IEnumerable<TItem> other) => GetSourceCollection ().IsProperSupersetOf (other);

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool IsSubsetOf (IEnumerable<TItem> other) => GetSourceCollection ().IsSubsetOf (other);

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool IsSupersetOf (IEnumerable<TItem> other) => GetSourceCollection ().IsSupersetOf (other);

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool Overlaps (IEnumerable<TItem> other) => GetSourceCollection ().Overlaps (other);

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public bool SetEquals (IEnumerable<TItem> other) => GetSourceCollection ().SetEquals (other);

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in 
        /// the specified collection, but not both.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public void SymmetricExceptWith (IEnumerable<TItem> other)
        {
            if (other == null) throw new ArgumentNullException (nameof (other));

            var source = GetSourceCollection ();

            var toRemove = source.Intersect (other);
            var toAdd = other.Except (source);

            ExceptWith (toRemove);
            AddAll (toAdd);
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the current set, in the
        /// specified collection, or in both.
        /// </summary>
        /// <param name='other'>The other collection.</param>
        public void UnionWith (IEnumerable<TItem> other)
        {
            if (other == null) throw new ArgumentNullException (nameof (other));

            var source = GetSourceCollection ();

            var elementsToAdd = source
              .Union (other)
              .Distinct ()
              .Except (source);

            AddAll (elementsToAdd);
        }

        /// <summary>
        /// Gets a strongly-typed representation of the <see cref="P:SourceCollection"/>.
        /// </summary>
        /// <returns>The source collection.</returns>
        protected ISet<TItem> GetSourceCollection () => (ISet<TItem>)SourceCollection;

        /// <summary>
        /// Creates a set of appropriately-populated before-action event arguments.
        /// </summary>
        /// <returns>The before-action event arguments.</returns>
        /// <param name="item">Item.</param>
        protected override BeforeModifyEventArgs<TItem> CreateBeforeActionEventArgs (TItem item)
            => new BeforeModifyEventArgs<TItem> (SourceCollection, item);

        /// <summary>
        /// Creates a set of appropriately-populated after-action event arguments.
        /// </summary>
        /// <returns>The after-action event arguments.</returns>
        /// <param name="item">The associated item.</param>
        protected override AfterModifyEventArgs<TItem> CreateAfterActionEventArgs (TItem item)
            => new AfterModifyEventArgs<TItem> (SourceCollection, item);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventRaisingSet{TItem}"/> class.
        /// </summary>
        /// <param name='source'>The source collection that this instance wraps.</param>
        public EventRaisingSet (ISet<TItem> source) : base (source) { }
    }
}

