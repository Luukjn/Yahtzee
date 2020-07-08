using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yahtzee.Web.Models;
using Yahtzee.Model.Domain;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Yahtzee.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RollDice()
        {
            var dice = new List<Die>();
            for(int i = 0; i < 5; i++)
            {
                dice.Add(new Die
                {
                    Sides = 6
                });
            }

            foreach(Die die in dice)
            {
                die.Roll();
            }

            var jsonResult = JsonConvert.SerializeObject(dice);
            return Content(jsonResult, MediaTypeNames.Application.Json);
        }
    }
}
