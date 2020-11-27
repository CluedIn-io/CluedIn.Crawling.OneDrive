
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Core.Logging;
using CluedIn.Crawling.OneDrive.Core;
using CrawlerIntegrationTesting.Clues;
using CrawlerIntegrationTesting.Log;
using Xunit.Abstractions;
using DebugCrawlerHost = CrawlerIntegrationTesting.CrawlerHost.DebugCrawlerHost<CluedIn.Crawling.OneDrive.Core.OneDriveCrawlJobData>;

namespace CluedIn.Crawling.OneDrive.Integration.Test
{
    public class OneDriveTestFixture
    {
        public ClueStorage ClueStorage { get; }
        private readonly DebugCrawlerHost debugCrawlerHost;

        public TestLogger Log { get; }
        public OneDriveTestFixture()
        {
            var executingFolder = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(8)).DirectoryName;
            debugCrawlerHost = new DebugCrawlerHost(executingFolder, OneDriveConstants.ProviderName);

            ClueStorage = new ClueStorage();

            Log = debugCrawlerHost.AppContext.Container.Resolve<TestLogger>();

            debugCrawlerHost.ProcessClue += ClueStorage.AddClue;
            
            var debugAgentJobProcessorState = new CrawlerIntegrationTesting.CrawlerHost.DebugAgentJobProcessorState<OneDriveCrawlJobData>
            {
                Log = new LoggingTargetLogger(this.Log),
                CancellationTokenSource = new System.Threading.CancellationTokenSource(),
                JobData = new OneDriveCrawlJobData(OneDriveConfiguration.Create()),
                Status = new AgentJobStatus(),
                TaskFactory = new TaskFactory(),
                Result = new AgentJobResult()
            };
            this.debugCrawlerHost.AppContext.Container.Register(Component.For<IAgentJobProcessorState<OneDriveCrawlJobData>>().Instance(debugAgentJobProcessorState));


            debugCrawlerHost.Execute(OneDriveConfiguration.Create(), OneDriveConstants.ProviderId);
        }

        public void PrintClues(ITestOutputHelper output)
        {
            foreach(var clue in ClueStorage.Clues)
            {
                output.WriteLine(clue.OriginEntityCode.ToString());
            }
        }

        public void PrintLogs(ITestOutputHelper output)
        {
            output.WriteLine(Log.PrintLogs());
        }

        public void Dispose()
        {
        }

    }
}


