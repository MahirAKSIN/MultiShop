namespace MultiShop.Basket.NewFolder
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                return user?.FindFirst("sub")?.Value
                    ?? user?.FindFirst("client_id")?.Value
                    ?? throw new UnauthorizedAccessException("User id (sub/client_id) claim not found.");
            }
        }
    }
}
