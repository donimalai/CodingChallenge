using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.IO;
namespace CodingChallenge
{
    class Program
    {

        static void Main(string[] args)
        {
            string user_roles_json = @"{ 'roles' : 
                                [
                                 { 'Id': 1,  'Name': 'System Administrator','Parent': 0 },
                                 { 'Id': 2,  'Name': 'Location Manager','Parent': 1 },
                                 { 'Id': 3,  'Name': 'Supervisor','Parent': 2 },
                                 { 'Id': 4,  'Name': 'Employee','Parent': 3 },
                                 { 'Id': 5,  'Name': 'Trainer','Parent': 3}
                             ]
                            }";

            string users_json = @"{
                'users' : [
                                {'Id': 1,'Name': 'Adam Admin','Role': 1},
                                {'Id': 2,'Name': 'Emily Employee','Role': 4},
                                {'Id': 3,'Name': 'Sam Supervisor','Role': 3},
                                {'Id': 4,'Name': 'Mary Manager','Role': 2},
                                {'Id': 5,'Name': 'Steve Trainer','Role': 5}
                        ]

                        }";

            
            UserManager u = new UserManager(user_roles_json, users_json);
           string subordinates =  u.GetSubordinates(2);
           Console.WriteLine(subordinates);
            
        }


    }

}
