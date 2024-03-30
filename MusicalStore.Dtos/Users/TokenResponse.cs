namespace MusicalStore.Dtos.Users
{
    public class TokenResponse
    {
        public string? Id { get; set; }
        public string? AccessToken { get; set; }
        public string? RefeshToken { get; set; }
        public string? UserName { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public DateTime TokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

    }
}
