namespace CorsLite.Business.Models
{
    /// <summary>
    /// Abstraction of the result of the command so we can 
    /// check on the success and handle errors nicely.
    /// </summary>
    public class Result : IResult
    {
        public bool IsOk => Error == null;

        public Error Error { get; set; }
    }
}