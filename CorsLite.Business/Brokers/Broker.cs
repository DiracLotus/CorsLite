using CorsLite.Business.Commands;
using CorsLite.Business.Models;
using System.Threading.Tasks;

namespace CorsLite.Business
{
    /// <summary>
    /// Command broker to enable easy DI and execution of the commands.
    /// In reality we'd probably want to implement something
    /// like MediatR to handle this side of things.
    /// </summary>
    public class Broker : IBroker
    {
        public async Task<IResult> ExecuteAsync(ICommand command)
        {
            return await command.ExecuteAsync();
        }
    }
}
