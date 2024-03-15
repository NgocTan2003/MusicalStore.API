namespace MusicalStore.Dtos.Users
{
    public class ClaimUserLogin
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
