﻿//
// ReplaceCollectionEventArgs.cs
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
using System.Collections;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Implementation of <c>EventArgs</c> for events which are raised after the replacement of a collection.
    /// </summary>
    public class AfterReplaceEventArgs<TCollection> : EventArgs
    {
        #region properties

        /// <summary>
        /// Gets the original collection to be replaced.
        /// </summary>
        /// <value>The original collection.</value>
        public TCollection Original {
            get;
            private set;
        }

        /// <summary>
        /// Gets the replacement collection.
        /// </summary>
        /// <value>The replacement collection.</value>
        public TCollection Replacement {
            get;
            private set;
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.Collections.EventRaising.AfterReplaceEventArgs{TCollection}"/> class.
        /// </summary>
        /// <param name="original">The original collection.</param>
        /// <param name="replacement">The replacement collection.</param>
        public AfterReplaceEventArgs (TCollection original, TCollection replacement)
        {
            Original = original;
            Replacement = replacement;
        }

        #endregion
    }
}

