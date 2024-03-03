namespace DalApi;
using DO;

public interface IUser : ICrud<User>
{
    public User? ReadByPassword(string password);
}
