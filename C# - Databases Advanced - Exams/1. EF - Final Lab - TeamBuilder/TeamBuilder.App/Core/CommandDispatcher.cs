namespace TeamBuilder.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string[] inputArgs = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string commandName = inputArgs.Length > 0 ? inputArgs[0] : string.Empty;

            inputArgs = inputArgs.Skip(1).ToArray();

            // Get command type
            Type commandType = Type.GetType("TeamBuilder.App.Core.Commands." + commandName + "Command");

            // If command's type is not found – it is not valid command
            if (commandType == null)
            {
                throw new NotSupportedException($"Command {commandName} not supported!");
            }

            // Create instance of command with the type that we already extracted
            object command = Activator.CreateInstance(commandType);

            // Get the method called “Execute” of the command
            MethodInfo executeMethod = command.GetType().GetMethod("Execute");

            // Invoke the method we found passing the instance of the command and
            // array of all expected arguments that the method should take when it is invoked
            string result = executeMethod.Invoke(command, new object[] { inputArgs }) as string;
            //string result = executeMethod.Invoke(command, new object[] { inputArgs }).ToString();

            //string result = string.Empty;
            //
            //switch (commandName)
            //{
            //    case "Exit":
            //        ExitCommand exit = new ExitCommand();
            //        exit.Execute(inputArgs);
            //        break;
            //    case "RegisterUser":
            //        RegisterUserCommand registerUser = new RegisterUserCommand();
            //        result = registerUser.Execute(inputArgs);
            //        break;
            //    case "Login":
            //        LoginCommand login = new LoginCommand();
            //        result = login.Execute(inputArgs);
            //        break;
            //    case "Logout":
            //        LogoutCommand logout = new LogoutCommand();
            //        result = logout.Execute(inputArgs);
            //        break;
            //    case "DeleteUser":
            //        DeleteCommand delete = new DeleteCommand();
            //        result = delete.Execute(inputArgs);
            //        break;
            //    case "CreateEvent":
            //        CreateEventCommand createEvent = new CreateEventCommand();
            //        result = createEvent.Execute(inputArgs);
            //        break;
            //    case "CreateTeam":
            //        CreateTeamCommand createTeam = new CreateTeamCommand();
            //        result = createTeam.Execute(inputArgs);
            //        break;
            //    case "InviteToTeam":
            //        InviteToTeamCommand inviteToTeam = new InviteToTeamCommand();
            //        result = inviteToTeam.Execute(inputArgs);
            //        break;
            //    case "AcceptInvite":
            //        AcceptInviteCommand acceptInvite = new AcceptInviteCommand();
            //        result = acceptInvite.Execute(inputArgs);
            //        break;
            //    case "DeclineInvite":
            //        DeclineInviteCommand declineInvite = new DeclineInviteCommand();
            //        result = declineInvite.Execute(inputArgs);
            //        break;
            //    case "KickMember":
            //        KickMemberCommand kickMember = new KickMemberCommand();
            //        result = kickMember.Execute(inputArgs);
            //        break;
            //    case "Disband":
            //        DisbandTeamCommand disbandTeam = new DisbandTeamCommand();
            //        result = disbandTeam.Execute(inputArgs);
            //        break;
            //    case "AddTeamTo":
            //        AddTeamToCommand addTeamTo = new AddTeamToCommand();
            //        result = addTeamTo.Execute(inputArgs);
            //        break;
            //    case "ShowEvent":
            //        ShowEventCommand showEvent = new ShowEventCommand();
            //        result = showEvent.Execute(inputArgs);
            //        break;
            //    case "ShowTeam":
            //        ShowTeamCommand showTeam = new ShowTeamCommand();
            //        result = showTeam.Execute(inputArgs);
            //        break;
            //    case "ImportUsers":
            //        ImportUsersCommand importUsers = new ImportUsersCommand();
            //        result = importUsers.Execute(inputArgs);
            //        break;
            //    case "ImportTeams":
            //        ImportTeamsCommand importUsers = new ImportTeamsCommand();
            //        result = importTeams.Execute(inputArgs);
            //        break;
            //    default:
            //        throw new NotSupportedException($"Command {commandName} not supported!");
            //}

            return result;
        }
    }
}