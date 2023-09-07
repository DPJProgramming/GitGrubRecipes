using Microsoft.AspNetCore.Mvc;
using RecipeWebsite.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient.Server;
using NuGet.ContentModel;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Json;
using AngleSharp.Dom;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using System.Xml.Linq;
using System.Net.Http.Headers;
using static System.Net.Http.HttpClient;

namespace RecipeWebsite.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Recieves data containing information from the Nutrition API, 
        /// and passes it as a list of NutritionInfoDTO objects to the partial view for display
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NutritionModal([FromBody] string foodText) {

            //String foodQuery = foodText.GetProperty("Food").GetString();
            String apiKey = ApiHelper.apiKey();

            //Http request for api
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("https://api.calorieninjas.com/v1"));
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            //client.DefaultVersionPolicy

            string nutrition = await client.GetStringAsync(
                 "https://api.calorieninjas.com/v1/nutrition?query=" + foodText);

            //rest of code
            var jsonData = JsonConvert.DeserializeObject<dynamic>(nutrition);

            List<NutritionInfoDTO> foodList = new List<NutritionInfoDTO>();

            foreach (var data in jsonData["items"]) {
                NutritionInfoDTO food = new NutritionInfoDTO();

                food.name = data["name"].ToString();
                food.calories = Math.Round((float)data["calories"], 2);
                food.serving_size_g = (int)data["serving_size_g"];
                food.fat_total_g = Math.Round((float)data["fat_total_g"], 2);
                food.fat_saturated_g = Math.Round((float)data["fat_saturated_g"], 2);
                food.protein_g = Math.Round((float)data["protein_g"], 2);
                food.sodium_mg = (int)data["sodium_mg"];
                food.potassium_mg = (int)data["potassium_mg"];
                food.cholesterol_mg = (int)data["cholesterol_mg"];
                food.carbohydrates_total_g = Math.Round((float)data["carbohydrates_total_g"], 2);
                food.fiber_g = Math.Round((float)data["fiber_g"], 2);
                food.sugar_g = Math.Round((float)data["sugar_g"], 2);

                foodList.Add(food);
            }
            return PartialView("_NutritionModal", foodList);
        }
    }

}