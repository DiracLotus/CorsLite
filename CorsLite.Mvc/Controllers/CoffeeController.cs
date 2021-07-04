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
    [Route("api/drink/coffee")]
    public class CoffeeController : Controller
    {
        private readonly IHubContext<DisplayHub> hubContext;
        private readonly IBroker broker;

        public CoffeeController(IHubContext<DisplayHub> hubContext, IBroker broker)
        {
            this.hubContext = hubContext;
            this.broker = broker;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeCoffeeAsync()
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Preparing your programming juice..."));
            // TODO: use errors not just bools for things going wrong

            if (!await BoilWaterAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await BrewCoffeeAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await PourCoffeeAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await AddMilkAndSugarAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Enjoy your coffee! <3"));
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

        private async Task<bool> BrewCoffeeAsync()
        {
            var brew = new SteepWaterCommand();
            var doBrew = await broker.ExecuteAsync(brew);

            if (!doBrew.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Failed to brew the coffee grounds"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Brewed the coffee grounds"));
            return true;
        }

        private async Task<bool> PourCoffeeAsync()
        {
            var pour = new PourDrinkCommand();
            var doPour = await broker.ExecuteAsync(pour);

            if (!doPour.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Tried to pour but missed the cup"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Poured the Coffee into the cup"));
            return true;
        }

        private async Task<bool> AddMilkAndSugarAsync()
        {
            var add = new AddExtraCommand();
            var doAdd = await broker.ExecuteAsync(add);

            if (!doAdd.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Added Baldrick's sugar substitute by mistake"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Added milk and sugar to the coffee"));
            return true;
        }
    }
}
