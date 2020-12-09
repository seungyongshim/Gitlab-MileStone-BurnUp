using System;
using System.Text.Json;
using System.Linq;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System.Threading.Tasks;
using ConsoleApp.Entity;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            foreach (var name in args)
            {

                var GraphQLClient = new GraphQLHttpClient(@"http://wish.mirero.co.kr" + @"/api/graphql", new SystemTextJsonSerializer());

                var issuesMilestoneResponse = await GraphQLClient.SendQueryAsync<IssuesMilestoneResponse>(new GraphQL.GraphQLRequest
                {
                    Query = Query.IssuesMilestone(name)
                });

                var validName =  string.Join("_", name.Split(Path.GetInvalidFileNameChars()));

                //var mileStone = MileStone.Create(issuesMilestoneResponse.Data, DateTime.Parse("2020-11-30"));
                var mileStone = MileStone.Create(issuesMilestoneResponse.Data, DateTime.Today);

                var plt = new ScottPlot.Plot(1280, 250);

                plt.PlotScatter(mileStone.IssuesCreateCounts.Select(x => x.Date.ToOADate()).ToArray(),
                                mileStone.IssuesCreateCounts.Select(y => Convert.ToDouble(y.Count)).ToArray(),
                                color: Color.Red,
                                label: "총 액션아이템");
                plt.PlotScatter(mileStone.IssuesCloseCounts.Select(x => x.Date.ToOADate()).ToArray(),
                                mileStone.IssuesCloseCounts.Select(y => Convert.ToDouble(y.Count)).ToArray(),
                                color: Color.Blue,
                                label: "해결된 액션아이템");

                plt.Grid(xSpacing: 1, xSpacingDateTimeUnit: ScottPlot.Config.DateTimeUnit.Day);

                plt.Legend();
                plt.Ticks(dateTimeX: true);
                plt.SetCulture(shortDatePattern: "M\\/dd");
                plt.SaveFig($"{validName}.png");


                static JsonContext Conv((DateTime dateTime, int Count) x, string c) => new(x.dateTime.Date.ToShortDateString(), x.Count, c);

                var create = from As in new[] {
                                                  mileStone.IssuesCreateCounts.Select(x => Conv(x, "0")),
                                                  mileStone.IssuesCloseCounts.Select(x => Conv(x, "1"))
                                              }
                             from a in As
                             select a;


                var json = JsonSerializer.Serialize(create);

                File.WriteAllText($"{validName}.json", json);
            }
        }
    }

    record JsonContext(string x, int y, string c);
}
