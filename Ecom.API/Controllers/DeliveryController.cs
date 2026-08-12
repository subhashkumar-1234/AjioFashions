using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ecom.Infrastructure.Data;
using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "DeliveryAgent")]
        public async Task<IActionResult> GetAssignedDeliveries()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                // Get orders assigned to the logged-in agent that are not finalized
                var orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.DeliveryAgentId == userId)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                var result = orders.Select(o => new
                {
                    o.Id,
                    CustomerName = o.User != null ? o.User.Name : "Customer",
                    CustomerEmail = o.User != null ? o.User.Email : "customer@ecom.com",
                    o.OrderDate,
                    o.ShippingAddress,
                    o.PhoneNumber,
                    o.PostalCode,
                    o.TotalAmount,
                    o.Status,
                    ItemsCount = o.OrderItems.Sum(oi => oi.Quantity),
                    Items = o.OrderItems.Select(oi => new {
                        oi.Id,
                        ProductName = oi.Product != null ? oi.Product.ProductName : "Unknown Product",
                        oi.Quantity,
                        oi.Size
                    }).ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("orders/{id}/delivery-status")]
        [Authorize(Roles = "DeliveryAgent")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] DeliveryStatusUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status cannot be empty");

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("Order not found");

                // Enforce that order must be assigned to this carrier
                if (order.DeliveryAgentId != userId)
                    return Forbid("You are not authorized to update this order delivery");

                var upperStatus = dto.Status.ToUpper();
                if (upperStatus != "PENDING" && upperStatus != "PAID" && upperStatus != "SHIPPED" && 
                    upperStatus != "OUT_FOR_DELIVERY" && upperStatus != "DELIVERED" && upperStatus != "CANCELLED")
                {
                    return BadRequest("Invalid status code");
                }

                order.Status = upperStatus;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Order status updated successfully", OrderId = id, Status = upperStatus });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("agents")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetDeliveryAgents()
        {
            try
            {
                // Fetch users who are assigned the DeliveryAgent role
                var deliveryRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "DeliveryAgent");
                if (deliveryRole == null) return Ok(new List<object>());

                var agents = await _context.UserRoles
                    .Include(ur => ur.User)
                    .Where(ur => ur.RoleId == deliveryRole.Id && ur.User != null)
                    .Select(ur => new
                    {
                        Id = ur.User!.Id,
                        Name = ur.User!.Name,
                        Email = ur.User!.Email
                    })
                    .Distinct()
                    .ToListAsync();

                return Ok(agents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("orders/{id}/assign")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> AssignOrderDelivery(int id, [FromBody] DeliveryAssignmentDto dto)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("Order not found");

                if (dto.DeliveryAgentId.HasValue)
                {
                    // Verify the user exists and holds DeliveryAgent role
                    var deliveryRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "DeliveryAgent");
                    var isAgent = await _context.UserRoles.AnyAsync(ur => ur.UserId == dto.DeliveryAgentId.Value && ur.RoleId == deliveryRole.Id);
                    if (!isAgent)
                        return BadRequest("Selected user is not registered as a delivery agent");

                    order.DeliveryAgentId = dto.DeliveryAgentId;
                }
                else
                {
                    order.DeliveryAgentId = null;
                }

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Order delivery carrier assigned successfully", OrderId = id, DeliveryAgentId = order.DeliveryAgentId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class DeliveryStatusUpdateDto
    {
        public required string Status { get; set; }
    }

    public class DeliveryAssignmentDto
    {
        public int? DeliveryAgentId { get; set; }
    }
}
