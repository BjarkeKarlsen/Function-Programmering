module Problem3.Type

open System

// For more information see https://aka.ms/fsharp-console-apps
type Exercises = {
     StartTime: DateTime
     TotalTime: float
     Distance:  float
     }
    
    
type Measurement ={
    Time: DateTime
    DistanceTraveled: float
    Cadence: int
    Speed: float
    Watt: float
    }
    