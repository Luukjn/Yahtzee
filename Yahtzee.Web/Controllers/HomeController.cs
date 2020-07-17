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
using Yahtzee.Model;
using Yahtzee.Model.Data;
using Microsoft.Extensions.Caching.Memory;
using Yahtzee.Web.Definitions;

namespace Yahtzee.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!_cache.TryGetValue(CacheKeys.Combinations, out Combinations combinations))
            {
                combinations = new Combinations();
                _cache.Set(CacheKeys.Combinations, combinations);
            }

            return View(combinations);
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

        public IActionResult RollDice(int diceCount)
        {
            var dice = new List<Die>();
            for(int i = 0; i < diceCount; i++)
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

        public IActionResult AddCombination(CombinationsEnum type, List<Die> dice)
        {
            if (!_cache.TryGetValue(CacheKeys.Combinations, out Combinations combinations))
            {
                combinations = new Combinations();
                _cache.Set(CacheKeys.Combinations, combinations);
            }
            combinations.AddDiceToCombination(type, dice);
            _cache.Set(CacheKeys.Combinations, combinations);
            return Ok();
        }
    }
}
