namespace BankSystemComplex
{
    using Core;
    using Data;

    //Solution added from Georgi on the website;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //BankSystemContext context = new BankSystemContext();
            //context.Database.Initialize(true);

            CommandDispatcher commandDispatcher = new CommandDispatcher();
            Engine engine = new Engine() { CommandDispatcher = commandDispatcher };
            engine.Run();
        }
    }
}
