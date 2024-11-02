using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.ProductsData.Galleries
{
    public class clsGalleryData : IGalleryData
    {
        private readonly IDataAccessSettings _settings;

        public clsGalleryData(IDataAccessSettings settings)
        {
            this._settings = settings;
        }
        public async Task<int> AddNewGallery(GalleryDTO gelleryDTO)
        {
            int NewGalleryID = -1;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spAddNewGallery", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", gelleryDTO.ProductID);
                    command.Parameters.AddWithValue("@ImagePath", gelleryDTO.ImagePath);
                    var OutputIdParam = new SqlParameter("@NewGalleryID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutputIdParam);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                    NewGalleryID = (int)OutputIdParam.Value;
                }
            }
            return NewGalleryID;
        }
        public async Task<bool> UpdateGallery(GalleryDTO galleryDTO)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spUpdateGallery", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GalleryID", galleryDTO.GalleryID);
                    command.Parameters.AddWithValue("@ProductID", galleryDTO.ProductID);
                    command.Parameters.AddWithValue("@ImagePath", galleryDTO.ImagePath);
                    connection.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected > 0;
        }
        public async Task<GalleryDTO> GetProductGalleryByID(int GalleryID)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductGalleryByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GalleryID", GalleryID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new GalleryDTO(
                                 reader.GetInt32(reader.GetOrdinal("GalleryID")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetString(reader.GetOrdinal("ImagePath"))
                                 );
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IList<GalleryDTO>> GetProductGalleries(int ProductID)
        {
            var GalleriesList = new List<GalleryDTO>();
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.spGetProductGalleries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            GalleriesList.Add(new GalleryDTO(
                                 reader.GetInt32(reader.GetOrdinal("GalleryDTO")),
                                 reader.GetInt32(reader.GetOrdinal("ProductID")),
                                 reader.GetString(reader.GetOrdinal("ImagePath"))));
                        }
                        return GalleriesList;
                    }
                }
            }
            return null;
        }
        public async Task<bool>DeleteProductGallery(int GalleryID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.spDeleteProductGallery", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GalleryID", GalleryID);
                    conn.Open();
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            return rowsAffected == 1;
        }
    }
}
