using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Repositories.SQLServer;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Employee, Manager, Department Manager")]
    public class ChartsController : Controller
    {
        private SQLServerProjectRepository db;

        public ChartsController()
        {
            db = new SQLServerProjectRepository();
        }

        public int Cyan { get; private set; }
        public int DarkRed { get; private set; }
        public int G { get; private set; }
        public int HotPink { get; private set; }
        public Color? LightGreen { get; private set; }

        public ActionResult Index()
        {
            //Note.RP : Eventually, it will be projects for the current user instead of get all projects 

            var user = new ApplicationUser();
            Employee empdata = new Employee();

            if (User.Identity.IsAuthenticated)
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                user = manager.FindById(User.Identity.GetUserId());
                var serverrepo = new SQLServerEmployeeRepository();
                empdata = serverrepo.GetFullEmployeeDataById(Guid.Parse(user.Id));
            }

            var allProjects = db.GetAllProjects();
            var myProjects = db.GetAllProjectsForUser(Guid.Parse(User.Identity.GetUserId()));
            var projectBudgets = new List<ProjectBudget>();
            var projectTimes = new List<TimeCompletion>(); //for project completion based on tme
            var personalProjectSuccess = new List<Successes>();
           // int lowestMin = int.MaxValue;
            foreach (var project in allProjects)
            {
                /*if (project.Budget < lowestMin)
                {
                    lowestMin = project.Budget;
                }
                else;
                if (project.CurrentSpent < lowestMin)
                {
                    lowestMin = project.CurrentSpent;
                }
                else;*/
                projectBudgets.Add(new ProjectBudget
                {
                    Name = project.Name,
                    Budget = project.Budget,
                    Spent = project.CurrentSpent
                    


                });
                //copy pasted code
                if (project.Completed == true)
                {
                    projectTimes.Add(new TimeCompletion
                    {
                        Name = project.Name,
                        CompletedOn = project.CompletedOn.GetValueOrDefault(DateTime.Now) ,
                        Completed = project.Completed,
                        DueDate = project.DueDate
                    });
                }
                else if(project.Completed == false)
                {
                    projectTimes.Add(new TimeCompletion
                    {
                        Name = project.Name,
                        CompletedOn = DateTime.Now,
                        Completed = project.Completed,
                        DueDate = project.DueDate
                    });

                }
            }

            //modify data type to make it of array type
            var xDataNames = projectBudgets.Select(i => i.Name).ToArray();
            var yDataBudgetsColumn = projectBudgets.Select(i => new object[] { i.Budget }).ToArray();
            var yDataBudgetsColumnFiller = projectBudgets.Select(i => new object[] { i.Spent }).ToArray();
            var yDataBudgetsPie = projectBudgets.Select(i => new object[] { i.Name, i.Budget }).ToArray();
            var xDataNames2 = projectTimes.Select(i => i.Name).ToArray();
            var yDataCompletionColumn = projectTimes.Select(i => new object[] { i.Completed }).ToArray();
            //instanciate an object of the Highcharts type
            var columnChart = new Highcharts("ColumnChart")
                //define the type of chart 
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                //overall Title of the chart 
                .SetTitle(new Title { Text = "Individual Project Budget for a department" })
                //small label below the main Title
                .SetSubtitle(new Subtitle { Text = "Department name" })
                //load the X values
                .SetXAxis(new XAxis { Categories = xDataNames })
                //set the Y title
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Budget amount in $" } })
                
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function() { return '<b>'+ this.series.name +': '+ this.y; }"

                })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        Grouping = true,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,

                        },
                        EnableMouseTracking = false
                    }
                })
                //load the Y values 
                .SetSeries(new[]
                {
                    new Series {Name = "Budget", Data = new Data(yDataBudgetsColumn)},
                    new Series {Name = "Spent", Data= new Data(yDataBudgetsColumnFiller) }
                    //you can add more y data to create a second line
                    // new Series { Name = "Other Name", Data = new Data(OtherData) }
                });

            var pieChart = new Highcharts("PieChart")
                //define the type of chart 
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie })
                //overall Title of the chart 
                .SetTitle(new Title { Text = "Individual Project Budget for a Department" })
                //small label below the main Title
                .SetSubtitle(new Subtitle { Text = "Department name" })
                //load the X values
                .SetXAxis(new XAxis { Categories = xDataNames })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function() { return '<b>'+ this.series.name +': '+ this.y; }"
                })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = false,
                        
                    }
                })
                //load the Y values 
                .SetSeries(new[]
                {
                    new Series {Name = "Budget", Data = new Data(yDataBudgetsPie) }
                    //you can add more y data to create a second line
                    // new Series { Name = "Other Name", Data = new Data(OtherData) }
                });

            var charts = new List<Highcharts>();
            charts.Add(columnChart);
            charts.Add(pieChart);



            //begin copy pasted code for column chart

            String[] timeStuff = new string[] { "Not Yet Decided", "On Time", "Ahead", "Late" };
            var timeValues = new int[] { 0, 0, 0, 0 };
            foreach (var TimeCompletion in projectTimes)//fill our bars FOR LOOP IN QUESTION HOW TO MAKE DYNAMIC
            {
                if (TimeCompletion.Completed == false && TimeCompletion.DueDate.Date > DateTime.Now.Date)
                {
                    timeValues[0]++;
                }
                else if (TimeCompletion.Completed == true && TimeCompletion.DueDate.Date > TimeCompletion.CompletedOn.Date)
                {
                    timeValues[2]++;
                }
                else if (TimeCompletion.Completed == true && TimeCompletion.DueDate.Date == TimeCompletion.CompletedOn.Date)
                {
                    timeValues[1]++;
                }
                else if (TimeCompletion.Completed == true && TimeCompletion.DueDate.Date < TimeCompletion.CompletedOn.Date)
                {
                    
                    timeValues[3]++;
                }
                else if(TimeCompletion.Completed == false && TimeCompletion.DueDate < DateTime.Now)
                {
                    timeValues[3]++;
                }
            }
            //make timeValues an object
            object[] graphDataforTime = timeValues.Cast<object>().ToArray();

            //instanciate an object of the Highcharts type
            var columnChartProjectCompletionTime = new Highcharts("ColumnChart2")
                //define the type of chart 
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                //overall Title of the chart 
                .SetTitle(new Title { Text = "Projects Completion Based on Time" })
                //small label below the main Title
                .SetSubtitle(new Subtitle { Text = "Department name" })
                
                //load the X values
                .SetXAxis(new XAxis { Categories = timeStuff })
                //set the Y title
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Number of Projects" } })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; }"
                })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    
                    {
                        Grouping = false,
                        Color = LightGreen,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true
                           
                        },
                        EnableMouseTracking = false,
                        
                    }
                })
                
                //load the Y values 
                .SetSeries(new[]
                {
                    new Series {Name = "Amount", Data = new Data(graphDataforTime) }
                    //you can add more y data to create a second line
                    // new Series { Name = "Other Name", Data = new Data(OtherData) }
                });



            //end copy pasted

            charts.Add(columnChartProjectCompletionTime);

            return View(charts);
        }

    }
}