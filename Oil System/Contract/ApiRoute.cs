namespace Oil_System.Contract
{
    public static class ApiRoute
    {
        public static class OilsPacket
        {
            public const string GetAll = "api/oilspacket/getall";
            public const string GetById = "api/oilspacket/getbyid/{id}";
            public const string Create = "api/oilspacket/create";
            public const string Update = "api/oilspacket/update/{id}";
            public const string Delete = "api/oilspacket/delete/{id}";
        }

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
        public static class Brand
        {
            public const string GetAllBrands = "api/Brands/Brands";
            public const string GetBrandById = "api/Brands/Brand/{id}";
            public const string CreateBrand = "api/Brands/createBrand";
            public const string UpdateBrand = "api/Brands/updateBrand";
            public const string DeleteBrand = "api/Brands/deleteBrand/{id}";
        }

    }
}
