namespace DMobile.Biz.Interface
{
    public interface ILoginEntry
    {
        IUserBase LoginForSession(string uid, string pwd);
        string CreateUser(string uid, string pwd);
        bool UpdateUser(IUserBase userEntity);
        bool DeleteUser(string uid);
        bool IsUserExists(string uid);
        bool CheckSession(string uid, string session);
    }
}