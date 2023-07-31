using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleContext;
using VehicleInterface.Interface;
using VehicleTypeModel;

namespace VehicleInterface
{
    public class BrandRepository : IBrandInterface
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Brands> AddBrand(Brands brand)
        {
            await _context.Brands.AddAsync(brand);
            var brandIsAdded=await _context.SaveChangesAsync();
            return brandIsAdded > 0 ? brand : null;

        }
        public  bool DeleteBrand(int id)
        {
            var brand= _context.Brands.Find(id);
            _context.Remove(brand);
            return _context.SaveChanges() > 0 ? true : false;
        }
        public ICollection<Brands> GetAllBrands()
        {
            return _context.Brands.ToList();
        }

        public ICollection<Brands> GetAllBrandsOfAVehicleType(int id)
        {
            return _context.Brands.Where(brands=>brands.VehicleTypeId==id).ToList();
        }

      

        public bool IsExists(int id)
        {
            return _context.Brands.Any(brand =>brand.BrandId == id);
        }
        public async Task<Brands> UpdateBrands(int id, Brands brand)
        {
            _context.Brands.Entry(brand).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return brand;
        }

        public Task<Brands> UpdateBrands(Brands brand)
        {
            throw new NotImplementedException();
        }
    }
}
