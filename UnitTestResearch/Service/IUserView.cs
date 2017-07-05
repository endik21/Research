namespace Service
{
    public interface IUserView
    {
        bool Saved { get; set; }

        bool Changed { get; set; }
    }
}