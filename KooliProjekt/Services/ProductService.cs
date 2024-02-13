using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Controllers;
using System.ComponentModel;
using System.Linq;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.ProductRepository;
        }

        public async Task<PagedResult<Product>> List(int page, int pageSize)
        {
            var result = await _productRepository.List(page, pageSize);
            return result;

        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetById(int Id)
        {
            var product = await _productRepository.GetById(Id);
            return product;
        }

        public async Task Save(Product product)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                await _productRepository.Save(product);
                await _unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
            }
                    
        }

        public async Task Delete(int Id)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                await _productRepository.Delete(Id);
                await _unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
            }

        }
        public async Task<IList<LookupItem>> Lookup()
        {
            
            return await _productRepository.Lookup();
        }

        public bool Existance(int Id)
        {
            return _productRepository.Existance(Id);
        }
        
        public async Task Add(Product product)
        {
            await _productRepository.Add(product);
        }
        public async Task Entry(Product product)
        {
            await _productRepository.Entry(product);
        }
    }
}