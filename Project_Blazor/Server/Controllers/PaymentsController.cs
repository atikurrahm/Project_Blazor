using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Blazor.shared.Models;
using Project_Blazor.Shared.DTO;

namespace Project_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly WorkerDbContext _context;

        public PaymentsController(WorkerDbContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
          if (_context.Payments == null)
          {
              return NotFound();
          }
            return await _context.Payments.ToListAsync();
        }
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<PaymentViewDTO>>> GetPaymentDTO()
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }
            return await _context.Payments
                .Include(o=>o.Customer)
                .Include(o=>o.Works).ThenInclude(oi=> oi.Worker)
                .Select(o=>
                new PaymentViewDTO
                {
                    PaymentId= o.PaymentId,
                    StartDate= o.StartDate,
                    EndDate= o.EndDate,
                    PaymentDone= o.PaymentDone,
                    CustomerName = o.Customer.CustomerName,
                    TotalWorker =o.Works.Count,
                    TotalPayment=o.Works.Sum(x=>x.Worker.Payrate * x.TotalWorkHour)
                })
                .ToListAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
          if (_context.Payments == null)
          {
              return NotFound();
          }
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }
        [HttpGet("DTO/{id}")]
        public async Task<ActionResult<PaymentViewDTO>> GetPaymentViewDTO(int id)
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments.Include(o=>o.Customer)
                .Include(o=>o.Works).ThenInclude(oi=>oi.Worker).FirstOrDefaultAsync(o=>o.PaymentId==id);

            if (payment == null)
            {
                return NotFound();
            }

            return new PaymentViewDTO
            {
                PaymentId = payment.PaymentId,
                StartDate = payment.StartDate,
                EndDate = payment.EndDate,
                CustomerName=payment.Customer.CustomerName,
                PaymentDone = payment.PaymentDone,
                TotalWorker = payment.Works.Count,
                TotalPayment = payment.Works.Sum(x => x.Worker.Payrate * x.TotalWorkHour)
            };
        }
        [HttpGet("Works/{id}")]
        public async Task<ActionResult<IEnumerable<WorkViewDTO>>> GetWorks(int id)
        {
            if (_context.Works == null)
            {
                return NotFound();
            }
            var works = await _context.Works.Include(x => x.Worker).Where(oi => oi.PaymentId == id).ToListAsync();

            if (works == null)
            {
                return NotFound();
            }

            return works.Select(oi => new WorkViewDTO { PaymentId = oi.PaymentId, WorkerName = oi.Worker.WorkerName, Payrate = oi.Worker.Payrate, TotalWorkHour = oi.TotalWorkHour }).ToList();
        }
        [HttpGet("OI/{id}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorkOf(int id)
        {
            if (_context.Works == null)
            {
                return NotFound();
            }
            var works = await _context.Works.Where(oi => oi.PaymentId == id).ToListAsync();

            if (works == null)
            {
                return NotFound();
            }

            return works;
        }
        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("DTO/{id}")]
        public async Task<IActionResult> PutPaymentWithWork(int id, PaymentEditDTO payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            var existing = await _context.Payments.Include(x => x.Works).FirstAsync(o => o.PaymentId == id);
            _context.Works.RemoveRange(existing.Works);
            existing.PaymentId = payment.PaymentId;
            existing.StartDate = payment.StartDate;
            existing.EndDate = payment.EndDate;
            existing.CustomerID = payment.CustomerID;
            existing.PaymentDone = payment.PaymentDone;
            foreach (var item in payment.Works)
            {
                _context.Works.Add(new Work { PaymentId = payment.PaymentId, WorkerId = item.WorkerId, TotalWorkHour = item.TotalWorkHour });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }

            return NoContent();
        }
        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
          if (_context.Payments == null)
          {
              return Problem("Entity set 'WorkerDbContext.Payments'  is null.");
          }
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.PaymentId }, payment);
        }
        [HttpPost("DTO")]
        public async Task<ActionResult<Payment>> PostPaymentDTO(PaymentDTO dto)
        {
            if (_context.Payments == null)
            {
                return Problem("Entity set 'WorkerDbContext.Payments'  is null.");
            }
            var payment = new Payment { CustomerID = dto.CustomerID, StartDate = dto.StartDate, EndDate = dto.EndDate, PaymentDone = dto.PaymentDone };
            foreach (var oi in dto.Works)
            {
                payment.Works.Add(new Work { WorkerId = oi.WorkerId, TotalWorkHour = oi.TotalWorkHour });
            }
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }
        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return (_context.Payments?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        }
    }
}
