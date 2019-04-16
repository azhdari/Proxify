using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mohmd.AspNetCore.Proxify.Exmaple.Models;
using Mohmd.AspNetCore.Proxify.Exmaple.Services;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISampleService _sampleService;

        public HomeController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        public async Task<IActionResult> Index()
        {
            var x = _sampleService.Sum(1, 3);

            await _sampleService.SumAsync(1, 2);
            await _sampleService.SetName("Mohammad Azhdari");
            object name = await _sampleService.GetName();
            return View(name);
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
    }
}
