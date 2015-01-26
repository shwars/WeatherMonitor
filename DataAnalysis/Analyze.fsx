#r @"bin\release\WeatherMonitorLib.dll"
#r "Microsoft.WindowsAzure.Storage"

open WeatherMonitorLib
let data = WeatherDB.GetData() |> Seq.toList

#load @"d:\winapp\lib\fsharp\FSharpChart-0.2\FSharpChart.fsx"
#load @"d:\winapp\lib\fsharp\FSharpChart-0.2\FSharpChartAutoDisplay.fsx"
open Samples.Charting

let DayLength (L : WeatherRecord seq) =
   let daymoments = L |> Seq.filter (fun x->x.Reading>700.0) |> Seq.map (fun x->x.When.TimeOfDay)
   if Seq.length daymoments=0 then 0.
   else
       let diff = (Seq.max daymoments)-(Seq.min daymoments)
       diff.TotalMinutes

data 
|> Seq.filter (fun x->x.WeatherInfoSource=WeatherInfoSource.Device && x.ReadingType=ReadingType.Luminocity)
|> Seq.sortBy (fun x->x.When)
|> Seq.groupBy (fun x->x.When.Date)
|> Seq.map (fun (x,l) -> (x,DayLength l))
|> Seq.filter (fun (x,l) -> l>400.0)
|> FSharpChart.Line

let s f x y = f y x

data 
|> Seq.filter (fun x->x.WeatherInfoSource=WeatherInfoSource.Device && x.ReadingType=ReadingType.Luminocity)
|> Seq.sortBy (fun x->x.When)
|> Seq.groupBy (fun x->x.When.Date)
|> Seq.map (fun (x,l) -> (x,DayLength l))
|> Seq.filter (fun (x,l) -> l>400.0)
|> Seq.map (snd >> s(/)60.)
|> Seq.toList


