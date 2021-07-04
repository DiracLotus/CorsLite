using CorsLite.Business.Models;
using System.Threading;
using System.Threading.Tasks;

namespace CorsLite.Business.Commands
{
    public abstract class Command : ICommand
    {
        public IResult GetResult()
        {
            return new Result();
        }
        public IResult GetErrorResult(Error error)
        {
            return new Result { Error = error };
        }

        /// <summary>
        /// here we could call external api, talk to machines etc
        /// to actually make the coffee, for now, we'll pretend we're
        /// doing stuff by sleeping a bit
        /// !We'd override this with command-specific instructions in
        /// each command implementation!
        /// </summary>
        public virtual Task<IResult> ExecuteAsync()
        {
            try
            {
                Thread.Sleep(1000);
                return Task.FromResult(GetResult());
            }
            catch
            {
                // handle specific errors gracefully
                // if we're out of water
                // return Task.FromResult(GetErrorResult(new NoWaterError());

                return Task.FromResult(GetErrorResult(new Error()));

                // throw; // if the exception is completely unexpected
            }
        }

        /// <summary>
        /// here we could call external api, talk to machines etc
        /// to actually make the coffee, for now, we'll pretend we're
        /// doing stuff by sleeping a bit
        /// !We'd override this with command-specific instructions in
        /// each command implementation!
        /// </summary>
        public virtual Task<IResult> ExecuteAsync<T>(T data)
        {
            try
            {
                Thread.Sleep(1000);
                return Task.FromResult(GetResult());
            }
            catch
            {
                // handle specific errors gracefully
                // if we're out of water
                // return Task.FromResult(GetErrorResult(new NoWaterError());

                return Task.FromResult(GetErrorResult(new Error()));

                // throw; // if the exception is completely unexpected
            }
        }
    }
}
