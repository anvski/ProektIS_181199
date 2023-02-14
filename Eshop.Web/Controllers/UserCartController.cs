using Eshop.Repository;
using Eshop.Domain.DomainModels;
using Eshop.Domain.Identity;
using Eshop.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eshop.Services.Interface;

namespace Eshop.Web.Controllers
{
    [Authorize(Roles ="Admin, Default")]
    public class UserCartController : Controller
    {
        private readonly IUserCartService _userCartService;

        public UserCartController(IUserCartService userCartService)
        {
            _userCartService = userCartService;
        }
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loader = _userCartService.getUserCartInfo(userId);

            return View(loader);

            //argument to service: userId
            //service returns: UserCartLoader
        }
        //GET: UserCart/PlaceOrder
        public IActionResult PlaceOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = _userCartService.placeOrder(userId);

            if (result == true)
            {
                return RedirectToAction("Index", "UserCart");
            }
            else
            {
                return NotFound();
            }
            
            

            //argument to service : userId
            //service returns: bool
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ticketsInShoppingCart = _userCartService.getTicketFromUserCart(id, userId);

            if (ticketsInShoppingCart == null)
            {
                return NotFound();
            }

            return View(ticketsInShoppingCart);

            //get ticket from shopping cart that matches the id
        
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = _userCartService.removeTicketFromCart(id, userId);
            if(result == true)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }

            
            //delete ticket that matches id
            //argument: guid
            //void
        }

    }

    
}
