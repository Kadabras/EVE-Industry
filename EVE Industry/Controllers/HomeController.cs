using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using EVE_Industry.EfStuff;
using EVE_Industry.EfStuff.DbModel;
using EVE_Industry.EfStuff.Repositories;
using EVE_Industry.Models;
using EVE_Industry.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EVE_Industry.Controllers
{
    //xPath herf /html//div[contains(@class,'dropdown-menu')]
    // small-games  div[contains(@id,'in-content')]//*[child::p]

    //https://eve-industry.org/calc/?q=Palad&techlevel=1&id=28660&runs=1&jobs=1&te=20&me=10&materials_modifier=1&cte=20&cme=10&c_materials_modifier=1&enc=5&dc1=5&dc2=5&decryptor=0&skill_m=0.80&implant_m=1.0&facility_m=1&solarSystem_m=Osmon&taxRate_m=10&skill_mc=0.80&implant_mc=1.0&facility_mc=1&solarSystem_mc=Osmon&taxRate_mc=10&skill_te=0.75&implant_te=1.0&facility_te=1&solarSystem_te=Osmon&taxRate_te=10&skill_me=0.75&implant_me=1.0&facility_me=1&solarSystem_me=Osmon&taxRate_me=10&skill_c=0.75&implant_c=1.0&facility_c=1&solarSystem_c=Osmon&taxRate_c=10&facility_i=1&solarSystem_i=Osmon&taxRate_i=10&advanced_industry=5
    //  id("materials") / tbody /tr [@class='type'] /td[1]


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IMapper _mapper;
        private MainIndustryRepository _mainIndustryRepository;
        private DumpRepository _dumpRepository;
        private DumpService _dumpService;
        private Random rand = new Random();


        public HomeController(
            ILogger<HomeController> logger,
            IMapper mapper,
            MainIndustryRepository mainIndustryRepository,
            DumpRepository dumpRepository,
            DumpService dumpService)
        {
            _logger = logger;
            _mapper = mapper;
            _mainIndustryRepository = mainIndustryRepository;
            _dumpRepository = dumpRepository;
            _dumpService = dumpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> EveResult()
        {

            //var html = "https://eve-industry.org/calc/getInfo/?id=41377&me=2&te=10&runs=1&jobs=1&enc=5&dc1=5&dc2=5&decryptor=0&techlevel=1&byoc=false&buildcopy=false&cme=10&cte=20&advanced_industry=5&materials_modifier=1&c_materials_modifier=1&skill_m=0.80&skill_mc=0.80&skill_te=0.75&skill_me=0.75&skill_c=0.75&skill_i=1.0&implant_m=1.0&implant_mc=1.0&implant_te=1.0&implant_me=1.0&implant_c=1.0&implant_i=1.0&facility_m=1&facility_mc=1&facility_te=1&facility_me=1&facility_c=1&facility_i=1&solarSystem_m=Ikuchi&solarSystem_mc=Osmon&solarSystem_te=Osmon&solarSystem_me=Osmon&solarSystem_c=Osmon&solarSystem_i=Osmon&taxRate_m=10&taxRate_mc=10&taxRate_te=10&taxRate_me=10&taxRate_c=10&taxRate_i=10&pi=false";

            var html = "https://eve-industry.org/calc/getInfo/?" +
                "id=" + _mainIndustryRepository.Get(rand.Next(0, _mainIndustryRepository.Count())).TypeId + 1 +
                "&me=2&te=10&runs=1&jobs=1&enc=5&dc1=5&dc2=5&decryptor=0&techlevel=1&byoc=false&buildcopy=false&cme=10&cte=20&advanced_industry=5&materials_modifier=1&c_materials_modifier=1&skill_m=0.80&skill_mc=0.80&skill_te=0.75&skill_me=0.75&skill_c=0.75&skill_i=1.0&implant_m=1.0&implant_mc=1.0&implant_te=1.0&implant_me=1.0&implant_c=1.0&implant_i=1.0&facility_m=1&facility_mc=1&facility_te=1&facility_me=1&facility_c=1&facility_i=1&solarSystem_m=Ikuchi&solarSystem_mc=Osmon&solarSystem_te=Osmon&solarSystem_me=Osmon&solarSystem_c=Osmon&solarSystem_i=Osmon&taxRate_m=10&taxRate_mc=10&taxRate_te=10&taxRate_me=10&taxRate_c=10&taxRate_i=10&pi=false";

            using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            using var doc = await context.OpenAsync(html);

            //var parser = new HtmlParser();
            //var document = parser.ParseDocument(html);

            var profit = doc.GetElementsByClassName("isk").Children("strong").Children("span").First().TextContent;

            var findTime = doc.GetElementsByClassName("info")
                .Skip(2)
                .SkipLast(1)
                .Children("tbody")
                .Children("tr")
                .Children("td")
                .Select(x => x.TextContent)
                .ToList();

            var time = findTime[7];
            var profitPerHour = findTime[8];


            return Content(time + profitPerHour);
        }

        [HttpGet]
        public IActionResult AddFile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFile(FileViewModel fileViewModel)
        {

            var result = new StringBuilder();


            using (var reader = new StreamReader(fileViewModel.File.OpenReadStream()))
            {
                MainIndustryCell mainIndustryCell = new MainIndustryCell();
                //var csv = new CsvReader(reader, config);

                var cell = new List<string>();
                var getAll = _mainIndustryRepository.GetAll();

                while (reader.Peek() >= 0)
                //result.AppendLine(reader.ReadLine());
                {
                    cell = reader.ReadLine().Split(';').ToList();

                    result.AppendLine("\n" + cell[0] + cell[1] + cell[2]);


                    mainIndustryCell.Id = 0;
                    mainIndustryCell.TypeId = int.Parse(cell[0]);
                    mainIndustryCell.Name = cell[1];
                    mainIndustryCell.TypeItem = Enum.Parse<TypeItem>(cell[2]);//cell[2];
                    mainIndustryCell.MaterialEfficiency = 10;
                    mainIndustryCell.TimeEfficiency = 20;


                    if (!getAll.Any(x => x.TypeId == mainIndustryCell.TypeId))
                    {
                        _mainIndustryRepository.Save(mainIndustryCell);
                    }
                }

            }


            return Content(result.ToString());
        }

        public async Task<IActionResult> TryDump(int id)
        {

            var html = "https://eve-industry.org/calc/getInfo/?" +
    "id=" + id +
    "&me=2&te=10&runs=1&jobs=1&enc=5&dc1=5&dc2=5&decryptor=0&techlevel=1&byoc=false&buildcopy=false&cme=10&cte=20&advanced_industry=5&materials_modifier=1&c_materials_modifier=1&skill_m=0.80&skill_mc=0.80&skill_te=0.75&skill_me=0.75&skill_c=0.75&skill_i=1.0&implant_m=1.0&implant_mc=1.0&implant_te=1.0&implant_me=1.0&implant_c=1.0&implant_i=1.0&facility_m=1&facility_mc=1&facility_te=1&facility_me=1&facility_c=1&facility_i=1&solarSystem_m=Ikuchi&solarSystem_mc=Osmon&solarSystem_te=Osmon&solarSystem_me=Osmon&solarSystem_c=Osmon&solarSystem_i=Osmon&taxRate_m=10&taxRate_mc=10&taxRate_te=10&taxRate_me=10&taxRate_c=10&taxRate_i=10&pi=false";

            using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            using var doc = await context.OpenAsync(html);

            var name = doc.QuerySelector("strong")?.TextContent;

            if (name != null)
            {
                using var itemIdRequest = await context.OpenAsync("https://www.fuzzwork.co.uk/api/typeid2.php?format=xml&typename=" + name);

                var itemId = itemIdRequest.QuerySelector("row").GetAttribute("typeID");

                var dumpCell = new DumpCell
                {
                    ParsedId = id,
                    Name = name,
                    TypeId = int.Parse(itemId)
                };

                var profit = doc.GetElementsByClassName("isk")?.Children("strong").Children("span").First().TextContent;

                if (profit != null)
                {
                    dumpCell.Profit = int.Parse(profit.Split(" ")[0].Split(".")[0].Replace(",", ""));
                }

                _dumpRepository.Save(dumpCell);
            }


            return Json(true);
        }

        public async Task<IActionResult> TryProfit(int siteId)
        {

            var html = "https://eve-industry.org/calc/getInfo/?" +
    "id=" + siteId +
    "&me=2&te=10&runs=1&jobs=1&enc=5&dc1=5&dc2=5&decryptor=0&techlevel=1&byoc=false&buildcopy=false&cme=10&cte=20&advanced_industry=5&materials_modifier=1&c_materials_modifier=1&skill_m=0.80&skill_mc=0.80&skill_te=0.75&skill_me=0.75&skill_c=0.75&skill_i=1.0&implant_m=1.0&implant_mc=1.0&implant_te=1.0&implant_me=1.0&implant_c=1.0&implant_i=1.0&facility_m=1&facility_mc=1&facility_te=1&facility_me=1&facility_c=1&facility_i=1&solarSystem_m=Ikuchi&solarSystem_mc=Osmon&solarSystem_te=Osmon&solarSystem_me=Osmon&solarSystem_c=Osmon&solarSystem_i=Osmon&taxRate_m=10&taxRate_mc=10&taxRate_te=10&taxRate_me=10&taxRate_c=10&taxRate_i=10&pi=false";

            using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            using var doc = await context.OpenAsync(html);

            var profit = doc.GetElementsByClassName("isk")?.Children("strong").Children("span").First().TextContent;

            if (profit != null)
            {
                var dumpCell = _dumpRepository.GetAll().Single(x => x.ParsedId == siteId);

                dumpCell.Profit = int.Parse(profit.Split(" ")[0].Split(".")[0].Replace(",", ""));

                _dumpRepository.Save(dumpCell);
            }

            return Json(true);
        }


        public IActionResult StartDump()
        {
            var getAll = _dumpRepository.GetAll();

            if (getAll.Count == 0)
            {
                _dumpService.StartDumpTask(1);

            }
            else
            {
                var lastId = getAll.Last().ParsedId;
                _dumpService.StartDumpTask(lastId);

            }

            return RedirectToAction("Index");
        }

        public IActionResult StartProfit()
        {
            var getAll = _dumpRepository.GetAll();

            _dumpService.StartProfitTask(getAll);

            return RedirectToAction("Index");
        }


        public IActionResult CheckDumpTask()
        {
            var content = "";

            var dumpTask = new DumpTaskModel();


            if (DumpService.DumpTasks.Any(x => x.Id == DumpService.DUMP_TASK_ID))
            {
                dumpTask = DumpService.DumpTasks.First(x => x.Id == DumpService.DUMP_TASK_ID);

                lock (DumpService.DumpTasks)
                {
                    content = dumpTask.siteId.ToString();
                }

            }

            content += "\n" + _dumpRepository.Count();

            foreach (var cell in _dumpRepository.GetAll().GetRange(_dumpRepository.GetAll().Count - 100, 100))
            {
                content += "\n" + cell.Id + "  " + cell.TypeId + "  " + cell.ParsedId + "  " + cell.Name;
            }

            return Content(content);
        }


        public IActionResult CheckProfitTask()
        {
            var content = "";
            var currentCell = 0;
            var dumpTask = new DumpTaskModel();


            if (DumpService.DumpTasks.Any(x => x.Id == DumpService.PROFIT_TASK_ID))
            {
                dumpTask = DumpService.DumpTasks.First(x => x.Id == DumpService.PROFIT_TASK_ID);

                lock (DumpService.DumpTasks)
                {
                    currentCell = dumpTask.siteId;
                    content = currentCell.ToString();
                }

            }


            foreach (var cell in _dumpRepository.GetAll())
            {
                if(cell.ParsedId <= currentCell + 100 && cell.ParsedId >= currentCell - 100)
                content += "\n" + cell.ParsedId + "  " + cell.Name + "  " + cell.Profit;
            }

            return Content(content);
        }


        public IActionResult Industry(string id)
        {
            if (id == "iddqd")
                return View();
            else 
                return RedirectToAction("Index");
        }

        public IActionResult CancelDumpTask()
        {
            if (DumpService.DumpTasks.Any(x => x.Id == DumpService.DUMP_TASK_ID))
            {
                var dumpTask = DumpService.DumpTasks.First(x => x.Id == DumpService.DUMP_TASK_ID);

                dumpTask.CancellationTokenSource.Cancel();

                lock (DumpService.DumpTasks)
                {
                    DumpService.DumpTasks.Remove(dumpTask);
                }

            }
            return RedirectToAction("Index");
        }

        public IActionResult CancelProfitTask()
        {
            if (DumpService.DumpTasks.Any(x => x.Id == DumpService.PROFIT_TASK_ID))
            {
                var dumpTask = DumpService.DumpTasks.First(x => x.Id == DumpService.PROFIT_TASK_ID);

                dumpTask.CancellationTokenSource.Cancel();

                lock (DumpService.DumpTasks)
                {
                    DumpService.DumpTasks.Remove(dumpTask);
                }

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MainItemTask()
        {

            var mainIndustryTable = _mainIndustryRepository.GetAll();
            var dumpTable = _dumpRepository.GetAll();

            foreach(var cell in mainIndustryTable)
            {
                if(cell.ParsedId == 0)
                {
                    var dumpParsedId = dumpTable.SingleOrDefault(x => x.TypeId == cell.TypeId);

                    if(dumpParsedId != null)
                    {
                        cell.ParsedId = (int)dumpParsedId.ParsedId;

                        _mainIndustryRepository.Save(cell);
                    }
                    else
                    {
                        var dd = cell.TypeId;
                    }

                }

            }

            foreach(var cell in mainIndustryTable)
            {
                cell.MaterialEfficiency = 10;
                cell.TimeEfficiency = 20;

                var html = "https://eve-industry.org/calc/getInfo/?" +
            "id=" + cell.ParsedId +
            "&me=" + cell.MaterialEfficiency +
            "&te=" + cell.ManufacturingTime +
            "&runs=1&jobs=1&enc=5&dc1=5&dc2=5&decryptor=0&techlevel=1&byoc=false&buildcopy=false&cme=10&cte=20&advanced_industry=5&materials_modifier=1&c_materials_modifier=1&skill_m=0.80&skill_mc=0.80&skill_te=0.75&skill_me=0.75&skill_c=0.75&skill_i=1.0&implant_m=1.0&implant_mc=1.0&implant_te=1.0&implant_me=1.0&implant_c=1.0&implant_i=1.0&facility_m=1&facility_mc=1&facility_te=1&facility_me=1&facility_c=1&facility_i=1&solarSystem_m=Ikuchi&solarSystem_mc=Osmon&solarSystem_te=Osmon&solarSystem_me=Osmon&solarSystem_c=Osmon&solarSystem_i=Osmon&taxRate_m=10&taxRate_mc=10&taxRate_te=10&taxRate_me=10&taxRate_c=10&taxRate_i=10&pi=false";

                using var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                using var doc = await context.OpenAsync(html);

                //var parser = new HtmlParser();
                //var document = parser.ParseDocument(html);

                var profit = doc.GetElementsByClassName("isk")?.Children("strong").Children("span").First().TextContent;

                var findTime = doc.GetElementsByClassName("info")
                    .Skip(2)
                    .SkipLast(1)
                    .Children("tbody")
                    .Children("tr")
                    .Children("td")
                    .Select(x => x.TextContent)
                    .ToList();

                var profitPerHour = "";

                for(var i = 0; i < findTime.Count(); i++)
                {
                    var findCell = findTime[i];
                    if (findCell.Contains("ISK"))
                    {
                        profitPerHour = findCell;
                        break;
                    }
                }


                /*
                var time = findTime[fghfghfjhjhgjfgjfgj].Split();
                string[] correctTime = new string[5];

                for(var i = correctTime.Count() - 1; i >=0 ; i--)
                {
                    var ii = correctTime.Count() - i - 1;

                    for (var k = time.Count() - 1 - ii; k >= 0 ; k--)
                    {
                        var cellTime = time[k];

                        if (cellTime.Contains("w"))
                        {
                            correctTime[i] = cellTime.Replace("w", "");
                            break;
                        } 

                        if (cellTime.Contains("d"))
                        {
                            correctTime[i] = cellTime.Replace("d", "");
                            break;
                        }

                        if (cellTime.Contains("h"))
                        {
                            correctTime[i] = cellTime.Replace("h", "");
                            break;
                        }

                        if (cellTime.Contains("m"))
                        {
                            correctTime[i] = cellTime.Replace("m", "");
                            break;
                        }

                        if (cellTime.Contains("s"))
                        {
                            correctTime[i] = cellTime.Replace("s", "");
                            break;
                        }
                        
                    }

                    if(correctTime[i] == null) correctTime[i] = "00";
                }

                var newTime = string.Join(":", correctTime);
                */
                cell.Profit = int.Parse(profit.Split(" ")[0].Split(".")[0].Replace(",", ""));
                //cell.ManufacturingTime = TimeSpan.Parse("00:03:01:7:39");
                cell.ProfitPerHour = int.Parse(profitPerHour.Split(" ")[0].Split(".")[0].Replace(",", ""));

                _mainIndustryRepository.Save(cell);
            }

            return View();
        }

    }
}


