namespace DMobile.Biz.Interface
{
    public interface IUserBase
    {
        string UserID { get; set; }

        string Password { get; set; }

        string Session { get; set; }
    }
}