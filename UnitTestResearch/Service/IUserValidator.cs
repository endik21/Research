namespace Service
{
    using Domain;

    public interface IUserValidator
    {
        bool Validate(User user);
    }
}