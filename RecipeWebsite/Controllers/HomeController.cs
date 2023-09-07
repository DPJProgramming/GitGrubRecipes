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
            String apiKey = ApiHelper.apiKey();

            //Http request for api
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            //get response from api
            string nutrition = await client.GetStringAsync(
                 "https://api.calorieninjas.com/v1/nutrition?query=" + foodText);

            //create dynamic object from nutrition to later convert to a list
            var jsonData = JsonConvert.DeserializeObject<dynamic>(nutrition);

            return PartialView("_NutritionModal", createFoodList(jsonData));
        }

        /// <summary>
        /// Creates a list of food items for return to the nutrition modal
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public List<NutritionInfoDTO> createFoodList(dynamic jsonData) {
            List<NutritionInfoDTO> foodList = new();

            foreach (var info in jsonData["items"]) {
                NutritionInfoDTO food = new NutritionInfoDTO();

                food.name = info["name"].ToString();
                food.calories = Math.Round((float)info["calories"], 2);
                food.serving_size_g = (int)info["serving_size_g"];
                food.fat_total_g = Math.Round((float)info["fat_total_g"], 2);
                food.fat_saturated_g = Math.Round((float)info["fat_saturated_g"], 2);
                food.protein_g = Math.Round((float)info["protein_g"], 2);
                food.sodium_mg = (int)info["sodium_mg"];
                food.potassium_mg = (int)info["potassium_mg"];
                food.cholesterol_mg = (int)info["cholesterol_mg"];
                food.carbohydrates_total_g = Math.Round((float)info["carbohydrates_total_g"], 2);
                food.fiber_g = Math.Round((float)info["fiber_g"], 2);
                food.sugar_g = Math.Round((float)info["sugar_g"], 2);

                foodList.Add(food);
            }
            return foodList;
        }
    }

}