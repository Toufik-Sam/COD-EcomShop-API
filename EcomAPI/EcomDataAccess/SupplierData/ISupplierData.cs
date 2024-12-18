
namespace EcomDataAccess.SupplierData
{
    public interface ISupplierData
    {
        Task<int> AddNewSupplier(SupplierDTO supplierDTO);
        Task<bool> DeleteSupplier(int SupplierID);
        Task<IList<SupplierDTO>> GetAllSuppliers();
        Task<SupplierDTO> GetSupplierByID(int SupplierID);
        Task<bool> UpdateSupplier(SupplierDTO supplierDTO);
        Task<int> IsSupplierExist(int SupplierID);
    }
}