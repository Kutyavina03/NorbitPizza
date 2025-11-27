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
            new Pizza
            {
                Id = 1,
                Name = "Пицца Жюльен",
                Description = "Запечённое куриное филе, шампиньоны, лук, сливочно-грибной соус, моцарелла и петрушка",
                Price = 520,
                ImageUrl = "/images/julien.png",
                Ingredients = new List<string> { "Куриное филе", "Шампиньоны", "Лук", "Сливочно-грибной соус", "Моцарелла", "Петрушка" },
                Category = "Meat"
            },
            new Pizza
            {
                Id = 2,
                Name = "Пицца 4 сыра",
                Description = "Сыр Рассольный, Моцарелла, Гауда, Маасдам, Чеддер и соус Пармеджано",
                Price = 600,
                ImageUrl = "/images/4cheese.png",
                Ingredients = new List<string> { "Сыр Рассольный", "Моцарелла", "Гауда", "Маасдам", "Чеддер", "Соус Пармеджано" },
                Category = "Vegetarian"
            },
            new Pizza
            {
                Id = 3,
                Name = "Пицца Пепперони",
                Description = "Ветчина, нежный сыр Моцарелла и колбаса Пепперони. Вкусная, пикантная пицца для любителей остренького",
                Price = 550,
                ImageUrl = "/images/pepperoni.png",
                Ingredients = new List<string> { "Ветчина", "Моцарелла", "Пепперони" },
                Category = "Spicy"
            },
            new Pizza
            {
                Id = 4,
                Name = "Пицца Итальянская",
                Description = "Сочная ветчина, нежный сыр Моцарелла, свежие шампиньоны и ароматная зелень петрушки под фирменным соусом из мякоти томатов",
                Price = 500,
                ImageUrl = "/images/italian.png",
                Ingredients = new List<string> { "Ветчина", "Моцарелла", "Шампиньоны", "Петрушка", "Соус из томатов" },
                Category = "Classic"
            },
            new Pizza
            {
                Id = 5,
                Name = "Пицца Груша-Блю Чиз",
                Description = "Сочетание нежной груши и сливочного вкуса Блю Чиз под тянущимся сыром Моцарелла",
                Price = 580,
                ImageUrl = "/images/pear-bluecheese.png",
                Ingredients = new List<string> { "Груша", "Блю Чиз", "Моцарелла" },
                Category = "Vegetarian"
            },
            new Pizza
            {
                Id = 6,
                Name = "Пицца Мясное трио",
                Description = "Нежная копченая грудка, сочный карбонад и ароматный бекон с соусом, сыром, томатом и маринованным огурчиком",
                Price = 650,
                ImageUrl = "/images/meat.png",
                Ingredients = new List<string> { "Копченая грудка", "Карбонад", "Бекон", "Соус", "Сыр", "Томат", "Маринованный огурчик" },
                Category = "Meat"
            }
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
            if (!string.IsNullOrEmpty(category) && category != "All")
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