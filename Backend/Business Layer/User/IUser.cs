using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    // Use this interface to test User component using Moq.
    public interface IUser
    {
        String email { get;}
        String password { get;}

        bool status { get;}



    }
}
