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

    public class rolesList
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoleRouteInfo
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class RouteInfo
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public bool Hidden { get; set; }
        public bool AlwaysShow { get; set; }
        public List<RouteInfo> Children { get; set; }
        public Meta Meta { get; set; }
    }

    public class Meta
    {
        public string Title { get; set; }
    }

}
