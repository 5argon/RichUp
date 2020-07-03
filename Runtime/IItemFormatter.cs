namespace E7.RichUp
{
    /// <summary>
    /// Add a component implementing this interface on the same
    /// GameObject as <see cref="RichUpPreprocessor"/> to process items in {} brackets.
    /// </summary>
    public interface IItemFormatter
    {
        /// <summary>
        /// Please return what {item_name} supposed to be for each item_name.
        /// </summary>
        /// <param name="itemName">Name in the { }</param>
        string FormatItem(string itemName);
    }
}