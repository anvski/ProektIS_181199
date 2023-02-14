using ClosedXML.Excel;
using Eshop.Domain.Identity;
using Eshop.Domain.ViewModels;
using Eshop.Services.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<EshopAppUser> _userManager;
        private readonly RoleManager<EshopAppRoles> _roleManager;
        private readonly ITicketService _ticketService;

        public AdminController(IUserService userService, UserManager<EshopAppUser> userManager
            , RoleManager<EshopAppRoles> roleManager, ITicketService ticketService)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _ticketService = ticketService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var users = _userService.getAllUsers();
            var usersInDefault = await _userManager.GetUsersInRoleAsync("Default");

            foreach (var item in users)
            {
                if (usersInDefault.Contains(item.user))
                {
                    item.role = "Default";
                }
                else
                {
                    item.role = "Admin";
                }
            }

            return View(users);
        }
        public async Task<IActionResult> MakeAdminAsync(string id)
        {
            var user = _userService.getUser(id);
            IdentityResult removeFromRole = await _userManager.RemoveFromRoleAsync(user, "Default");
            IdentityResult addToRole = await _userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ImportUsers(IFormFile file )
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using(FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<UsersForImport> users = getUsersForImport(file.FileName);

            _userService.addUsers(users);

            return RedirectToAction("Index");
        }

        private List<UsersForImport> getUsersForImport(string fileName)
        {
            List<UsersForImport> users = new List<UsersForImport>(); 
            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using(var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new UsersForImport
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            return users;
        }
        
        public FileContentResult ExportAllTickets()
        {
            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            try { 
                string selectedValue = Request.Form["Genres"].ToString();
                var AllTickets = _ticketService.GetTicketsFromGenre(selectedValue);
                if (AllTickets == null) return null;

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");

                    worksheet.Cell(1, 1).Value = "Ticket ID";
                    worksheet.Cell(1, 2).Value = "Ticket Name";
                    worksheet.Cell(1, 3).Value = "Ticket Genre";
                    worksheet.Cell(1, 4).Value = "Ticket Price";

                    for (int i = 0; i < AllTickets.Count; i++)
                    {
                        var item = AllTickets[i];

                        worksheet.Cell(i + 2, 1).Value = item.ID.ToString();
                        worksheet.Cell(i + 2, 2).Value = item.ProductName.ToString();
                        worksheet.Cell(i + 2, 3).Value = selectedValue.ToString();
                        worksheet.Cell(i + 2, 4).Value = item.Price.ToString();
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(content, contentType, fileName);
                    }
                }
            } catch(InvalidOperationException exception)
            {
                Console.WriteLine(exception.Message);
            }
            return null;
            
            
            
        }
    }
}
