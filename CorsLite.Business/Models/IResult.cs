namespace CorsLite.Business.Models
{
    /// <summary>
    /// Abstraction of the result of the command so we can 
    /// check on the success and handle errors nicely.
    /// </summary>
    public interface IResult
    {
        public bool IsOk { get; }
        public Error Error { get; }
    }
}
