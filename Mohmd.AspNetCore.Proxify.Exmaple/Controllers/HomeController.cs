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
            await _sampleService.SumAsync(1, 2);
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
    }
}
