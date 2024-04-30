namespace Courier_lockers.Repos
{
    public class VoUserInfo
    {

        public List<string> Roles { get; set; }
        public string Introduction { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }

        public VoUserInfo(List<string> roles, string introduction, string avatar, string name)
        {
            Roles = roles;
            Introduction = introduction;
            Avatar = avatar;
            Name = name;
        }
    }
}
