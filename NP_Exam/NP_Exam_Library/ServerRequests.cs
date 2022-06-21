using System;
using System.Collections.Generic;

namespace NP_Exam_Library
{
    [Serializable]
    // Абстрактный класс клиентского запроса на сервер
    public abstract class ServerRequest
    {
        public RequestSender Sender { get; set; }

        public ServerRequest(RequestSender sender)
        {
            Sender = sender;
        }
    }

    [Serializable]
    // Запрос на регистрацию пользователя с указанным логином и паролем на сервере
    public class RegisterUserRequest : ServerRequest
    {
        public RegisterUserRequest(string username, string password)
            : base(new RequestSender(-1, username, password))
        { }
    }

    [Serializable]
    // Запрос на авторизацию пользователя с указанным логином и паролем на сервере
    public class AuthorizeUserRequest : ServerRequest
    {
        public AuthorizeUserRequest(RequestSender sender) : base(sender)
        { }
    }

    [Serializable]
    // Запрос на деавторизацию пользователя с указанным логином и паролем на сервере
    public class DeauthorizeUserRequest : ServerRequest
    {
        public DeauthorizeUserRequest(RequestSender sender) : base(sender)
        { }
    }

    [Serializable]
    // Запрос на получение информации о пользователе по его имени
    public class GetUserInfoRequest : ServerRequest
    {
        public string Username { get; set; }

        public GetUserInfoRequest(RequestSender sender, string username) : base(sender)
        {
            Username = username;
        }
    }

    [Serializable]
    // Запрос на создание группы с указанным названием и учасниками
    public class CreateGroupRequest : ServerRequest
    {
        public string GroupName { get; set; }
        public List<int> UserIds { get; set; }

        public CreateGroupRequest(RequestSender sender, string groupName, List<int> userIds) : base(sender)
        {
            GroupName = groupName;
            UserIds = userIds;
        }
    }

    [Serializable]
    // Запрос на изменение названия группы
    public class RenameGroupRequest : ServerRequest
    {
        public int GroupId { get; set; }
        public string NewName { get; set; }

        public RenameGroupRequest(RequestSender sender, int groupId, string newName) : base(sender)
        {
            GroupId = groupId;
            NewName = newName;
        }
    }

    [Serializable]
    // Запрос на вступление пользователя в группу
    public class EnterGroupRequest : ServerRequest
    {
        public int GroupId { get; set; }

        public EnterGroupRequest(RequestSender sender, int groupId) : base(sender)
        {
            GroupId = groupId;
        }
    }

    [Serializable]
    // Запрос на выход пользователя из группы
    public class LeaveGroupRequest : ServerRequest
    {
        public int GroupId { get; set; }

        public LeaveGroupRequest(RequestSender sender, int groupId) : base(sender)
        {
            GroupId = groupId;
        }
    }

    [Serializable]
    // Запрос на добавление указаного пользователя в группу
    public class AddUserToGroupRequest : ServerRequest
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public AddUserToGroupRequest(RequestSender sender, int userId, int groupId) : base(sender)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    [Serializable]
    // Запрос на получение списка групп пользователя
    public class GetGroupListRequest : ServerRequest
    {
        public GetGroupListRequest(RequestSender sender) : base(sender)
        { }
    }

    [Serializable]
    // Запрос на отправку сообщения в указанную группу
    public class SendMessageRequest : ServerRequest
    {
        public int GroupId { get; set; }
        public string MessageText { get; set; }
        public object Attachment { get; set; }

        public SendMessageRequest(RequestSender sender, int groupId, string txt, object attach) : base(sender)
        {
            GroupId = groupId;
            MessageText = txt;
            Attachment = attach;
        }
    }

    [Serializable]
    // Запрос на получение списка сообщений для указанной группы
    public class GetGroupMessagesRequest : ServerRequest
    {
        public int GroupId { get; set; }

        public GetGroupMessagesRequest(RequestSender sender, int groupId) : base(sender)
        {
            GroupId = groupId;
        }
    }
}
