﻿using Microsoft.EntityFrameworkCore;
using NollyTickets.Ng.Models;

namespace NollyTickets.Ng.Data.Cart
{
    //For removing and adding  data to the shopping cart
    public class ShoppingCart
    {
        public ApplicationDbContext _context { get; set; }

        public string ShopppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context; 
        }
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            //Check for cartId session. if it is null generate a new sesiom Id
            string cartId = session.GetString("cartId") ?? Guid.NewGuid().ToString();
            //set the session
            session.SetString("cartId", cartId);

            return new ShoppingCart(context) { ShopppingCartId = cartId };

        }

        //Adding Item to shopping cart
        public void AddItemToCart(Movie movie)
        {
            //Check if the shopping cart item is in the database
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(x  => x.Movie.Id == movie.Id &&
            x.ShopppingCartId == ShopppingCartId);
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShopppingCartId = ShopppingCartId,
                    Movie = movie,
                    Amount = 1,
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }
        //Remove item from shopping cart
        public void RemoveItemFromCart(Movie movie)
        {
            //Check if the shopping cart item is in the database
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.Movie.Id == movie.Id &&
            x.ShopppingCartId == ShopppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
                _context.SaveChanges();
            }
        

        //for getting all the shopping cart item
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.ShopppingCartId ==
            ShopppingCartId).Include(n => n.Movie).ToList());
        }
        //for getting the shopping cart total.
        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(n => n.ShopppingCartId == ShopppingCartId).Select(n =>
            n.Movie.Price * n.Amount).Sum();
         
        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShopppingCartId == ShopppingCartId).ToListAsync();
             _context.ShoppingCartItems.RemoveRange(items);

            await _context.SaveChangesAsync();
        }


    }
}
