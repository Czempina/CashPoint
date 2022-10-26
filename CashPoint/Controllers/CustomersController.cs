using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CashPoint.EntityFramework;
using CashPoint.Models;
using CashPoint.Enums;

namespace CashPoint.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CashPointContext _context;

        public CustomersController(CashPointContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> CheckUser(string name, string pass)
        {
            var customer = _context.Customers.Where(x => x.Name == name && x.Password == pass);
            return View("Index",await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Password,AccountBalance")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,AccountBalance")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Withdraw(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Withdraw")]
        [Route("{amount}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> withdraw(int id, int amount)
        {
            var customer = await _context.Customers.FindAsync(id);
            var banknotes = new List<BanknoteEnum>();
            customer.AccountBalance -= amount;
            if (customer.AccountBalance < 0)
            {
                return View("Error", "You can not withdraw more money than You have on your account");
            }
            if (amount%10 != 0)
            {
                return View("Error", "You have to withdraw multiple of 10");
            }
            else
            {
                await _context.SaveChangesAsync();
                while (amount - 200 >= 0) { banknotes.Add(BanknoteEnum.TwoHundred); amount -= 200; }
                while (amount - 100 >= 0) { banknotes.Add(BanknoteEnum.OneHundred); amount -= 100; }
                while (amount - 50 >= 0) { banknotes.Add(BanknoteEnum.Fifty); amount -= 50; }
                while (amount - 20 >= 0) { banknotes.Add(BanknoteEnum.Twenty); amount -= 20; }
                while (amount - 10 >= 0) { banknotes.Add(BanknoteEnum.Ten); amount -= 10; }
                return View("Cash", banknotes);
            }
        }
    }
}
