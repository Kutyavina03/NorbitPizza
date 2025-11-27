using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorbitPizza.Data;
using NorbitPizza.Models;
using System.Text.Json;

namespace NorbitPizza.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CartModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string OrderData { get; set; }  

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string UserPhone { get; set; }

        [BindProperty]
        public string UserAddress { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(OrderData) || string.IsNullOrEmpty(UserName)
                || string.IsNullOrEmpty(UserPhone) || string.IsNullOrEmpty(UserAddress))
            {
                ModelState.AddModelError("", "Заполните все поля!");
                return Page();
            }

            var items = JsonSerializer.Deserialize<List<CartItemDto>>(OrderData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var order = new Order
            {
                CustomerName = UserName,
                CustomerPhone = UserPhone,
                CustomerAddress = UserAddress,
                TotalPrice = items.Sum(i => i.Price * i.Quantity),
                Items = items.Select(i => new OrderItem
                {
                    PizzaId = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Очищаем корзину на клиенте через JS
            TempData["OrderSuccess"] = true;
            return RedirectToPage("/Index");
        }
    }

    public class CartItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
        }
}
