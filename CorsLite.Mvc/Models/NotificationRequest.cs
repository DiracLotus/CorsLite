namespace CorsLite.Mvc.Models
{
    /// <summary>
    /// Wrap the signalr parameters so future modifications avoid
    /// breaking changes
    /// </summary>
    public class NotificationRequest
    {
        public string Text { get; set; }

        /// <summary>
        /// Should probably set up a factory to make these
        /// but will use ctor for expediancy sake
        /// </summary>
        public NotificationRequest(string text)
        {
            Text = text;
        }
    }
}
