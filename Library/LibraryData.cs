using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class LibraryData
    {
        public BookManagement bookManagement { get; set; }
        public UsersManagement usersManagement { get; set; }
    }
}
