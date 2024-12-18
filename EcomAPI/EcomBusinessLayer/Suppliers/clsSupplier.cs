using EcomDataAccess.SupplierData;

namespace EcomBusinessLayer.Suppliers
{
    public class clsSupplier : ISupplier
    {
        private readonly ISupplierData _supplierData;

        public clsSupplier(ISupplierData supplierData)
        {
            this._supplierData = supplierData;
        }
        public async Task<SupplierDTO> AddNewSupplier(SupplierDTO supplierDTO)
        {
            int newID = await _supplierData.AddNewSupplier(supplierDTO);
            return newID > 0 ? new SupplierDTO(newID, supplierDTO.SupplierName, supplierDTO.Address, supplierDTO.Email, supplierDTO.Phone) : null!;
        }
        public async Task<bool> UpdateSupplier(SupplierDTO supplierDTO)
        {
            bool flag = await _supplierData.UpdateSupplier(supplierDTO);
            return flag;
        }
        public async Task<SupplierDTO> Find(int SupplierID)
        {
            var Res = await _supplierData.GetSupplierByID(SupplierID);
            return Res;
        }
        public async Task<IList<SupplierDTO>> GetAllSuppliers()
        {
            var Res = await _supplierData.GetAllSuppliers();
            return Res;
        }
        public async Task<bool> DeleteSupplier(int SupplierID)
        {
            bool flag = await _supplierData.DeleteSupplier(SupplierID);
            return flag;
        }
        public async Task<bool>IsSupplierExist(int SupplierID)
        {
            int Found = await _supplierData.IsSupplierExist(SupplierID);
            return Found!=-1;
        }

    }
}
