namespace EcomDataAccess.SupplierData
{
    public class SupplierDTO
    {
        public int SupplierID { set; get; }
        public string SupplierName { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public SupplierDTO(int SupplierID,string SupplierName,string Address,string Email,string Phone)
        {
            this.SupplierID = SupplierID;
            this.SupplierName = SupplierName;
            this.Address = Address;
            this.Email = Email;
            this.Phone = Phone;
        }
    }
}
