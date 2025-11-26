using Microsoft.EntityFrameworkCore;
using NorbitPizza.Data;
using NorbitPizza.Models;

namespace NorbitPizza.Services
{
    public class PizzaService
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> _availableIngredients = new()
        {
            "Томатный соус", "Моцарелла", "Пепперони", "Ветчина", "Грибы", "Перец", "Лук", "Оливки", "Ананасы", "Бекон"
        };

        private readonly List<Pizza> _initialPizzas = new()
        {
            new Pizza { Id = 1, Name = "Маргарита", Description = "Классическая итальянская пицца", Price = 450, ImageUrl = "/images/margarita.jpg", Ingredients = new List<string> { "Томатный соус", "Моцарелла", "Базилик" }, Category = "Classic" },
            new Pizza { Id = 2, Name = "Пепперони", Description = "Острая пицца с колбасками", Price = 550, ImageUrl = "/images/pepperoni.jpg", Ingredients = new List<string> { "Томатный соус", "Моцарелла", "Пепперони" }, Category = "Spicy" },
            new Pizza { Id = 3, Name = "Гавайская", Description = "С ветчиной и ананасами", Price = 520, ImageUrl = "/images/hawaiian.jpg", Ingredients = new List<string> { "Томатный соус", "Моцарелла", "Ветчина", "Ананасы" }, Category = "Sweet" },
            new Pizza { Id = 4, Name = "Четыре сыра", Description = "Нежная пицца с четырьмя видами сыра", Price = 600, ImageUrl = "/images/4cheese.jpg", Ingredients = new List<string> { "Моцарелла", "Горгонзола", "Пармезан", "Фета" }, Category = "Vegetarian" },
            new Pizza { Id = 5, Name = "Мясная", Description = "Для настоящих мясоедов", Price = 650, ImageUrl = "/images/meat.jpg", Ingredients = new List<string> { "Томатный соус", "Моцарелла", "Пепперони", "Ветчина", "Бекон" }, Category = "Meat" },
            new Pizza { Id = 6, Name = "Вегетарианская", Description = "Полезно и вкусно", Price = 480, ImageUrl = "/images/vegetarian.jpg", Ingredients = new List<string> { "Томатный соус", "Моцарелла", "Грибы", "Перец", "Оливки", "Лук" }, Category = "Vegetarian" }
        };

        public PizzaService(ApplicationDbContext context)
        {
            _context = context;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!_context.Pizzas.Any())
            {
                _context.Pizzas.AddRange(_initialPizzas);
                _context.SaveChanges();
            }
        }

        public List<string> GetAvailableIngredients() => _availableIngredients;

        public async Task<List<Pizza>> GetPizzasAsync(string category = null)
        {
            var query = _context.Pizzas.AsQueryable();
            if (!string.IsNullOrEmpty(category) && category != "all")
                query = query.Where(p => p.Category == category);
            return await query.ToListAsync();
        }

        public async Task<Pizza> CreateCustomPizzaAsync(string name, List<string> ingredients)
        {
            var pizza = new Pizza
            {
                Name = name,
                Ingredients = ingredients,
                Price = 400 + (ingredients.Count * 50),
                IsCustom = true,
                Category = "Custom",
                ImageUrl = "/images/custom-pizza.jpg",
                Description = "Ваша собственная пицца"
            };

            _context.Pizzas.Add(pizza);
            await _context.SaveChangesAsync();
            return pizza;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}