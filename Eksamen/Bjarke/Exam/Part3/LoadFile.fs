module Part3.LoadFile

open System
open System.IO
open System.Text

let file fileName = 
    let fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
    let fileStreamReader = new StreamReader(fileStream, Encoding.UTF8)
    
    let text = fileStreamReader.ReadToEnd()
    
    match text with
    | text when String.IsNullOrEmpty text -> None
    | text -> Some text