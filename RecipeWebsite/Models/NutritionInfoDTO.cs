using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeWebsite.Models {

    /// <summary>
    /// A class used to hold/transfer data for nutrition information
    /// </summary>
    public class NutritionInfoDTO {
        public string name { get; set; }
        public double calories { get; set; }
        public double serving_size_g { get; set; }
        public double fat_total_g { get; set; }
        public double fat_saturated_g { get; set; }
        public double protein_g { get; set; }
        public int sodium_mg { get; set; }
        public int potassium_mg { get; set; }
        public int cholesterol_mg { get; set; }
        public double carbohydrates_total_g { get; set; }
        public double fiber_g { get; set; }
        public double sugar_g { get; set; }
    }
}
