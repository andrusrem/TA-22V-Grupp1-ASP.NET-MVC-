using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepository = _unitOfWork.InvoiceRepository;
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
            var result = await _invoiceRepository.List(page, pageSize);
            return result;
        }

        public async Task<List<Invoice>> GetCustomerInvoices(string email)
        {
            return await _invoiceRepository.GetCustomerInvoices(email);
        }

        public async Task<Invoice> GetById(int id)
        {
            var invoice = await _invoiceRepository.GetById(id);
            return invoice;
        }

        public async Task Save(Invoice invoice)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                await _invoiceRepository.Save(invoice);
                await _unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
            }
        }

        public async Task<Invoice> FindId(int id)
        {
            return await _invoiceRepository.FindId(id);
        }

        public async Task Delete(int? id)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                await _invoiceRepository.Delete(id);
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
            }
        }

        public bool Existance(int id)
        {
            return _invoiceRepository.Existance(id);
        }

        internal Task<string> FindId(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
