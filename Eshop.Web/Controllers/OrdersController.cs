using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Domain.DomainModels;
using Eshop.Repository;
using Eshop.Services.Interface;
using System.Security.Claims;
using GemBox.Document;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Eshop.Web.Controllers
{
    [Authorize(Roles ="Admin, Default")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderService = orderService;
        }

        // GET: Orders
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_orderService.GetOrdersForUser(userId));
        }

        public FileContentResult GenerateInvoice(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template.docx");

            var template = _orderService.createInvoice(templatePath, userId, id);

            var stream = new MemoryStream();

            template.Save(stream, new PdfSaveOptions());

            var templateName = userId + ".pdf";

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, templateName.ToString());
        }

    }
}
