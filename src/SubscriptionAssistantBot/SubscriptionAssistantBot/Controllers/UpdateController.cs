using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionAssistantBot.Services;
using Telegram.Bot.Types;

namespace SubscriptionAssistantBot.Controllers
{
    [Route("api/update")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService _updateService;

        public UpdateController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        // POST api/update
        [HttpGet]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            var message = update.Message;
            await _updateService.BotService.Api.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
            return Ok();
        }
    }
}