
public class User
{
    public string Id {get ; set ;}
    public string Username {get; set;}
    public int Pv {get; set;} = 100;

    public User(string id, string username)
    {
        Id= id;
        Username = username;
    }
}