using System;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Newtonsoft.Json;

class MidiConverter
{
    static void Main()
    {

        string projectRoot = @"D:\Unity\RhytmTyping";

        string midiFolderPath = Path.Combine(projectRoot, "MidiToConvert");

        string assetsPath = Path.Combine(projectRoot, "GeneratedMaps");

        if (!Directory.Exists(midiFolderPath))
        {
            Console.WriteLine("Midi folder has not found!");
            return;
        }

        if (!Directory.Exists(assetsPath))
        {
            Directory.CreateDirectory(assetsPath);
        }

        string[] midiFiles = Directory.GetFiles(midiFolderPath, "*.mid");
        if (midiFiles.Length == 0)
        {
            Console.WriteLine("No MIDI files found");
            return;
        }

        string midiPath = midiFiles[0];

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"output_{timestamp}.json";
        string jsonPath = Path.Combine(assetsPath, fileName);


        var midiFile = MidiFile.Read(midiPath);
        var tempoMap = midiFile.GetTempoMap();

        List<NoteData> notes = new List<NoteData>();

        foreach (var note in midiFile.GetNotes())
        {
            double timeInSeconds = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap).TotalSeconds;
            notes.Add(new NoteData { time = Math.Round(timeInSeconds, 3) });
        }

        var jsonData = new
        {
            songName = "",
            artist = "",
            audioFile = Path.GetFileNameWithoutExtension(midiPath),
            bpm = 0,
            coverImage = "cover",
            notes = notes
        };

        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));

        Console.WriteLine($"Json file was created: {jsonPath}");
    }

    class NoteData
    {
        public double time { get; set; } 
    }
}
