using System;
using System.Linq;
using System.Collections.Generic;
using NP_Exam_Library;
using NP_Exam_Server.Model;

namespace NP_Exam_Server.Repository
{
    public static class MessagerDbRepository
    {
        private static MessagerDbContext _dbContext = new MessagerDbContext();

        public static Group GetGroup(int id)
        {
            return _dbContext.Groups.FirstOrDefault(x => x.Id == id);
        }

        public static Message GetMessage(int id)
        {
            return _dbContext.Messages.FirstOrDefault(x => x.Id == id);
        }

        public static User GetUser(string username)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public static User GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public static UserGroup GetUserGroup(int userId, int groupId)
        {
            return _dbContext.UsersGroups.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
        }

        public static DisplayUser RegisterUser(string username, string password)
        {
            try
            {
                if (IsUsernameOccupied(username))
                    return null;

                lock (_dbContext)
                {
                    User user = _dbContext.Users.Add(new User(username, password));
                    _dbContext.SaveChanges();
                    Console.WriteLine($"Users in database: {_dbContext.Users.Count()}");
                    return user.ToDisplayUser();
                }
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayUser AuthorizeUser(User user)
        {
            if (user == null)
                return null;

            try
            {
                if (!IsValidUser(user) || IsUserAuthorized(user))
                    return null;

                lock (_dbContext)
                {
                    user = GetUser(user.Username);
                    user.Authorized = true;
                    _dbContext.SaveChanges();
                }

                return user.ToDisplayUser();
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayUser DeauthorizeUser(User user)
        {
            if (user == null)
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user))
                    return null;

                lock (_dbContext)
                {
                    user = GetUser(user.Username);
                    user.Authorized = false;
                    _dbContext.SaveChanges();
                }

                return user.ToDisplayUser();
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayGroup CreateGroup(User creator, string groupName, List<int> userIds)
        {
            if (creator == null || String.IsNullOrEmpty(groupName) || userIds == null)
                return null;

            try
            {
                if (!IsValidUser(creator) || !IsUserAuthorized(creator))
                    return null;

                Group g = new Group(groupName);
                if (IsValidGroup(g))
                    return null;

                lock (_dbContext)
                {
                    g = _dbContext.Groups.Add(g);
                    _dbContext.SaveChanges();
                }

                lock (_dbContext)
                {
                    foreach (int uid in userIds)
                        _dbContext.UsersGroups.Add(new UserGroup(uid, g.Id));
                    _dbContext.UsersGroups.Add(new UserGroup(creator.Id, g.Id));
                    _dbContext.SaveChanges();
                }

                return g.ToDisplayGroup();
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayGroup RenameGroup(User user, Group group, string newName)
        {
            if (user == null || group == null || String.IsNullOrEmpty(newName))
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user)
                    || !IsValidGroup(group) || !IsUserInGroup(user, group))
                    return null;

                Group g = GetGroup(group.Id);

                lock (_dbContext)
                {
                    g.GroupName = newName;
                    _dbContext.SaveChanges();
                }

                return g.ToDisplayGroup();
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayUserGroup AddUserToGroup(User user, Group group)
        {
            if (user == null || group == null)
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user)
                    || !IsValidGroup(group) || IsUserInGroup(user, group))
                    return null;

                lock (_dbContext)
                {
                    UserGroup ug = _dbContext.UsersGroups.Add(new UserGroup(user.Id, group.Id));
                    _dbContext.SaveChanges();
                    return ug.ToDisplayUserGroup();
                }
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayUserGroup RemoveUserFromGroup(User user, Group group)
        {
            if (user == null || group == null)
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user)
                    || !IsValidGroup(group) || !IsUserInGroup(user, group))
                    return null;

                lock (_dbContext)
                {
                    UserGroup ug = _dbContext.UsersGroups.Remove(GetUserGroup(user.Id, group.Id));
                    _dbContext.SaveChanges();
                    return ug.ToDisplayUserGroup();
                }
            }
            catch (Exception)
            { return null; }
        }

        public static List<DisplayGroup> GetUserGroupList(User user)
        {
            if (user == null)
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user))
                    return null;

                lock (_dbContext)
                    return GetUser(user.Username).UserGroups.Select(x => x.Group.ToDisplayGroup()).Distinct().ToList();
            }
            catch (Exception)
            { return null; }
        }

        public static DisplayMessage SendMesssage(User user, Group group, string txt, object attach)
        {
            if (user == null || group == null || String.IsNullOrEmpty(txt))
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user)
                    || !IsValidGroup(group) || !IsUserInGroup(user, group))
                    return null;

                Message msg = new Message(user.Id, group.Id, txt, attach);
                lock (_dbContext)
                {
                    msg = _dbContext.Messages.Add(msg);
                    _dbContext.SaveChanges();
                    return msg.ToDisplayMessage();
                }
            }
            catch (Exception)
            { return null; }
        }

        public static List<DisplayMessage> GetGroupMessages(User user, Group group)
        {
            if (user == null || group == null)
                return null;

            try
            {
                if (!IsValidUser(user) || !IsUserAuthorized(user)
                    || !IsValidGroup(group) || !IsUserInGroup(user, group))
                    return null;

                lock (_dbContext)
                    return GetGroup(group.Id).Messages.Select(x => x.ToDisplayMessage()).ToList();
            }
            catch (Exception)
            { return null; }
        }

        public static bool IsUserAuthorized(User user)
        {
            return _dbContext.Users.Where(
                x => x.Username == user.Username
                && x.Authorized
            ).FirstOrDefault() != null;
        }

        public static bool IsValidUser(User user)
        {
            return _dbContext.Users.Where(
                x => x.Username == user.Username
                && x.Password == user.Password
            ).FirstOrDefault() != null;
        }

        public static bool IsValidGroup(Group group)
        {
            return _dbContext.Groups.Where(
                x => x.Id == group.Id
                && x.GroupName == group.GroupName
            ).FirstOrDefault() != null;
        }

        public static bool IsUserInGroup(User user, Group group)
        {
            return _dbContext.UsersGroups.Where(
                x => x.UserId == user.Id
                && x.GroupId == group.Id
            ).FirstOrDefault() != null;
        }

        public static bool IsUsernameOccupied(string username)
        {
            return _dbContext.Users.Where(
                x => x.Username == username
            ).FirstOrDefault() != null;
        }
    }
}
