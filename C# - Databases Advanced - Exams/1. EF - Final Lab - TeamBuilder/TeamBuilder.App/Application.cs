namespace TeamBuilder.App
{
    using Core;

    internal class Application
    {
        private static void Main(string[] args)
        {
            Engine engine = new Engine(new CommandDispatcher());
            engine.Run();
        }
    }
}