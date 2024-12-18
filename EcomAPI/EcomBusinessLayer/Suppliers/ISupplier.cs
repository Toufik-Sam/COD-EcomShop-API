using EcomDataAccess.SupplierData;

namespace EcomBusinessLayer.Suppliers
{
    public interface ISupplier
    {
        Task<SupplierDTO> AddNewSupplier(SupplierDTO supplierDTO);
        Task<bool> DeleteSupplier(int SupplierID);
        Task<SupplierDTO> Find(int SupplierID);
        Task<IList<SupplierDTO>> GetAllSuppliers();
        Task<bool> UpdateSupplier(SupplierDTO supplierDTO);
        Task<bool> IsSupplierExist(int SupplierID);
    }
}