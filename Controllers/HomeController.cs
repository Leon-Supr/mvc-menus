using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Menus.Models;
using System.Text.Json;
using menus.Models;
using menus.Services;
using AspNetCoreGeneratedDocument;

namespace Menus.Controllers;

public class HomeController : Controller
{

    [HttpGet("Home/GetPokemon/{name}")]
    public async Task<IActionResult> GetPokemon(string name)
    {
        String url = $"https://pokeapi.co/api/v2/pokemon/{name}";
        HttpResponseMessage response =
             await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        String json = await response
                           .Content
                           .ReadAsStringAsync();
        JsonDocument doc = JsonDocument
                          .Parse(json);
        Pokemon pokemon = new Pokemon
        {
            Name = doc.RootElement.GetProperty("name").GetString(),
            ImageUrl = doc
                   .RootElement
                   .GetProperty("sprites")
                   .GetProperty("front_default")
                   .GetString(),
            Types = [.. doc.RootElement.GetProperty("types")
           .EnumerateArray()
           .Select(t => t.GetProperty("type")
                       .GetProperty("name")
                       .GetString())],
            Abilities = [.. doc.RootElement.GetProperty("abilities")
                .EnumerateArray()
                .Select(a => a.GetProperty("ability")
                              .GetProperty("name")
                              .GetString())],
            Weight = doc.RootElement.GetProperty("weight").GetInt32(),
        };

        return View(pokemon);
    }

    [HttpGet("Home/GetJoke/{lang}")]
    public async Task<IActionResult> GetJoke(string lang)
    {
        var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
        if (isAuthenticated != "true")
        {
            return RedirectToAction("Login");
        }

        string url = $"https://v2.jokeapi.dev/joke/Programming?lang={lang}&type=single";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        string json = await response
                           .Content
                           .ReadAsStringAsync();
        JsonDocument doc = JsonDocument
                          .Parse(json);
        Joke joke = new Joke
        {
            joke = doc.RootElement.GetProperty("joke").GetString()
        };

        return View(joke);
    }

    private readonly IAuthService authService;
    private readonly HttpClient httpClient;

    public HomeController(HttpClient httpClient)
    {
        this.httpClient = new HttpClient();
        this.authService = new SimpleAuthService();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Photos()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Login(User user)
    {
        if (authService.Authenticate(user.Username, user.Password))
        {
            HttpContext.Session.SetString("UserAuthenticated", "true");
            return RedirectToAction("Welcome");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }
    }

    [HttpPost]
    public ActionResult LogOut(User user)
    {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
    }

    public IActionResult Welcome()
    {
        var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
        if (isAuthenticated != "true")
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
