namespace Oil_System.Contract.Request.Authentcation
{
    public class ChangeStatusRequest
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
