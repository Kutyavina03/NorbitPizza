using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorbitPizza.Models;
using NorbitPizza.Services;

namespace NorbitPizza.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PizzaService _pizzaService;

        public List<Pizza> Pizzas { get; set; } = new();
        public List<string> Categories { get; set; } = new() { "Все", "Классические", "Острые", "Вегетарианские", "Мясные", "Сладкие" };
        public List<string> AvailableIngredients { get; set; } = new();

        [BindProperty] public string? NewPizzaName { get; set; }
        [BindProperty] public List<string> SelectedIngredients { get; set; } = new();

        public IndexModel(PizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        public async Task OnGetAsync(string category = "Все")
        {
            Pizzas = await _pizzaService.GetPizzasAsync(category);
            AvailableIngredients = _pizzaService.GetAvailableIngredients();
        }

        public async Task<IActionResult> OnPostCreatePizzaAsync()
        {
            if (string.IsNullOrEmpty(NewPizzaName) || !SelectedIngredients.Any())
            {
                TempData["Error"] = "Заполните название и выберите ингредиенты";
                return RedirectToPage();
            }

            await _pizzaService.CreateCustomPizzaAsync(NewPizzaName, SelectedIngredients);
            TempData["Success"] = "Пицца успешно создана!";
            return RedirectToPage();
        }
    }
}