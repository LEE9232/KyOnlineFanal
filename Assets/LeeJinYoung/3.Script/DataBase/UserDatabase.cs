public class UserDatabase
{
    public string userId { get; private set; }
    public string userName { get; private set;}
    public UserDatabase(string userId, string userName)
    {
        this.userId = userId;
        this.userName = userName;        
    }
    public UserDatabase()
    {
        userId = "defaultId";
        userName = "defaultName";
    }
    public UserDatabase(string userId)
    {
        this.userId = userId;
        this.userName = "";
    }
}
