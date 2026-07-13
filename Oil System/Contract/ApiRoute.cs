namespace Oil_System.Contract
{
    public static class ApiRoute
    {

        public static class Account
        {
            public const string GetAllUsers = "api/account/Users";
            public const string GetById = "api/account/UserDetails/{id}";
            public const string Register = "api/account/CreateUser";
            public const string Login = "api/account/Login";
            public const string UpdateStatus = "api/account/UpdateStatus";
            public const string Delete = "api/account/DeleteUser/{id}";
            public const string UpdateUser = "api/account/UpdateUser";
        }

        public static class OilsPacket
        {
            public const string GetAllOilsPackets = "api/oilspacket/oilPackets";
            public const string GetOilsPacketById = "api/oilspacket/oilPacketDetails/{id}";
            public const string CreateOilsPackets = "api/oilspacket/createOilPacket";
            public const string UpdateOilsPackets = "api/oilspacket/updateOilPacket";
            public const string DeleteOilsPackets = "api/oilspacket/deleteOilPacket/{id}";
        }

        public static class Brand
        {
            public const string GetAllBrands = "api/Brands/Brands";
            public const string GetBrandById = "api/Brands/Brand/{id}";
            public const string CreateBrand = "api/Brands/createBrand";
            public const string UpdateBrand = "api/Brands/updateBrand";
            public const string DeleteBrand = "api/Brands/deleteBrand/{id}";
        }

        public static class Category
        {
            public const string GetAllCategories = "api/Categories/Categories";
            public const string GetCategoryById = "api/Categories/Category/{id}";
            public const string CreateCategory = "api/Categories/createCategory";
            public const string UpdateCategory = "api/Categories/updateCategory";
            public const string DeleteCategory = "api/Categories/deleteCategory/{id}";
        }
    }
}
