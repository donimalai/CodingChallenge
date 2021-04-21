using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CodingChallenge
{
    public class UserManager
    {
        private JObject users;
        private JObject user_roles;

        public UserManager(string user_roles_json, string users_json)
        {
            try
            {
                user_roles = JObject.Parse(user_roles_json);
                users = JObject.Parse(users_json);
            }
            catch(Exception ex) {
                
                throw ex;

            }

        }

        public JObject Users { get => users; set => users = value; }
        public JObject User_roles { get => user_roles; set => user_roles = value; }
        /// <summary>
        /// Get the subordinates of the user based on the role hierachy.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>
        ///             Json string of all the subordinates 
        ///             else if there are no subordinates return empty string 
        ///             If the role assigned to the user is invalid or does not have childroles an empty string will be returned.
        /// </returns>
        public string GetSubordinates(int userid)
        {
            try
            {
                StringBuilder subordinates = new StringBuilder();

                string token_path = "$.users[?(@.Id==" + userid + ")]";

                var user = users.SelectToken(token_path);

                if (user is not null)
                {
                    ArrayList childRoleids = GetChildRoles(user["Role"].Value<int>(), new ArrayList());
                    foreach (int i in childRoleids)
                    {
                        token_path = "$.users[?(@.Role==" + i + ")]";
                        var subordinate = users.SelectToken(token_path);
                        subordinates.Append(subordinate.ToString(Newtonsoft.Json.Formatting.None,null));
                    }
                }
                
                return subordinates.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Get the roles under the specified role
        /// Recursively traverse through the hierarchy.
        /// </summary>
        /// <param name="roleid">Parent role id</param>
        /// <param name="children"> Arraylist of all the child roles </param>
        /// <returns>Arraylist of child roleids. If the role id doesnot exists 
        ///             then return empty array list.
        ///             If there are no child role ids then as well return empty array list
        /// </returns>
        private ArrayList GetChildRoles(int roleid, ArrayList children)
        {
            try
            {
                string token_path = "$.roles[?(@.Parent==" + roleid + ")]";
                var childroles = user_roles.SelectTokens(token_path).ToArray();
                if (childroles is not null)
                {
                    foreach (var c in childroles)
                    {
                        //If role is parent to itself we have to avoid going into infinite loop.
                        if (c["Id"].Value<int>() != c["Parent"].Value<int>())
                        {
                            GetChildRoles(c["Id"].Value<int>(), children);
                            children.Add(c["Id"].Value<int>());
                        }
                    }
                }
               
                return children;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}
