﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using FishGame.Utils;
using CsvHelper.Configuration;
using FishGame.Entities;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace FishGame.Inventory
{
    public enum FishType
    {
        Fish,
        Creature,

        COUNT,
    }

    public struct FishRecord
    {
        public string Name { get; set; }
        public int Idx { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Vector2 TexMapLocation => new Vector2(X, Y);

        public Location Location { get; set; }

        public FishType Type { get; set; }

        public Season Season { get; set; }
    }

    public sealed class FishTexUtils
    {
        public static readonly Vector2 FishTileDim = new Vector2(16, 16);

        public static Rectangle GetFishTilePxRect(FishDB db, int id)
        {
            ref var record = ref db.GetFishById(id);
            return new Rectangle(new Point(record.X, record.Y) * FishTileDim.ToPoint(), FishTileDim.ToPoint());
        }

        public static void DrawFish(SpriteBatch sb, Texture2D fishTex, FishDB db, int id, Vector2 location)
        {
            sb.Draw(fishTex, new Rectangle((int)location.X, (int)location.Y, 16, 16), GetFishTilePxRect(db, id), Color.White);
        }
    }

    public sealed class FishDB : IGameComponent
    {
        private FishRecord[] fishRecords;
        private List<int>[] fishByType;
        private List<int>[] fishByLocation;
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

            fishByType = new List<int>[(int)FishType.COUNT];
            for (int i = 0; i < fishByType.Length; i++)
            {
                fishByType[i] = new List<int>();
            }
            foreach (var fishRecord in fishRecords)
            {
                fishByType[(int)fishRecord.Type].Add(fishRecord.Idx);
            }

            fishByLocation = new List<int>[Enum.GetValues<Location>().Length];
            for (int i = 0; i < fishByLocation.Length; i++)
            {
                fishByLocation[i] = new List<int>();
            }
            foreach (var fishRecord in fishRecords)
            {
                fishByLocation[System.Numerics.BitOperations.Log2((uint)fishRecord.Location)].Add(fishRecord.Idx);
            }
        }

        public ref FishRecord GetFishById(int id)
        { 
            return ref fishRecords[id];
        }

        public int Count => fishRecords.Length;

        public int GetFishId(string name)
        {
            return fishNameLookup[name];
        }

        public IReadOnlyList<int> GetFishForLocation(Location location)
        {
            return fishByLocation[System.Numerics.BitOperations.Log2((uint)location)];
        }

        public IReadOnlyList<int> GetFishOfType(FishType type)
        {
            return fishByType[(int)type];
        }
    }

    public struct FishInventoryEntry
    {
        public bool HasCollected { get; set; }
    }

    public sealed class FishJournal
    {
        private readonly FishInventoryEntry[] fish;
        private readonly FishDB fishDB;

        public FishJournal(FishDB fishDB)
        {
            fish = new FishInventoryEntry[fishDB.Count];
            this.fishDB = fishDB;
        }

        public ref FishInventoryEntry GetInvSlot(int id)
        {
            return ref fish[id];
        }

        public float GetCompletionPercent()
        {
            return ((float)fish.Count(f => f.HasCollected)) / fish.Count();
        }

        private static readonly Vector2 InvScaling = new Vector2(2, 2);

        public void Draw(SpriteBatch spriteBatch, Texture2D missingTex, Texture2D collectedTex, Vector2 upperLeft, Season season, Location location)
        {
            for (int i = 0; i < fish.Length; i++)
            {
                ref FishInventoryEntry entry = ref fish[i];
                ref FishRecord record = ref fishDB.GetFishById(i);
                Rectangle srcRec = FishTexUtils.GetFishTilePxRect(fishDB, i);
                
                // Calculate where we are placing the sprite.
                Vector2 offset = FishTexUtils.FishTileDim * record.TexMapLocation * InvScaling;
                Vector2 loc = upperLeft + offset;
                Rectangle dstRec = new Rectangle(loc.ToPoint(), (FishTexUtils.FishTileDim * InvScaling).ToPoint());

                Texture2D tex = entry.HasCollected ? collectedTex : missingTex;

                bool isInSeason = (season & record.Season) != Season.None;
                bool isInLocation = (location & record.Location) != Location.None;
                Color tint = isInSeason && isInLocation ? Color.White : new Color(255, 255, 255, 128);

                spriteBatch.Draw(tex, dstRec, srcRec, tint);
            }
        }

        public int? QueryHover(Vector2 upperLeft, Vector2 point)
        {
            for (int i = 0; i < fish.Length; i++)
            {
                ref FishInventoryEntry entry = ref fish[i];
                ref FishRecord record = ref fishDB.GetFishById(i);

                // Calculate where we are placing the sprite.
                Vector2 offset = FishTexUtils.FishTileDim * record.TexMapLocation * InvScaling;
                Vector2 loc = upperLeft + offset;
                Rectangle dstRec = new Rectangle(loc.ToPoint(), (FishTexUtils.FishTileDim * InvScaling).ToPoint());

                if (dstRec.Contains(point))
                {
                    return i;
                }
            }

            return null;
        }
    }
}
