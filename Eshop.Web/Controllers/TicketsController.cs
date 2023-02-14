using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Repository;
using Eshop.Domain.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Eshop.Domain.Identity;
using System.Security.Claims;
using Eshop.Domain.ViewModels;
using Eshop.Services.Interface;

namespace Eshop.Web.Controllers
{
    [Authorize(Roles ="Admin, Default")]
    public class TicketsController : Controller
    {
        private readonly ITicketService _service;
        public TicketsController(ITicketService service)
        {
            _service = service;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var tickets = _service.GetAllTickets();
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _service.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            List<SelectListItem> genres = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Horror" , Value = "Horror"},
                new SelectListItem { Text = "Comedy" , Value = "Comedy"},
                new SelectListItem {  Text = "Romance" ,Value = "Romance"},
                new SelectListItem {Text = "Action" , Value = "Action"},
                new SelectListItem { Text = "Drama" ,Value = "Drama" },
            };

            //assigning SelectListItem to view Bag
            ViewBag.genres = genres;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,ProductName,ProductImage,ProductDescription,Rating,Price,dateValid,Genre")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.ID = Guid.NewGuid();
                _service.createNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _service.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ID,ProductName,ProductImage,ProductDescription,Rating,Price")] Ticket ticket)
        {
            if (id != ticket.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.updateExistingTIcket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _service.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _service.checkTicketExists(id);
        }

        //GET: Tickets/AddToCart/id
        [Authorize(Roles ="Default, Admin")]
        public ActionResult AddToCart(Guid id)
        {
            if( id == null )
            {
                return NotFound();
            }

            var ticketToCart = _service.GetShoppingCartInfo(id);

            if( ticketToCart == null)
            {
                return NotFound();
            }

            return View(ticketToCart);
        }

        //POST: Tickets/AddToCart/id
        [HttpPost]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult AddToCart(Guid id, [Bind("Quantity")] TicketToCart ticketToCart)
        {

            var ticket = _service.GetTicket(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ticketToCart.ticket = ticket;

            var result =_service.AddToUserCart(ticketToCart, userId);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else return RedirectToAction("AddToCart", "TicketsController", id);
            
        }
    }
}
