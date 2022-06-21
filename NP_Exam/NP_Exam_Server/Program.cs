using System;
using System.Threading.Tasks;
using NP_Exam_Library;
using NP_Exam_Server.Model;
using static NP_Exam_Server.Repository.MessagerDbRepository;

namespace NP_Exam_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("СтП Экзамен, Шелест\nСерверная часть\n");

            Console.WriteLine("Запуск сервера на порту 1024...");
            AsyncTCPServer server = new AsyncTCPServer("127.0.0.1", 1024, HandleRequest);
            Task _ = server.StartAsync();

            Console.WriteLine("Сервер запущен (Нажмите Enter для завершения)\n");
            Console.ReadLine();
            server.Stop();
        }

        private static object HandleRequest(object request)
        {
            Console.WriteLine($"Handling {request.GetType()}...");

            if (!(request is ServerRequest))
                return new ServerResponse(null);

            if (request is RegisterUserRequest)
            {
                RegisterUserRequest rur = request as RegisterUserRequest;
                return new ServerResponse(RegisterUser(rur.Sender.Username, rur.Sender.Password));
            }
            else if (request is AuthorizeUserRequest)
            {
                AuthorizeUserRequest aur = request as AuthorizeUserRequest;
                return new ServerResponse(AuthorizeUser(new User(aur.Sender)));
            }
            else if (request is DeauthorizeUserRequest)
            {
                DeauthorizeUserRequest dur = request as DeauthorizeUserRequest;
                return new ServerResponse(DeauthorizeUser(new User(dur.Sender)));
            }
            else if (request is GetUserInfoRequest)
            {
                GetUserInfoRequest guir = request as GetUserInfoRequest;
                return new ServerResponse(GetUser(guir.Username)?.ToDisplayUser());
            }
            else if (request is CreateGroupRequest)
            {
                CreateGroupRequest cgr = request as CreateGroupRequest;
                return new ServerResponse(CreateGroup(new User(cgr.Sender), cgr.GroupName, cgr.UserIds));
            }
            else if (request is RenameGroupRequest)
            {
                RenameGroupRequest rgr = request as RenameGroupRequest;
                return new ServerResponse(RenameGroup(new User(rgr.Sender), GetGroup(rgr.GroupId), rgr.NewName));
            }
            else if (request is EnterGroupRequest)
            {
                EnterGroupRequest egr = request as EnterGroupRequest;
                return new ServerResponse(AddUserToGroup(new User(egr.Sender), GetGroup(egr.GroupId)));
            }
            else if (request is LeaveGroupRequest)
            {
                LeaveGroupRequest lgr = request as LeaveGroupRequest;
                return new ServerResponse(RemoveUserFromGroup(new User(lgr.Sender), GetGroup(lgr.GroupId)));
            }
            else if (request is AddUserToGroupRequest)
            {
                AddUserToGroupRequest autgr = request as AddUserToGroupRequest;
                return new ServerResponse(AddUserToGroup(GetUser(autgr.UserId), GetGroup(autgr.GroupId)));
            }
            else if (request is GetGroupListRequest)
            {
                GetGroupListRequest gglr = request as GetGroupListRequest;
                return new ServerResponse(GetUserGroupList(new User(gglr.Sender)));
            }
            else if (request is SendMessageRequest)
            {
                SendMessageRequest smr = request as SendMessageRequest;
                return new ServerResponse(SendMesssage(new User(smr.Sender), GetGroup(smr.GroupId),
                    smr.MessageText, smr.Attachment));
            }
            else if (request is GetGroupMessagesRequest)
            {
                GetGroupMessagesRequest ggmr = request as GetGroupMessagesRequest;
                return new ServerResponse(GetGroupMessages(new User(ggmr.Sender), GetGroup(ggmr.GroupId)));
            }
            else
                return new ServerResponse(null);
        }
    }
}
