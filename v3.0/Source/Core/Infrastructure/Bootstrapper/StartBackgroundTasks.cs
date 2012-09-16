namespace Kigg.Infrastructure
{
    public class StartBackgroundTasks : IBootstrapperTask
    {
        private readonly IBackgroundTask[] _tasks;

        public StartBackgroundTasks(IBackgroundTask[] tasks)
        {
            Check.Argument.IsNotEmpty(tasks, "tasks");

            _tasks = tasks;
        }

        public void Execute()
        {
            _tasks.ForEach(t => t.Start());
        }
    }
}