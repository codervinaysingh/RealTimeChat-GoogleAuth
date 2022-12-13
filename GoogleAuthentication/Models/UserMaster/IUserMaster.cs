namespace GoogleAuthentication.Models.UserMaster
{
    public interface IUserMaster
    {

        public String AddUpdateConnection(UserMaster user);
        public string DisConnect(UserMaster user);
        public string GetAllUser (string UserName);
        public string GellAllChat(string userName, string selectUser);
    }
}
