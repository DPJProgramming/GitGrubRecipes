namespace RecipeWebsite.Models {
    public static class ApiHelper {
        public static string apiKey() {
            var builder = WebApplication.CreateBuilder();
            return builder.Configuration.GetSection("NutritionApi").Value;
        }
    }
}
