using EcomDataAccess;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcomDataAccess.CustomersData;
using EcomBusinessLayer.Customers;
using EcomDataAccess.EmployeesData;
using EcomBusinessLayer.Employees;
using EcomDataAccess.ProductsData;
using EcomBusinessLayer.Products;
using EcomBusinessLayer.Products.Categories;
using EcomDataAccess.ProductsData.CategoriesData;
using EcomDataAccess.ProductsData.ProductsCategories;
using EcomDataAccess.ProductsData.Galleries;
using EcomDataAccess.CustomersData.CustomersAddresses;
using EcomDataAccess.OrdersData;
using EcomDataAccess.OrdersData.OrderStatus;
using EcomBusinessLayer.Orders;
using EcomDataAccess.OrdersData.OrderItems;
using EcomBusinessLayer.Cards;
using EcomDataAccess.CartsData;
using EcomDataAccess.CartsData.CartsItemsData;
using EcomDataAccess.SalesData;
using EcomBusinessLayer.Sales;
using EcomDataAccess.SalesData.SalesManagement;
using EcomBusinessLayer.Sales.SalesDetails;
using EcomDataAccess.SalesData.SalesManagement.CustomerBehavior;
using EcomBusinessLayer.Sales.SalesDetails.CustomerBehavior;
using EcomDataAccess.SalesData.SalesManagement.ProductInsight;
using EcomBusinessLayer.Sales.SalesDetails.ProductInsight;
using EcomDataAccess.SupplierData;
using EcomBusinessLayer.Suppliers;
using EcomDataAccess.SupplierData.ProductSupplierData;
using EcomBusinessLayer.Suppliers.ProductSuppliers;

namespace EcommerceAppAPI.StartupConfig
{
    public static class DependencyInjectionExtentions
    {
        public static void AddStandardServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.AddSwagerServices();
        }
        public static void AddSwagerServices(this WebApplicationBuilder builder)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "JWT Authorization header info using bearer tokens",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            var securityRequiremet = new OpenApiSecurityRequirement
            {
               {        
                  new OpenApiSecurityScheme
                  {
                      Reference=new OpenApiReference
                      {
                         Type=ReferenceType.SecurityScheme,
                         Id="bearerAuth"
                      }
                  },
                  new string[]{}
               }
            };
            builder.Services.AddSwaggerGen(opts =>
            {
                opts.AddSecurityDefinition(name: "bearerAuth", securityScheme);
                opts.AddSecurityRequirement(securityRequiremet);
            });
        }
        public static void AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IDataAccessSettings, DataAccessSettings>();
            builder.Services.AddScoped<ICustomerData, clsCustomerData>();
            builder.Services.AddScoped<ICustomer,clsCustomer>();
            builder.Services.AddScoped<IEmployeeData, clsEmployeeData>();
            builder.Services.AddScoped<IEmployee, clsEmployee>();
            builder.Services.AddScoped<IProductData, clsProductData>();
            builder.Services.AddScoped<IProduct, clsProduct>();
            builder.Services.AddScoped<ICategoryData, clsCategoryData>();
            builder.Services.AddScoped<ICategory, clsCategory>();
            builder.Services.AddScoped<IProductCategoryData, clsProductCategoryData>();
            builder.Services.AddScoped<IGalleryData, clsGalleryData>();
            builder.Services.AddScoped<ICustomerAddressData, clsCustomerAddressData>();
            builder.Services.AddScoped<IOrder, clsOrder>();
            builder.Services.AddScoped<IOrdersData, clsOrdersData>();
            builder.Services.AddScoped<IOrderStatusData, clsOrderStatusData>();
            builder.Services.AddScoped<IOrderItemsData, clsOrderItemsData>();
            builder.Services.AddScoped<ICartData, clsCartData>();
            builder.Services.AddScoped<ICartItemData, clsCartItemData>();
            builder.Services.AddScoped<ICart, clsCart>();
            builder.Services.AddScoped<ISaleData, clsSaleData>();
            builder.Services.AddScoped<ISale, clsSale>();
            builder.Services.AddScoped<ISalesManagementData,clsSalesManagementData>();
            builder.Services.AddScoped<ISaleManagement, clsSaleManagement>();
            builder.Services.AddScoped<ICustomerBehaviorData, clsCustomerBehaviorData>();
            builder.Services.AddScoped<ICustomerBehavior, clsCustomerBehavior>();
            builder.Services.AddScoped<IProductInsightData, clsProductInsightData>();
            builder.Services.AddScoped<IProductInsight, clsProductInsight>();
            builder.Services.AddScoped<ISupplierData, clsSupplierData>();
            builder.Services.AddScoped<ISupplier, clsSupplier>();
            builder.Services.AddScoped<IProductSupplierData, clsProductSupplierData>();
            builder.Services.AddScoped<IProductSupplier, clsProductSupplier>();
        }
        public static void AddAuthServices(this WebApplicationBuilder builder)
        { 
            builder.Services.AddAuthorization(opts =>
            opts.FallbackPolicy =new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
            );

            builder.Services.AddAuthentication(defaultScheme: "Bearer").AddJwtBearer(opts => opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>(key: "Authentication:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>(key: "Authentication:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                GetBytes(builder.Configuration.GetValue<string>
                (key: "Authentication:SecretKey")!))
            });
        }
        public static void AddHealthCheckServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.
                GetConnectionString(name: "Default")!);
        }
    }
}
