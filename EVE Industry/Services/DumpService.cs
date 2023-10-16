using AngleSharp;
using EVE_Industry.EfStuff.DbModel;
using EVE_Industry.EfStuff.Repositories;
using EVE_Industry.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EVE_Industry.Services
{
    public class DumpService
    {
        private Random _random = new Random();

        public static List<DumpTaskModel> DumpTasks = new List<DumpTaskModel>();
        public static int DUMP_TASK_ID = 10;
        public static int PROFIT_TASK_ID = 20;

        private IHttpContextAccessor _httpContextAccessor;
        private DumpRepository _dumpRepository;

        public DumpService(IHttpContextAccessor httpContextAccessor, 
            DumpRepository dumpRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _dumpRepository = dumpRepository;
        }

        public void StartDumpTask(int id)
        {

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            DumpTaskModel taskModel;

            var path = _httpContextAccessor.HttpContext.Request.Host.ToUriComponent();

            lock (DumpTasks)
            {
                if (!DumpTasks.Any(x => x.Id == DUMP_TASK_ID))
                {
                    taskModel = new DumpTaskModel
                    {
                        Id = DUMP_TASK_ID,
                        CancellationTokenSource = cancelTokenSource
                    };

                    DumpTasks.Add(taskModel);

                    Task task = new Task(() => DumpTaskAsync(taskModel, id, path), token);

                    task.Start();
                }
            }
        }


        private async Task DumpTaskAsync(DumpTaskModel taskModel, int lastId, string path)
        {
            var client = new HttpClient();

            lastId++;

            while (lastId < 100000)
            {

                taskModel.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                taskModel.siteId = lastId;

                await client.GetAsync("http://" + path + "/Home/TryDump?id=" + lastId);

                //using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                //using var doc = await context.OpenAsync("http://localhost/Home/TryDump?id=" + lastId);


                lastId++;
                Thread.Sleep(1000);
            }

            lock (DumpTasks)
            {
                DumpTasks.Remove(taskModel);
            }
        }


        public void StartProfitTask(List<DumpCell> dumpCells)
        {

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            DumpTaskModel taskModel;

            var path = _httpContextAccessor.HttpContext.Request.Host.ToUriComponent();

            lock (DumpTasks)
            {
                if (!DumpTasks.Any(x => x.Id == PROFIT_TASK_ID))
                {
                    taskModel = new DumpTaskModel
                    {
                        Id = PROFIT_TASK_ID,
                        CancellationTokenSource = cancelTokenSource
                    };

                    DumpTasks.Add(taskModel);

                    Task task = new Task(() => ProfitTaskAsync(taskModel, dumpCells, path), token);

                    task.Start();
                }
            }
        }


        private async Task ProfitTaskAsync(DumpTaskModel taskModel, List<DumpCell> dumpCells, string path)
        {
            var client = new HttpClient();

            foreach(var cell in dumpCells)
            {
                taskModel.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                taskModel.siteId = cell.ParsedId;

                await client.GetAsync("http://" + path + "/Home/TryProfit?siteId=" + cell.ParsedId);

                //using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                //using var doc = await context.OpenAsync("http://localhost/Home/TryDump?id=" + lastId);

                Thread.Sleep(1000);
            }

            lock (DumpTasks)
            {
                DumpTasks.Remove(taskModel);
            }
        }


    }
}
