using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Model;

namespace Web.DAO
{
    public class login
    {
        private BearShopEntities db = new BearShopEntities();
        public int Login(string userName, string passWord)
        {

            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            
                
                else
                {
                    if (result.UserType == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                }


            }
        public User GetById(string userName)
        {
            return db.Users.FirstOrDefault(x => x.UserName == userName);
        }






    }
}