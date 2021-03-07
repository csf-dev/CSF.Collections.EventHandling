using System.Collections.Generic;

namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// Implementation of <c>EventArgs</c> for events which are raised after the successful modification of a list.
    /// </summary>
    public class AfterModifyListEventArgs<TItem> : AfterModifyEventArgs<TItem>, IHasListIndex where TItem : class
    {
        /// <summary>
        /// Gets the zero-based index of the item, relative to the context of the current operation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The meaning of this property depends upon the context of the event arguments.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>For <see cref="AfterModifyListEventArgs{TItem}"/>, representing a removal, this
        /// is the index of the item before it was removed.</description>
        /// </item>
        /// <item>
        /// <description>For <see cref="AfterModifyListEventArgs{TItem}"/>, representing an addition, this
        /// is the index of the item where it now resides (after it has been added).</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <value>The index of the item.</value>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterModifyListEventArgs{TItem}"/>
        /// class.
        /// </summary>
        /// <param name="collection">The collection which is to be modified.</param>
        /// <param name="item">The item to be added or removed to/from the collection.</param>
        /// <param name="index">The index of the item.</param>
        public AfterModifyListEventArgs(ICollection<TItem> collection, TItem item, int index)
            : base(collection, item)
        {
            Index = index;
        }
    }
}