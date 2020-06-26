using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.Services
{
    public class PermissionTree : IPermissionTree
    {
        /// <inheritdoc />
        public Permission Data { get; }

        /// <inheritdoc />
        public IPermissionTree Parent { get; private set; }

        /// <inheritdoc />
        public ICollection<IPermissionTree> Children { get; }

        /// <inheritdoc />
        public PermissionTree(Permission data)
        {
            Children = new LinkedList<IPermissionTree>();
            Data = data;
        }

        /// <inheritdoc />
        public static PermissionTree FromLookup(ILookup<Permission, Permission> lookup)
        {
            var rootData = lookup.Count == 1 ? lookup.First().Key : default;
            var root = new PermissionTree(rootData);
            root.LoadChildren(lookup);
            return root;
        }

        private void LoadChildren(ILookup<Permission, Permission> lookup)
        {
            foreach (var data in lookup[Data])
            {
                var child = new PermissionTree(data) { Parent = this };
                Children.Add(child);
                child.LoadChildren(lookup);
            }
        }
    }
}