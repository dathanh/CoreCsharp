namespace Framework.Service.Diagnostics
{
    /// <summary>
    ///     The type of method that is passed into e.g. "/>
    ///     and allows the callback method to "submit" it's message to the underlying output system.
    /// </summary>
    /// <param name="format">
    ///     the format argument as in <see cref="string.Format(string,object[])" />
    /// </param>
    /// <param name="args">
    ///     the argument list as in <see cref="string.Format(string,object[])" />
    /// </param>
    /// <author>Erich Eichinger</author>
    public delegate string FormatMessageHandler(string format, params object[] args);
}
