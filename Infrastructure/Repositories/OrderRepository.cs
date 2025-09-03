using Microsoft.EntityFrameworkCore;
using OrbitPass.Core.Entities;
using OrbitPass.Core.Interfaces;
using OrbitPass.Infrastructure.Data;

namespace OrbitPass.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.User)    // Load related User data
            .Include(o => o.Event)   // Load related Event data
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        return await context.Orders
            .Include(o => o.Event)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt) // Latest orders first
            .ToListAsync();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        order.UpdatedAt = DateTime.UtcNow;
        context.Orders.Update(order);
        await context.SaveChangesAsync();
        return order;
    }
}
