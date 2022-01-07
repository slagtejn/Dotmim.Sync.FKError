using Dotmim.Sync.Enumerations;
using Dotmim.Sync.SqlServer;
using System;
using System.Data;

namespace Dotmim.Sync.FKError
{
    internal class Program
    {
        public static string serverDbName = "ServerDB";
        public static string clientDbName = "Client";

        private static async Task Main(string[] args)
        {
            var setup = new SyncSetup(new string[] { "FilterTable2", "FilterTable1" });

            foreach (var syncSetupTable in setup.Tables)
            {
                var filter = new SetupFilter(syncSetupTable.TableName);
                filter.AddParameter("FilterId", syncSetupTable.TableName);
                filter.AddWhere("FilterId", syncSetupTable.TableName, "FilterId");
                setup.Filters.Add(filter);
            }

            var serverProvider = new SqlSyncProvider(DBHelper.GetDatabaseConnectionString(serverDbName));
            var clientProvider = new SqlSyncProvider(DBHelper.GetDatabaseConnectionString(clientDbName));
            var options = new SyncOptions();
            await SynchronizeAsync(clientProvider, serverProvider, setup, options);
        }

        private static async Task SynchronizeAsync(CoreProvider clientProvider, CoreProvider serverProvider, SyncSetup setup, SyncOptions options)
        {
            var scopes = new List<string> { "FilterScope2", "FilterScope1" };
            var progress = new SynchronousProgress<ProgressArgs>(s =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{s.ProgressPercentage:p}:  \t[{s.Source[..Math.Min(4, s.Source.Length)]}] {s.TypeName}:\t{s.Message}");
                Console.ResetColor();
            });

            do
            {
                Console.Clear();

                foreach (var scope in scopes)
                {
                    Console.WriteLine("Sync start scope: " + scope);

                    var agent = new SyncAgent(clientProvider, serverProvider, options, setup, scope);

                    try
                    {
                        if (!agent.Parameters.Contains("FilterId"))
                            agent.Parameters.Add("FilterId", scope == "FilterScope1" ? 1 : 2);

                        var s = await agent.SynchronizeAsync(SyncType.Normal, progress);
                        Console.WriteLine(s);
                    }
                    catch (SyncException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("UNKNOW EXCEPTION : " + e.Message);
                    }
                }
                Console.WriteLine("Sync Ended. Press a key to start again, or Escapte to end");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

        }
    }
}