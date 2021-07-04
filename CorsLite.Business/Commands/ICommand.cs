using CorsLite.Business.Models;
using System.Threading.Tasks;

namespace CorsLite.Business.Commands
{
    public interface ICommand
    {
        public Task<IResult> ExecuteAsync();
    }
}
