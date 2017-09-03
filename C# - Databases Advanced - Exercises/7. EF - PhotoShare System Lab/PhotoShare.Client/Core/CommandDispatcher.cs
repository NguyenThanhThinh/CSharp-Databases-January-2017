namespace PhotoShare.Client.Core
{
    using Commands;
    using Services;
    using System.Linq;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            PictureService pictureService = new PictureService();
            AlbumService albumService = new AlbumService();
            UserService userService = new UserService();
            TownService townService = new TownService();
            TagService tagService = new TagService();

            string commandName = commandParameters[0];
            commandParameters = commandParameters.Skip(1).ToArray();
            string result = string.Empty;

            switch (commandName)
            {
                case "RegisterUser":
                    RegisterUserCommand registerUser = new RegisterUserCommand(userService);
                    result = registerUser.Execute(commandParameters);
                    break;

                case "AddTown":
                    AddTownCommand addTown = new AddTownCommand(townService);
                    result = addTown.Execute(commandParameters);
                    break;

                case "ModifyUser":
                    ModifyUserCommand modifyUser = new ModifyUserCommand(userService, townService);
                    result = modifyUser.Execute(commandParameters);
                    break;

                case "DeleteUser":
                    DeleteUserCommand deleteUser = new DeleteUserCommand(userService);
                    result = deleteUser.Execute(commandParameters);
                    break;

                case "AddTag":
                    AddTagCommand addTag = new AddTagCommand(tagService);
                    result = addTag.Execute(commandParameters);
                    break;

                case "CreateAlbum":
                    CreateAlbumCommand createAlbum = new CreateAlbumCommand(albumService, userService, tagService);
                    result = createAlbum.Execute(commandParameters);
                    break;

                case "AddTagTo":
                    AddTagToCommand addTagTo = new AddTagToCommand(albumService, tagService);
                    result = addTagTo.Execute(commandParameters);
                    break;

                case "MakeFriends":
                    MakeFriendsCommand makeFriends = new MakeFriendsCommand(userService);
                    result = makeFriends.Execute(commandParameters);
                    break;

                case "ListFriends":
                    ListFriendsCommand listFriends = new ListFriendsCommand(userService);
                    result = listFriends.Execute(commandParameters);
                    break;

                case "ShareAlbum":
                    ShareAlbumCommand shareAlbum = new ShareAlbumCommand(albumService, userService);
                    result = shareAlbum.Execute(commandParameters);
                    break;

                case "UploadPicture":
                    UploadPictureCommand uploadPicture = new UploadPictureCommand(albumService, pictureService);
                    result = uploadPicture.Execute(commandParameters);
                    break;

                case "Exit":
                    ExitCommand exit = new ExitCommand();
                    exit.Execute();
                    break;

                case "Login":
                    LoginUserCommand loginUser = new LoginUserCommand();
                    result = loginUser.Execute(commandParameters);
                    break;

                case "Logout":
                    LogoutUserCommand logoutUser = new LogoutUserCommand();
                    result = logoutUser.Execute(commandParameters);
                    break;

                default:
                    result = $"Command {commandName} not valid!";
                    break;
            }

            return result;
        }
    }
}