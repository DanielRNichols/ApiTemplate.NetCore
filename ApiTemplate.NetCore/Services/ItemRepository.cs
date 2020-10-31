using ApiTemplate.NetCore.Data;
using ApiTemplate.NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _db;

        // db created via dependency injection
        public ItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Item entity)
        {
            await _db.AddAsync(entity);

            return await Save();
        }

        public async Task<bool> Delete(Item entity)
        {
            _db.Remove(entity);

            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _db.Items.AnyAsync(row => row.Id == id);
        }

        public async Task<IList<Item>> GetAll()
        {
            var items = await _db.Items.ToListAsync();

            //Note if you have related items, do something like this
            // var authors = await _db.Authors.Include(a => a.Books).ToListAsync();

            return items;
        }

        public async Task<Item> GetById(int id)
        {
            var item = await _db.Items.FindAsync(id);
            // Note if you have related items, do something like this
            // var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(q => q.Id == id);

            return item;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Update(Item entity)
        {
            _db.Items.Update(entity);

            return await Save();
        }
    }
}
