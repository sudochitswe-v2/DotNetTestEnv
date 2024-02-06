// See https://aka.ms/new-console-template for more information
using CSDotNetTranning.ConsoleApp.AdoDotNetExamples;
using CSDotNetTranning.ConsoleApp.DapperExamples;

//var adoDotNet = new AdoDotNetExample();
//adoDotNet.Edit(11);
//adoDotNet.Create("new","new","new");
//adoDotNet.Update(7,"update","new","new");
//adoDotNet.Delete(8);
var dapper = new DapperExample();
//dapper.Create("test", "testAuthor", "Content");
dapper.Edit(6);
//dapper.Update(6, "UpdateTitle", string.Empty, string.Empty);
//dapper.Delete(7);
Console.ReadKey();