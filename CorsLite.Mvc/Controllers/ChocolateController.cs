using CorsLite.Business;
using CorsLite.Business.Commands;
using CorsLite.Mvc.Models;
using CorsLite.Mvc.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Threading.Tasks;

namespace CorsLite.Mvc.Controllers
{
    /// <summary>
    /// would prefer to have the api in a seperate project to the front end
    /// (maybe react) 
    /// </summary>
    [Route("/api/drink/chocolate")]
    public class ChocolateController : Controller
    {
        private readonly IHubContext<DisplayHub> hubContext;
        private readonly IBroker broker;

        public ChocolateController(IHubContext<DisplayHub> hubContext, IBroker broker)
        {
            this.hubContext = hubContext;
            this.broker = broker;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeLemonTeaAsync()
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Preparing your hot chocolate..."));
            // TODO: use errors not just bools for things going wrong

            if (!await BoilWaterAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await AddChocolatePowderAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await PourChocolateAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Enjoy your hot chocolate! <3"));
            return Ok();
        }

        private async Task<bool> BoilWaterAsync()
        {
            var boil = new BoilWaterCommand();
            var doBoil = await broker.ExecuteAsync(boil);

            if (!doBoil.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Forgot to put the kettle on :("));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Water has boiled"));
            return true;
        }

        private async Task<bool> AddChocolatePowderAsync()
        {
            var add = new SteepWaterCommand();
            var doAdd = await broker.ExecuteAsync(add);

            if (!doAdd.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Forgot to put the hot chocolate in the water"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Added the Hot Chocolate powder"));
            return true;
        }

        private async Task<bool> PourChocolateAsync()
        {
            var pour = new PourDrinkCommand();
            var doPour = await broker.ExecuteAsync(pour);

            if (!doPour.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Tried to pour but missed the cup"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Poured the Hot Chocolate into the cup"));
            return true;
        }
    }
}
