using CorsLite.Business;
using CorsLite.Business.Commands;
using CorsLite.Mvc.Models;
using CorsLite.Mvc.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CorsLite.Mvc.Controllers
{
    /// <summary>
    /// would prefer to have the api in a seperate project to the front end
    /// (maybe react) 
    /// </summary>
    [Route("/api/drink/lemontea")]
    public class LemonTeaController : Controller
    {
        private readonly IHubContext<DisplayHub> hubContext;
        private readonly IBroker broker;

        public LemonTeaController(IHubContext<DisplayHub> hubContext, IBroker broker)
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
            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Preparing your lemon tea..."));
            // TODO: use errors not just bools for things going wrong

            if (!await BoilWaterAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await SteepWaterAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await PourTeaAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);
            else if (!await AddLemonAsync())
                return StatusCode((int)HttpStatusCode.InternalServerError);

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Enjoy your lemon tea! <3"));
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

        private async Task<bool> SteepWaterAsync()
        {
            var steep = new SteepWaterCommand();
            var doSteep = await broker.ExecuteAsync(steep);

            if (!doSteep.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Forgot to put the tea in the water"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Tea has Steeped in Water"));
            return true;
        }

        private async Task<bool> PourTeaAsync()
        {
            var pour = new PourDrinkCommand();
            var doPour = await broker.ExecuteAsync(pour);

            if (!doPour.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Tried to pour but missed the cup"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Poured the Tea into the cup"));
            return true;
        }

        private async Task<bool> AddLemonAsync()
        {
            var add = new SteepWaterCommand();
            var doAdd = await broker.ExecuteAsync(add);

            if (!doAdd.IsOk)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Whatever we just added... was not lemon"));
                return false;
            }

            await hubContext.Clients.All.SendAsync("ReceiveMessage", new NotificationRequest("Added lemon to the tea"));
            return true;
        }
    }
}
