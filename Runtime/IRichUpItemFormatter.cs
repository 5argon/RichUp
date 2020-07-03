namespace E7.RichUp
{
    public interface IRichUpItemFormatter
    {
        /// <summary>
        /// Please return what {item_name} supposed to be for each item_name.
        /// </summary>
        /// <param name="itemName">Name in the { }</param>
        string FormatItem(string itemName);
    }
}