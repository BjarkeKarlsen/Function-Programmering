module Part3.Type

open System

type Exercise = {
    StartTime: DateTime
    Total: float
    Distance: float   
}
    
type Measurement = {
    Time: DateTime
    DistanceTraveled: float
    Cadence: int
    Speed: float
    Watt: int
}

type TrainingSession = {
    Exercise: Exercise
    Measurement: Measurement list
}