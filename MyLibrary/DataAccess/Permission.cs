using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.DataAccess
{
    public partial class Permission
    {
        public Permission()
        {
            RoleHasPermissions = new HashSet<RoleHasPermission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }

        public virtual ICollection<RoleHasPermission> RoleHasPermissions { get; set; }
    }
}
