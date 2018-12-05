namespace DispatchRider
{
    public interface IUserInfo
    {
        string Id { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
    }
}