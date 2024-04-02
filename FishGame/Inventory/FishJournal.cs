using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using FishGame.Utils;
using CsvHelper.Configuration;

namespace FishGame.Inventory
{
    public enum FishLocation
    {
        Ocean,
        River,
        Pond,
    }

    public enum FishType
    {
        Fish,
        Creature,
    }

    public struct FishRecord
    {
        public string Name { get; set; }
        public int Idx { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public FishLocation Location { get; set; }

        public FishType Type { get; set; }
    }

    public sealed class FishDB : IGameComponent
    {
        private FishRecord[] fishRecords;
        private readonly Dictionary<string, int> fishNameLookup = new Dictionary<string, int>();

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
            string dataTablePath = $"{contentManager.RootDirectory}/fish_data_table.csv";
            using FileStream fileStream = new FileStream(dataTablePath, FileMode.Open);
            using TextReader textReader = new StreamReader(fileStream);
            using CsvHelper.CsvReader csv = new CsvHelper.CsvReader(textReader, new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                Delimiter = "\t",
            });

            var fishList = new List<FishRecord>();
            fishList.EnsureCapacity( 128 );
            foreach (var fishRecord in csv.GetRecords<FishRecord>())
            {
                if (fishRecord.Idx >= fishList.Count)
                    fishList.Resize(fishRecord.Idx + 1);

                fishList[fishRecord.Idx] = fishRecord;

                fishNameLookup.Add(fishRecord.Name, fishRecord.Idx);
            }

            fishRecords = fishList.ToArray();
        }

        public ref FishRecord GetFishById(int id)
        { 
            return ref fishRecords[id];
        }

        public int GetFishId(string name)
        {
            return fishNameLookup[name];
        }
    }


    public sealed class FishJournal
    {

    }
}
