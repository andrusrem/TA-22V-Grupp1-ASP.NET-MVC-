using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Query;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _orderRepository = unitOfWork.OrderRepository;
        }

        public async Task<PagedResult<Order>> List(int page, int pageSize)
        {
            var result = await _orderRepository.List(page, pageSize);
            return result;

        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<List<Order>> GetCustomerOrders(string email)
        {
            var result = await _orderRepository.GetCustomerOrders(email);
            return result;
        }
        

        public async Task<Order> GetById(int id)
        {
            var order = await _orderRepository.GetById(id);
            return order;
        }

        public async Task Save(Order order)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                await _orderRepository.Save(order);
                await _unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
            }
        }

        public async Task Delete(int? id)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                await _orderRepository.Delete(id);
                await _unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
            }
        }

        public bool Existance(int Id)
        {
            return _orderRepository.Existance(Id);
        }

        public async Task Entry(Order order)
        {
            await _orderRepository.Entry(order);
        }

        public async Task Add(Order order)
        {
            await _orderRepository.Add(order);
        }
        
    }
}