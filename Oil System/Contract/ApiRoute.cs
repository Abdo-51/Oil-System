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
            public const string Register = "api/account/createUser";
            public const string Login = "api/account/login";
            public const string Update = "api/acount/updateStatus";
        }
    }
}
