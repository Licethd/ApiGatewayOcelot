using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tarea1.Dto
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
