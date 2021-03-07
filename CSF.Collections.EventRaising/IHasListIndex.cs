namespace CSF.Collections.EventRaising
{
    /// <summary>
    /// An object which provide the index at which an item is added or removed.
    /// </summary>
    public interface IHasListIndex
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
        /// <description>For <see cref="BeforeModifyListEventArgs{TItem}"/>, representing a removal, this
        /// is the index of the item before it is removed.</description>
        /// </item>
        /// <item>
        /// <description>For <see cref="BeforeModifyListEventArgs{TItem}"/>, representing an addition, this
        /// is the index of the item where it will be added.</description>
        /// </item>
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
        int Index { get; }
    }
}